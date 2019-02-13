Function Set-SCSearchProvider	{
	Param(
		[Parameter(Mandatory=$True)]
		[string]$rootPath
   )

    $validInput = $true;
    #test that path is valid
    If (!(Test-Path -Path $rootPath))
    {
        Write-Host "The supplied path was invalid or inaccessible." -ForegroundColor Red;
        $validInput = $false;
    }
    If ($validInput)
    {
        Write-Host "Set to Solr." -ForegroundColor Yellow;
        $selectedProvider = "Solr";
        $deselectedProvider = "Lucene";
        #enumerate all config files to be enabled
        $filter = "*" + $selectedProvider + "*.config*";
        $filesToEnable = Get-ChildItem -Recurse -File -Path $rootPath -Filter $filter;
        foreach ($file in $filesToEnable)
        {
            Write-Host $file.Name;
            if (($file.Extension -ne ".config"))
            {
                $newFileName = [io.path]::GetFileNameWithoutExtension($file.FullName);
                $newFile = Rename-Item -Path $file.FullName -NewName $newFileName -PassThru;
                Write-Host "-> " $newFile.Name -ForegroundColor Green;
            }
        }
        #enumerate all config files to be disabled
        $filter = "*" + $deselectedProvider + "*.config*";
        $filesToDisable = Get-ChildItem -Recurse -File -Path $rootPath -Filter $filter;
        foreach ($file in $filesToDisable)
        {
            Write-Host $file.Name;
            if ($file.Extension -eq ".config")
            {
                $newFileName = $file.Name + ".disabled";
                $newFile = Rename-Item -Path $file.FullName -NewName $newFileName -PassThru;
                Write-Host "-> " $newFile.Name -ForegroundColor Green;
            }
        }
    }
}