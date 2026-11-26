[CmdletBinding()]
param (
    [Parameter(Mandatory = $true)]
    [string[]]
    $PackageNames,

    [Parameter(Mandatory = $true)]
    [string[]]
    $Versions,

    [Parameter(Mandatory = $false)]
    [securestring]
    $ApiKey
)

Set-StrictMode -Version Latest

function Get-ApiKey([securestring]$apiKey)
{
    $ptr = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($apiKey)

    try
    {
        return [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($ptr)
    }
    finally
    {
        [System.Runtime.InteropServices.Marshal]::ZeroFreeBSTR($ptr)
    }
}

if (-Not($ApiKey))
{
    $ApiKey = Read-Host "Enter the NuGet API key" -AsSecureString
}

$rawApiKey = Get-ApiKey $ApiKey

try
{
    foreach ($packageName in $PackageNames)
    {
        $nuGetOrgPackage = Invoke-RestMethod -Uri "https://azuresearch-usnc.nuget.org/query?q=PackageId:$packageName&prerelease=true&semVerLevel=2.0.0" -Method Get

        if ($nuGetOrgPackage.data.Count -ne 1)
        {
            Write-Error "The package '$packageName' does not exists."
            continue
        }
            
        foreach ($version in $Versions)
        {
            $nuGetOrgVersions = @($nuGetOrgPackage.data[0].versions | Where-Object { $_.version -eq $version })

            if ($nuGetOrgVersions.Count -ne 1)
            {
                Write-Warning "The package '$packageName' version '$version' does not exists."
                continue
            }

            $headers = @{
                "X-NuGet-ApiKey" = $rawApiKey
            }

            Write-Host "Deleting package: $packageName version $Version" -ForegroundColor Cyan
            
            Invoke-RestMethod `
                -Headers $headers `
                -Uri "https://www.nuget.org/api/v2/package/$packageName/$Version" `
                -Method Delete
            
            Write-Host "Package successfully deleted." -ForegroundColor Green
        }
    }
}
catch
{
    Write-Error "Error occurred: $($_.Exception.Message)"
}