#-------------------------------------------------------------------------------
# Displays how to use this script.
#-------------------------------------------------------------------------------
function Help {
    "Sets the AssemblyVersion and AssemblyFileVersion of AssemblyInfo.cs files`n"
    ".\SetVersion.ps1 [VersionNumber]`n"
    "   [VersionNumber]     The version number to set, for example: 1.1.9301.0"
    "                       If not provided, a version number will be generated.`n"
}

#-------------------------------------------------------------------------------
# Get a version number.
# Note: customize this function to generate it using your version schema.
#-------------------------------------------------------------------------------
function Get-VersionNumber {
    
    $versionPattern = '^[^/]{2}.*\("([0-9]+(\.([0-9]+|\*)){1,3})"\)'
    $path = ([string] (Resolve-Path ..\)) + "\CSharpBrew.*\**\AssemblyInfo.cs";

	Get-ChildItem $path -r | Select-Object -first 1 | ForEach-Object {
			$match = select-string -path $_.FullName -pattern $versionPattern
            $fullVersion = [string]$match[0].Matches[0].Groups[1].Value;
            $start = $fullVersion.LastIndexOf(".") + 1;
            $length = $fullVersion.Length - $start;
            $version =  1;
            [int32]::TryParse($fullVersion.Substring($start,$length),[ref] $version) | Out-Null;
            $version++;
            return ($fullVersion.Substring(0,$start) + $version);
		}
}
  
#-------------------------------------------------------------------------------
# Update version numbers of AssemblyInfo.cs
#-------------------------------------------------------------------------------
function Update-AssemblyInfoFiles ([string] $version) {
    $assemblyVersionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $fileVersionPattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $assemblyVersion = 'AssemblyVersion("' + $version + '")';
    $fileVersion = 'AssemblyFileVersion("' + $version + '")';

    $path = ([string] (Resolve-Path ..\)) + "\CSharpBrew.*\**\AssemblyInfo.cs";

	Get-ChildItem $path -r | ForEach-Object {
        $filename = $_.Directory.ToString() + '\' + $_.Name
        $filename + ' -> ' + $version
        
        # If you are using a source control that requires to check-out files before 
        # modifying them, make sure to check-out the file here.
        # For example, TFS will require the following command:
        # tf checkout $filename
    
        (Get-Content $filename) | ForEach-Object {
            % {$_ -replace $assemblyVersionPattern, $assemblyVersion } |
            % {$_ -replace $fileVersionPattern, $fileVersion }
        } | Set-Content $filename
    }
}

#-------------------------------------------------------------------------------
# Update version numbers of package.nuspec
#-------------------------------------------------------------------------------
function Update-NuspecFiles ([string] $version) {
    $nuspecPattern = '\>[0-9]+(\.([0-9]+|\*)){1,3}\<'
	$nuspecVersion = '>' + $version + '<';
    
    $path = ([string](Resolve-Path ..\)) + "\CSharpBrew.*\**\*.nuspec"
    
    Get-ChildItem $path -r | ForEach-Object {

        $filename = $_.Directory.ToString() + '\' + $_.Name
        $filename + ' -> ' + $version
        
        # If you are using a source control that requires to check-out files before 
        # modifying them, make sure to check-out the file here.
        # For example, TFS will require the following command:
        # tf checkout $filename
    
        (Get-Content $filename) | ForEach-Object {
            % {$_ -replace $nuspecPattern, $nuspecVersion } 
        } | Set-Content $filename
    }
}


#-------------------------------------------------------------------------------
# Parse arguments.
#-------------------------------------------------------------------------------
if ($args -ne $null) {
    $version = $args[0]
    if (($version -eq '/?') -or ($version -notmatch "[0-9]+(\.([0-9]+|\*)){1,3}")) {
        Help
        return;
    }
} else {
    $version =  Get-VersionNumber
}

Update-AssemblyInfoFiles $version
Update-NuspecFiles $version