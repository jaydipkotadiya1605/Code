<#
.SYNOPSIS
    Creates the Sitecore solr cores/collections (solr standalone / solr cloud)
.DESCRIPTION
    Script to create all the the needed Sitecore cores/collections in Solr  
 
.PARAMETER solrPath
    Path to the solr installation
.PARAMETER configName
    Name of the core/collection configuration name. You can create this configuration by using the sitecore-config-creator.ps1 script. 
.PARAMETER shards
    Number of shards for each collection
.PARAMETER replicationFactor
    Replication factor per collection 
.EXAMPLE
    C:\PS> sitecore-solr-cores-creation.ps1 -command create -solrPath C:\solr -configName sitecoreconf
#>


param(
    [Parameter(Mandatory=$true)]
    [string]$solrPath,      # The path to the solr installation. We expect bin\solr.cmd to be there (for Sitecore 5+)
    [Parameter(Mandatory=$true)]
    [string]$configName,            # The configuration name to use for the cores/collections
	[array]$defaultSitecoreCollectionIndexNames=@(),
    [string]$configPath,
    [string]$shards="1",
    [string]$replicationFactor="3",
    [array]$extraCollectionNames=@(),
    [array]$deleteCollectionNames=@()
    )

$sitecore_collection_names = $defaultSitecoreCollectionIndexNames + $extraCollectionNames


$ErrorActionPreference = "stop"
$location = get-location
Import-Module .\scripts\solr\solr-powershell.psm1 -Force -ArgumentList @($solrPath) 


#Check-Collection-Exist -collectionName "sitecore_core_index"
Function Create-Sitecore-Collections {
    Write-Host "Creating Sitecore Collections" -ForegroundColor Cyan
    foreach ($collectionName in $sitecore_collection_names)
    {
	    
        if (Check-Collection-Exists -collectionName $collectionName )
        {
            Write-Host "Collection is existed $collectionName"
        }
		else
		{
			if (Create-Collection -collectionName $collectionName  -configName $configName -shards $shards -replicationFactor $replicationFactor)
			{
				Write-Host "Collection $collectionName created sucessfully" -ForegroundColor Cyan
				Copy-Item -Path ".\scripts\solr\sitecore-collection-conf-solr-5\schema.xml" -Destination "$solrPath/server/solr/$collectionName/conf"
				Copy-Item  -Path ".\scripts\solr\sitecore-collection-conf-solr-5\solrconfig.xml" -Destination "$solrPath/server/solr/$collectionName/conf" -Recurse -force
				Remove-Item "$solrPath/server/solr/$collectionName/conf/managed-schema"
			}
			else 
			{
				Write-Error "Error creating collection $collectionName"
				return $false;
			}
		}
    }
    Write-Host "Finished creating Sitecore collections" -ForegroundColor Green
}
Function Delete-Sitecore-Collections
{
	foreach( $collectionName in $deleteCollectionNames)
		{
			if(Check-Collection-Exists -collectionName $collectionName)
			{
				 Delete-Collection -collectionName $collectionName
				 Write-Host "Collection is deleted $collectionName"
				 
			}
		}
}
Create-Sitecore-Collections
Delete-Sitecore-Collections