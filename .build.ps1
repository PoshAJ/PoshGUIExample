param (
    [version] $ModuleVersion = '0.0.0'
)

[string] $Script:RootModule = 'Example'


task CleanModule {
    Remove-Module $RootModule -Force -ErrorAction 'Ignore'
}


task CleanFiles {
    Remove-Item -Path "./output/" -Recurse -Force -ErrorAction 'Ignore'
}


task CleanLibrary {
    assert (Test-Path -Path './source/library/library.csproj')

    exec { dotnet clean './source/library/library.csproj' }
}


task LoadModuleManifest {
    assert (Test-Path -Path "./source/${RootModule}.psd1")

    [hashtable] $Script:ModuleManifest = Import-PowerShellDataFile -Path "./source/${RootModule}.psd1"

    $Script:ModuleManifest += $ModuleManifest.PrivateData.PSData
    $Script:ModuleManifest.Remove('PrivateData')
}


# TODO ReleaseNotes


task BuildLibrary {
    assert (Test-Path -Path './source/library/library.csproj')

    exec { dotnet build './source/library/library.csproj' --output "./output/${RootModule}/${ModuleVersion}/" --property:Version="${ModuleVersion}" --property:Module="${RootModule}" --configuration 'Release' }

    [string[]] $Script:LibraryFunctions = 'Show-PublicFunction' # TODO Get List Dynamically
}


task BuildModule {
    [System.IO.FileInfo] $Script:ModuleFile = New-Item -ItemType 'File' -Path "./output/${RootModule}/${ModuleVersion}/${RootModule}.psm1" -Force

    [array] $Groups = Get-ChildItem -Path './source/*/*.ps1' | Group-Object { $_.Directory.BaseName }
    [string[]] $Script:PublicFunctions = $Groups | Where-Object -Property 'Name' -EQ 'Public' | & { Process { $_.Group.BaseName } }

    foreach ($Group in $Groups) {
        Add-Content -Path $ModuleFile -Value "#region: $( $Group.Name )"

        foreach ($File in $Group.Group) {
            Add-Content -Path $ModuleFile -Value "#region: $( Resolve-Path -Path $File -Relative )"
            Add-Content -Path $ModuleFile -Value (Get-Content -Path $File)
            Add-Content -Path $ModuleFile -Value '#endregion'
        }

        Add-Content -Path $ModuleFile -Value '#endregion'
    }
}


# TODO Help


task BuildModuleManifest {
    $Script:ModuleManifest.ModuleVersion = $ModuleVersion
    $Script:ModuleManifest.FunctionsToExport = $PublicFunctions
    $Script:ModuleManifest.CmdletsToExport = $LibraryFunctions
    $Script:ModuleManifest.AliasesToExport = [string[]] (Import-Module $ModuleFile -PassThru).ExportedAliases.Keys

    New-ModuleManifest @ModuleManifest -Path "./output/${RootModule}/${ModuleVersion}/${RootModule}.psd1"
}


task LoadModule {
    assert (Test-Path -Path "./output/${RootModule}/${ModuleVersion}/${RootModule}.psd1")

    Import-Module "./output/${RootModule}/${ModuleVersion}/${RootModule}.psd1" -Force
}


# TODO Analyze and Test


task Clean @(
    'CleanModule'
    'CleanFiles'
    'CleanLibrary'
)

task Build @(
    'LoadModuleManifest'
    'BuildLibrary'
    'BuildModule'
    'BuildModuleManifest'
)

task . {}
