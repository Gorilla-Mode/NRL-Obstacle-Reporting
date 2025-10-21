param(
    [Switch]$h, # helper
    [Switch]$ni, # no inject
    [Switch]$ne # no execute
)

if($h)
{
    Write-Host "    -h: helper. Displays what you're reading rn"
    Write-Host "    -ni: no inject. Prevents sql from being injected to container"
    Write-Host "    -ne: no execute. Prevents sql script in container from being executed"
    return
}

$scriptAbsolutePath = Split-Path -Parent $MyInvocation.MyCommand.Definition
$sqlAbsolutePath = $scriptAbsolutePath+"/db.sql"
$envAbsolutePath = $scriptAbsolutePath+"/.env"

#tests
$sqlFileExists = Test-Path -Path $sqlAbsolutePath
$envFileExists = Test-Path -Path $sqlAbsolutePath
if(!$sqlFileExists)
{
    Write-Host "Error: db.sql not found"
    return
}
if(!$envFileExists)
{
    Write-Host "Error: .env file not found, make sure to make env file first"
    return
}

#stores env file as hashtable, for ease of access
$envHash = @{}

if(!$ni)
{
    try
    {
        #injects sql script to container
        Write-Host "Injecting sql from @ $sqlAbsolutePath to container..."
        docker cp $sqlAbsolutePath db:/
    }
    catch
    {
        Write-Host "Sql injection failed"
        return
    }
}

#populates hashtable with variables and values from env
Get-Content $envAbsolutePath | foreach {
    $variable, $value = $_.split('=')
    if ([string]::IsNullOrWhiteSpace($variable) -or $variable.Contains('#')) 
    {
        #skips empty lines or comments
        return
    }
    $envHash["$variable"] = "$value"
}

if(!$ne)
{
    try
    {
        #runs sql script on container
        Write-Host "Executing sql script on $($envHash['MYSQL_DATABASE'])..."
        docker exec db sh -c "mariadb $($envHash['MYSQL_DATABASE']) -u root -p$($envHash['MYSQL_ROOT_PASSWORD']) <db.sql"
        Write-Host "Sql script executed"
    }
    catch
    {
        return
    } 
}