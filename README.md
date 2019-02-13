# Frasers Property project

-------------------------------------------------------------------------

Getting Started

To install:
    1. Update the below files for your local environment
        * /src/Project/Habitat/code/App_Config/Include/Project/z.Frasers.DevSettings.config
        * /gulp-config.js
        * /publishsettings.targets
    2. Set up a sitecore instance
        * Sitecore Experience Platform 8.2 Update 3
        * Webforms for Marketers 8.2 Update 3 (Optional)
    3. Restore Node.js modules: In an elevated privileges command prompt (started with Run as administrator), run 'npm install' in the root of repository.
    4. Build and publish the solution.
        * Open Visual Studio 2017 in administrator mode.
        * Open the Visual Studio 2017 Task Runner Explorer pane (View | Other Windows | Task Runner Explorer)
        * Run the "default" task

Switch Lucene to Solr  -> solr 5.1.0 may caused an issue related to:  https://issues.apache.org/jira/browse/LUCENE-7188 
    1.  Download unzip Solr 6.6.3
    2.  Host Solr 6.6.3 as a window service.
    3.  Change Solr folder path in the gulp-config.js file.
    4.  Run Gulp Task "Sitecore-Solr-Cores-Creation" to generate the solr cores.
    4.  Switch config to solr
        * Open the Visual Studio 2017 Task Runner Explorer pane (View | Other Windows | Task Runner Explorer)
        * Run the "Setup-Solr-Config" --> this task will automatically switch configs of Lucene to Solr.
        * Go to "Website\App_config\Include" disable config "Sitecore.ContentSearch.SolrCloud.SwitchOnRebuild.config.disabled".
    5.  Restart Solr Service.
    6.  Restart IIS website and Rebuild all indexes.
    (Finished)

