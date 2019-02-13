/// <binding />
var gulp = require("gulp");
var msbuild = require("gulp-msbuild");
var debug = require("gulp-debug");
var foreach = require("gulp-foreach");
var rename = require("gulp-rename");
var newer = require("gulp-newer");
var util = require("gulp-util");
var runSequence = require("run-sequence");
var nugetRestore = require('gulp-nuget-restore');
var fs = require('fs');
var concat = require("gulp-concat");
var cssmin = require("gulp-cssmin");
var uglify = require('gulp-uglifyes');
var merge = require("merge-stream");
var yargs = require("yargs").argv;
var unicorn = require("./scripts/unicorn.js");
var habitat = require("./scripts/habitat.js");
var helix = require("./scripts/helix.js");
var flatten = require('gulp-flatten');
var del = require('del');
var config;
var bundleconfig = require("./bundleconfig.json");
var exec = require("child_process").exec;
if (fs.existsSync('./gulp-config.js.user')) {
    config = require("./gulp-config.js.user")();
}
else {
    config = require("./gulp-config.js")()
}
module.exports.config = config;

helix.header("The Habitat source code, tools and processes are examples of Sitecore Helix.", "Habitat is not supported by Sitecore and should be used at your own risk.");

gulp.task("default", function (callback) {
  config.runCleanBuilds = true;
  return runSequence(
    "01-Copy-Sitecore-License",
    "02-Nuget-Restore",
    "Generate-Bundle-Css-Js",
    "Generate-Minify-Js-Feature",
    "03-Publish-All-Projects",
    "04-Apply-Xml-Transform",
    "05-Sync-Unicorn",
    "06-Deploy-Transforms",
	callback);
});

gulp.task("deploy", function (callback) {
  config.runCleanBuilds = true;
  return runSequence(
    "01-Copy-Sitecore-License",
    "02-Nuget-Restore",
    "Generate-Bundle-Css-Js",
    "Generate-Minify-Js-Feature",
    "03-Publish-All-Projects",
    "04-Apply-Xml-Transform",
    "06-Deploy-Transforms",
	callback);
});

/*****************************
  Initial setup
*****************************/
gulp.task("01-Copy-Sitecore-License", function () {
  console.log("Copying Sitecore License file");

  return gulp.src(config.licensePath).pipe(gulp.dest("./lib"));
});

gulp.task("02-Nuget-Restore", function (callback) {
  var solution = "./" + config.solutionName + ".sln";
  return gulp.src(solution).pipe(nugetRestore());
});


gulp.task("03-Publish-All-Projects", function (callback) {
  return runSequence(
    "Build-Solution",
    "Publish-Foundation-Projects",
    "Publish-Feature-Projects",
    "Publish-Project-Projects", callback);
});

gulp.task("04-Apply-Xml-Transform", function () {
  var layerPathFilters = ["./src/Foundation/**/*.transform", "./src/Feature/**/*.transform", "./src/Project/**/*.transform", "!./src/**/obj/**/*.transform", "!./src/**/bin/**/*.transform"];
  return gulp.src(layerPathFilters)
      .pipe(foreach(function (stream, file) {
       var fileToTransform = file.path.replace(/.+code\\(.+)\.transform/, "$1");
      util.log("Applying configuration transform: " + file.path);
      return gulp.src("./scripts/applytransform.targets")
        .pipe(msbuild({
          targets: ["ApplyTransform"],
          configuration: config.buildConfiguration,
          logCommand: false,
          verbosity: config.buildVerbosity,
          stdout: true,
          errorOnFail: true,
          maxcpucount: config.buildMaxCpuCount,
          nodeReuse: false,
          toolsVersion: config.buildToolsVersion,
          properties: {
            Platform: config.buildPlatform,
            WebConfigToTransform: config.websiteRoot,
            TransformFile: file.path,
            FileToTransform: fileToTransform
          }
        }));
    }));
});

gulp.task("05-Sync-Unicorn", function (callback) {
  var options = {};
  options.siteHostName = habitat.getSiteUrl();
  options.authenticationConfigFile = config.websiteRoot + "/App_config/Include/Unicorn.SharedSecret.config";

  unicorn(function() { return callback() }, options);
});


gulp.task("06-Deploy-Transforms", function () {
  return gulp.src("./src/**/code/**/*.transform")
      .pipe(gulp.dest(config.websiteRoot + "/temp/transforms"));
});

gulp.task("Generate-Bundle-Css-Js", ["min:js", "min:css"]);

