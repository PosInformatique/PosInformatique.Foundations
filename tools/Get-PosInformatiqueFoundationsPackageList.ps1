[CmdletBinding()]
param (
)

$sourceDirectory = Join-Path $PSScriptRoot "..\src"

Get-ChildItem -LiteralPath $sourceDirectory -Directory -ErrorAction Stop |
    Sort-Object -Property Name |
    ForEach-Object { "PosInformatique.Foundations.$($_.Name)" }