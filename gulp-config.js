module.exports = function() {
    var instanceRoot = "C:\\inetpub\\wwwroot\\FrasersProperty";
    var config = {
        websiteRoot: instanceRoot + "\\Website",
        sitecoreLibraries: instanceRoot + "\\Website\\bin",
        licensePath: instanceRoot + "\\Data\\license.xml",
        solutionName: "FrasersProperty",
        buildConfiguration: "Debug",
        buildToolsVersion: 15.0,
        buildMaxCpuCount: 0,
        buildVerbosity: "minimal",
        buildPlatform: "Any CPU",
        publishPlatform: "AnyCpu",
        runCleanBuilds: false,
        solrPath: "C:\\solr-5.1.0\\",
        defaultSitecoreCollectionIndexNames:
            "sitecore_analytics_index" +
                ",sitecore_core_index" +
                ",sitecore_core_indexMainAlias" +
                ",sitecore_core_indexRebuildAlias" +
                ",sitecore_fxm_master_index" +
                ",sitecore_fxm_web_index" +
                ",sitecore_list_index" +
                ",sitecore_marketing_asset_index_master" +
                ",sitecore_marketing_asset_index_web" +
                ",sitecore_marketingdefinitions_core" +
                ",sitecore_marketingdefinitions_master" +
                ",sitecore_marketingdefinitions_web" +
                ",sitecore_master_index" +
                ",sitecore_master_indexMainAlias" +
                ",sitecore_master_indexRebuildAlias" +
                ",sitecore_suggested_test_index" +
                ",sitecore_testing_index" +
                ",sitecore_web_index" +
                ",sitecore_web_indexMainAlias" +
                ",sitecore_web_indexRebuildAlias" +
                ",social_messages_master" +
                ",social_messages_web",
        extraCollectionIndexNames:
            "sitecore_anchorpoint_master_index" +
                ",sitecore_anchorpoint_web_index" +
                ",sitecore_anchorpoint_web_index_rebuild" +
                ",sitecore_frasersrewards_master_index" +
                ",sitecore_frasersrewards_web_index" +
                ",sitecore_frasersrewards_web_index_rebuild" +
                ",sitecore_fraserstower_master_index" +
                ",sitecore_fraserstower_web_index" +
                ",sitecore_fraserstower_web_index_rebuild" +
                ",sitecore_waterwaypoint_master_index" +
                ",sitecore_waterwaypoint_web_index" +
                ",sitecore_waterwaypoint_web_index_rebuild"
    };
    return config;
};
