$scriptAbsolutePath = Split-Path -Parent $MyInvocation.MyCommand.Definition
$sqlAbsolutePath = $scriptAbsolutePath+"/db.sql"
$envAbsolutePath = $scriptAbsolutePath+"/.env"

$envHash = @{}
$envHash.Add('test', 'outtest')
docker cp $sqlAbsolutePath db:/

Get-Content $envAbsolutePath | foreach {
    $variable, $value = $_.split('=')
    if ([string]::IsNullOrWhiteSpace($variable) -or $variable.Contains('#')) 
    {
        #skips empty lines or comments
        return
    }
    $envHash["$variable"] = "$value"
}
Write-Host $envHash['MYSQL_DATABASE']
Write-Host $envHash['MYSQL_ROOT_PASSWORD']

docker exec db sh -c "mariadb $($envHash['MYSQL_DATABASE']) -u root -p$($envHash['MYSQL_ROOT_PASSWORD']) <db.sql"
