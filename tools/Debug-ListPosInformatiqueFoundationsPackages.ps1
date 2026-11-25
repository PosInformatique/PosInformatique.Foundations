$jsonResponse = Invoke-RestMethod -Uri "https://azuresearch-usnc.nuget.org/query?q=PosInformatique.Foundations&prerelease=true&semVerLevel=2.0.0&take=100" -Method Get

$packages = $jsonResponse.data | Sort-Object -Property title

foreach ($package in $packages)
{
    Write-Host $package.title

    foreach ($version in $package.versions)
    {
        Write-Host "  - $($version.version)"
    }
}