gulp.task("Generate-Minify-Js-Feature", function () {
    var root = "./src";
    var roots = root + "/Feature/**/**/js";
    var files = "/**/*.js";
    return gulp.src(roots, { base: root }).pipe(
        foreach(function (stream, file) {
            var tasks = gulp.src(file.path + files, { base: file.path })
                .pipe(foreach(function (jsStream, jsFile) {
                    var parts = jsFile.path.split("\\");
                    var fileName = parts[parts.length - 1];
                    var destination = jsFile.path.replace("\\" + fileName, "");
                    if (!jsFile.path.endsWith("min.js")) {
                        return gulp.src(jsFile.path)
                            .pipe(concat(fileName.replace(".js", ".min.js")))
                            .pipe(uglify())
                            .on('error', function (err) { util.log(util.colors.red('[Error]'), err.toString()); })
                            .pipe(gulp.dest(destination));
                    }
                    return jsStream;
                }));
            return merge(tasks);
        })
    );
});

/*****************************
  Copy assemblies to all local projects
*****************************/
gulp.task("Copy-Local-Assemblies", function () {
  console.log("Copying site assemblies to all local projects");
  var files = config.sitecoreLibraries + "/**/*";

  var root = "./src";
  var projects = root + "/**/code/bin";
  return gulp.src(projects, { base: root })
    .pipe(foreach(function (stream, file) {
      console.log("copying to " + file.path);
      gulp.src(files)
        .pipe(gulp.dest(file.path));
      return stream;
    }));
});

/*****************************
  Publish
*****************************/
var publishStream = function (stream, dest) {
  var targets = ["Build"];

  return stream
    .pipe(debug({ title: "Building project:" }))
    .pipe(msbuild({
      targets: targets,
      configuration: config.buildConfiguration,
      logCommand: false,
      verbosity: config.buildVerbosity,
      stdout: true,
      errorOnFail: true,
      maxcpucount: config.buildMaxCpuCount,
      nodeReuse: false,
      toolsVersion: config.buildToolsVersion,
      properties: {
        Platform: config.publishPlatform,
        DeployOnBuild: "true",
        DeployDefaultTarget: "WebPublish",
        WebPublishMethod: "FileSystem",
        DeleteExistingFiles: "false",
        publishUrl: dest,
        _FindDependencies: "false"
      }
    }));
}

var publishProject = function (location, dest) {
  dest = dest || config.websiteRoot;

  console.log("publish to " + dest + " folder");
  return gulp.src(["./src/" + location + "/code/*.csproj"])
    .pipe(foreach(function (stream, file) {
      return publishStream(stream, dest);
    }));
}

var publishProjects = function (location, dest) {
  dest = dest || config.websiteRoot;

  console.log("publish to " + dest + " folder");
  return gulp.src([location + "/**/code/*.csproj"])
    .pipe(foreach(function (stream, file) {
      return publishStream(stream, dest);
    }));
};

gulp.task("Build-Solution", function () {
  var targets = ["Build"];
  if (config.runCleanBuilds) {
    targets = ["Clean", "Build"];
  }

  var solution = "./" + config.solutionName + ".sln";
  return gulp.src(solution)
      .pipe(msbuild({
          targets: targets,
          configuration: config.buildConfiguration,
          logCommand: false,
          verbosity: config.buildVerbosity,
          stdout: true,
          errorOnFail: true,
          maxcpucount: config.buildMaxCpuCount,
          nodeReuse: false,
          toolsVersion: config.buildToolsVersion,
          properties: {
            Platform: config.buildPlatform
          }
        }));
});

gulp.task("Publish-Foundation-Projects", function () {
  return publishProjects("./src/Foundation");
});

gulp.task("Publish-Feature-Projects", function () {
  return publishProjects("./src/Feature");
});

gulp.task("Publish-Project-Projects", function () {
  return publishProjects("./src/Project");
});

gulp.task("Publish-Project", function () {
  if(yargs && yargs.m && typeof(yargs.m) == 'string') {
    return publishProject(yargs.m);
  } else {
    throw "\n\n------\n USAGE: -m Layer/Module \n------\n\n";
  }
});

gulp.task("Publish-Assemblies", function () {
  var root = "./src";
  var binFiles = root + "/**/code/**/bin/Sitecore.{Feature,Foundation,Habitat}.*.{dll,pdb}";
  var destination = config.websiteRoot + "/bin/";
  return gulp.src(binFiles, { base: root })
    .pipe(rename({ dirname: "" }))
    .pipe(newer(destination))
    .pipe(debug({ title: "Copying " }))
    .pipe(gulp.dest(destination));
});

gulp.task("Publish-All-Views", function () {
  var root = "./src";
  var roots = [root + "/**/Views", "!" + root + "/**/obj/**/Views"];
  var files = "/**/*.cshtml";
  var destination = config.websiteRoot + "\\Views";
  return gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, file) {
      console.log("Publishing from " + file.path);
      gulp.src(file.path + files, { base: file.path })
        .pipe(newer(destination))
        .pipe(debug({ title: "Copying " }))
        .pipe(gulp.dest(destination));
      return stream;
    })
  );
});

gulp.task("Publish-All-Configs", function () {
  var root = "./src";
  var roots = [root + "/**/App_Config", "!" + root + "/**/obj/**/App_Config"];
  var files = "/**/*.config";
  var destination = config.websiteRoot + "\\App_Config";
  return gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, file) {
      console.log("Publishing from " + file.path);
      gulp.src(file.path + files, { base: file.path })
        .pipe(newer(destination))
        .pipe(debug({ title: "Copying " }))
        .pipe(gulp.dest(destination));
      return stream;
    })
  );
});

/*****************************
 Watchers
*****************************/
gulp.task("Auto-Publish-Css", function () {
  var root = "./src";
  var roots = [root + "/**/styles", "!" + root + "/**/obj/**/styles"];
  var files = "/**/*.css";
  var destination = config.websiteRoot + "\\styles";
  gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, rootFolder) {
      gulp.watch(rootFolder.path + files, function (event) {
        if (event.type === "changed") {
          console.log("publish this file " + event.path);
          gulp.src(event.path, { base: rootFolder.path }).pipe(gulp.dest(destination));
        }
        console.log("published " + event.path);
      });
      return stream;
    })
  );
});

gulp.task("Auto-Publish-Views", function () {
  var root = "./src";
  var roots = [root + "/**/Views", "!" + root + "/**/obj/**/Views"];
  var files = "/**/*.cshtml";
  var destination = config.websiteRoot + "\\Views";
  return gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, rootFolder) {
      gulp.watch(rootFolder.path + files, function (event) {
        if (event.type === "changed") {
          console.log("publish this file " + event.path);
          gulp.src(event.path, { base: rootFolder.path }).pipe(gulp.dest(destination));
        }
        console.log("published " + event.path);
      });
      return stream;
    })
  );
});

gulp.task("Auto-Publish-Assemblies", function () {
  var root = "./src";
  var roots = [root + "/**/code/**/bin"];
  var files = "/**/Sitecore.{Feature,Foundation,Habitat}.*.{dll,pdb}";;
  var destination = config.websiteRoot + "/bin/";
  gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, rootFolder) {
      gulp.watch(rootFolder.path + files, function (event) {
        if (event.type === "changed") {
          console.log("publish this file " + event.path);
          gulp.src(event.path, { base: rootFolder.path }).pipe(gulp.dest(destination));
        }
        console.log("published " + event.path);
      });
      return stream;
    })
  );
});

gulp.task("Remove-Minify-Js-Feature", function () {
    var root = "./src";
    var roots = root + "/Feature/**/**/js";
    var files = "/**/*.js";
    return gulp.src(roots, { base: root }).pipe(
        foreach(function (stream, file) {
            gulp.src(file.path + files, { base: file.path })
                .pipe(foreach(function (jsStream, jsFile) {
                    if (jsFile.path.endsWith("min.js")) {
                        del(jsFile.path, { force: true });
                    }
                    return jsStream;
                }));
            return stream;
        })
    );
});

gulp.task("min:js", function () {
    var destination = "./src/Project/Common/code/js";
    var tasks = getBundles(".js").map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(concat(bundle.outputFileName))
            .pipe(uglify())
            .on('error', function (err) { util.log(util.colors.red('[Error]'), err.toString()); })
            .pipe(gulp.dest(destination));
    });
    return merge(tasks);
});

gulp.task("min:css", function () {
    var destination = "./src/Project/Common/code/styles";
    var tasks = getBundles(".css").map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(concat(bundle.outputFileName))
            .pipe(cssmin())
            .pipe(gulp.dest(destination));
    });
    return merge(tasks);
});

function getBundles(extension) {
    return bundleconfig.filter(function (bundle) {
        return new RegExp(`${extension}$`).test(bundle.outputFileName);
    });
}
/*****************************
 Package
*****************************/
var websiteRootBackup = config.websiteRoot;
var path = require("path");
var rimrafDir = require("rimraf");
var rimraf = require("gulp-rimraf");
var xmlpoke = require("xmlpoke");

/* publish files to temp location */
gulp.task("Package-Publish", function (callback) {
    config.websiteRoot = path.resolve("./temp");
    config.buildConfiguration = "Release";
    fs.mkdirSync(config.websiteRoot);
    runSequence(
      "Build-Solution",
      "Publish-Foundation-Projects",
      "Publish-Feature-Projects",
      "Publish-Project-Projects", callback);
});

/* Remove unwanted files */
gulp.task("Package-Prepare-Package-Files", function (callback) {
    var excludeList = [
      config.websiteRoot + "\\bin\\{Sitecore,Lucene,Newtonsoft,System,Microsoft.Web.Infrastructure}*dll",
      config.websiteRoot + "\\compilerconfig.json.defaults",
      config.websiteRoot + "\\packages.config",
      config.websiteRoot + "\\App_Config\\Include\\{Feature,Foundation,Project}\\*Serialization.config",
      config.websiteRoot + "\\App_Config\\Include\\{Feature,Foundation,Project}\\z.*DevSettings.config",
      "!" + config.websiteRoot + "\\bin\\Sitecore.Support*dll",
      "!" + config.websiteRoot + "\\bin\\Sitecore.{Feature,Foundation,FrasersProperty,Demo,Common}*dll"
    ];
    gulp.src(config.websiteRoot +"\\App_Config\\Include\\Project\\z.FrasersProperty.config.disabled")
        .pipe(rename(config.websiteRoot +"\\App_Config\\Include\\Project\\z.FrasersProperty.config"))
        .pipe(gulp.dest('.'));
    console.log(excludeList);
    return gulp.src(excludeList, { read: false }).pipe(rimraf({ force: true }));
});

/* Add files to package definition */
gulp.task("Package-Enumerate-Files", function () {
    var packageFiles = [];
    config.websiteRoot = websiteRootBackup;

    return gulp.src(path.resolve("./temp") + "/**/*.*", { base: "temp", read: false })
      .pipe(foreach(function (stream, file) {
          var item = "/" + file.relative.replace(/\\/g, "/");
          console.log("Added to the package:" + item);
          packageFiles.push(item);
          return stream;
      })).pipe(util.buffer(function () {
          xmlpoke("./package.xml", function (xml) {
              for (var idx in packageFiles) {
                  xml.add("project/Sources/xfiles/Entries/x-item", packageFiles[idx]);
              }
          });
      }));
});

/* Add items to package definition */
gulp.task("Package-Enumerate-Items", function () {
    var itemPaths = [];
    var allowedPatterns = [
      "./src/**/serialization/**/*.yml",
      "!./src/**/serialization/Roles/**/*.yml",
      "!./src/**/serialization/Users/**/*.yml"
    ];
    return gulp.src(allowedPatterns)
        .pipe(foreach(function (stream, file) {
        console.log(file);
            var itemPath = unicorn.getFullItemPath(file);
            itemPaths.push(itemPath);
            return stream;
        })).pipe(util.buffer(function () {
            xmlpoke("./package.xml", function (xml) {
                for (var idx in itemPaths) {
                    xml.add("project/Sources/xitems/Entries/x-item", itemPaths[idx]);
                }
            });
        }));
});

/* Add users to package definition */
gulp.task("Package-Enumerate-Users", function () {
    var users = [];

    return gulp.src("./src/**/serialization/Users/**/*.yml")
        .pipe(foreach(function (stream, file) {
          console.log(file);
            var fileContent = file.contents.toString();
            var userName = unicorn.getUserPath(file);
            users.push(userName);
            return stream;
        })).pipe(util.buffer(function () {
            xmlpoke("./package.xml", function (xml) {
                for (var idx in users) {
                    xml.add("project/Sources/accounts/Entries/x-item", users[idx]);
                }
            });
        }));
});

/* Add roles to package definition */
gulp.task("Package-Enumerate-Roles", function () {
    var roles = [];

    return gulp.src("./src/**/serialization/Roles/**/*.yml")
        .pipe(foreach(function (stream, file) {
          console.log(file);
            var fileContent = file.contents.toString();
            var roleName = unicorn.getRolePath(file);            
            roles.push(roleName);
            return stream;
        })).pipe(util.buffer(function () {
            xmlpoke("./package.xml", function (xml) {
                for (var idx in roles) {
                    xml.add("project/Sources/accounts/Entries/x-item", roles[idx]);
                }
            });
        }));
});

/* Remove temp files */
gulp.task("Package-Clean", function (callback) {
    rimrafDir.sync(path.resolve("./temp"));
    callback();
});

/* Main task, generate package.xml */
gulp.task("Package-Generate", function (callback) {
    runSequence(
        "Package-Clean",
        "Package-Publish",
        "Package-Prepare-Package-Files",
        "Package-Enumerate-Files",
        "Package-Enumerate-Items",
        "Package-Enumerate-Users",
        "Package-Enumerate-Roles",
        "Package-Clean",
        callback);
});


/*****************************
 Package CI build for CM server
*****************************/


gulp.task("CI-Package-Clean", function (callback) {
    rimrafDir.sync(path.resolve("./artifact/Website"));
    rimrafDir.sync(path.resolve("./artifact/Data"));
    rimrafDir.sync(path.resolve("./artifact"));
    callback();
});

/* publish files to temp location */
gulp.task("CI-Package-Publish", function (callback) {
    config.websiteRoot = path.resolve("./artifact/Website");
    config.buildConfiguration = "Release";
    fs.mkdirSync("./artifact");
    fs.mkdirSync(config.websiteRoot);
    runSequence(
        "Build-Solution",
        "Publish-Foundation-Projects",
        "Publish-Feature-Projects",
        "Publish-Project-Projects", callback);
});

/* Prepare .yml files*/
gulp.task("CI-Copy-Items-For-Unicorn", function () {
    return gulp.src("./src/**/serialization/**/*.yml")
        .pipe(gulp.dest("./artifact/Data" + "/Unicorn/"));
});

/* Prepare .config Unicorn files*/
gulp.task("CI-Copy-Config-Files-For-Unicorn", function () {
    return gulp.src("./src/**/code/App_config/Include/**/*.Serialization.config")
        .pipe(flatten())
        .pipe(gulp.dest("./artifact/Website/App_Config/Include" + "/FrasersUnicorn/"));
});


gulp.task("CI-Package-Generate", function (callback) {
    runSequence(
        "CI-Package-Clean",
        "CI-Package-Publish",
        "Package-Prepare-Package-Files",
        "CI-Copy-Items-For-Unicorn",
        "CI-Copy-Config-Files-For-Unicorn",
        callback);
});


gulp.task("Setup-Solr-Config", function (callback) {
    exec("Powershell.exe -executionpolicy remotesigned . .\\scripts\\solr\\setup-solr.ps1; Set-SCSearchProvider -rootPath '" + config.websiteRoot + "'",
        function (err, stdout, stderr) {
            console.log(stdout);
            callback(err);
        });
});

gulp.task("Sitecore-Solr-Cores-Creation", function (callback) {
    var psArguments = " -solrPath " + config.solrPath;
    psArguments += " -configPath " + "..\\scripts\\solr\\sitecore-collection-conf-solr-5";
    psArguments += " -configName " + "sitecoreconf";
    psArguments += " -defaultSitecoreCollectionIndexNames " + config.defaultSitecoreCollectionIndexNames;
    psArguments += " -extraCollectionNames " + config.extraCollectionIndexNames;
    console.log("Running Sitecore-solr-cores-creation with parameters: " + psArguments);
    exec("Powershell.exe -executionpolicy Bypass . .\\scripts\\solr\\sitecore-solr-cores-creation.ps1 " + psArguments ,
        function (err, stdout, stderr) {
            console.log(stdout);
            callback(err);
        });
});

gulp.task("Sitecore-Solr-Cores-Delete", function (callback) {
    var psArguments = " -solrPath " + config.solrPath;
    psArguments += " -configPath " + "..\\scripts\\solr\\sitecore-collection-conf-solr-5";
    psArguments += " -configName " + "sitecoreconf";
    psArguments += " -deleteCollectionNames " + config.extraCollectionIndexNames;
    console.log("Running Sitecore-solr-cores-creation with parameters: " + psArguments);
    exec("Powershell.exe -executionpolicy Bypass . .\\scripts\\solr\\sitecore-solr-cores-creation.ps1 " + psArguments,
        function (err, stdout, stderr) {
            console.log(stdout);
            callback(err);
        });
});