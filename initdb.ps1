param(
    [Switch]$h, # helper
    [Switch]$ni, # no inject
    [Switch]$ne, # no execute
    [Switch]$r #Rebuilds database
)

if($h)
{
    Write-Host "WARNING: script will not catch any errors inside docker container. Check tables manually for now"
    Write-Host "    -h: helper. Displays what you're reading rn"
    Write-Host "    -ni: no inject. Prevents sql from being injected to container"
    Write-Host "    -ne: no execute. Prevents sql script in container from being executed"
    Write-Host "    -r: rebuild. Drops database, and reacreates it"
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

if($r) #goofy script maybe redo
{
    try
    {
        Write-Host "Rebuilding Database"
        Write-Host "    Generating sql script..."
        $null = New-Item -Path $scriptAbsolutePath -Name "dropdb.sql" -Value "DROP DATABASE $($envHash['MYSQL_DATABASE']); CREATE DATABASE $($envHash['MYSQL_DATABASE']);" -Force
        $dropsqlAbsolutePath = $scriptAbsolutePath+"/dropdb.sql"

        Write-Host "    Injecting sql from @ $dropsqlAbsolutePath to container..."
        docker cp -q $dropsqlAbsolutePath db:/
        Write-Host "    Removing used script from @ $dropsqlAbsolutePath..."
        Remove-Item -Path $dropsqlAbsolutePath
        Write-Host "    Used script removed"

        Write-Host "    Executing sql script on $($envHash['MYSQL_DATABASE'])..."
        docker exec db sh -c "mariadb $($envHash['MYSQL_DATABASE']) -u root -p$($envHash['MYSQL_ROOT_PASSWORD']) <dropdb.sql"
        Write-Host "    Database Rebuilt"
    }
    catch
    {
        return
    }
}

if(!$ne)
{
    try
    {
        #runs sql script on container
        Write-Host "Executing sql script on $($envHash['MYSQL_DATABASE'])..."
        docker exec db sh -c "mariadb $($envHash['MYSQL_DATABASE']) -u root -p$($envHash['MYSQL_ROOT_PASSWORD']) <db.sql"
        Write-Host "    Sql script executed, tables built"
    }
    catch
    {
        return
    } 
}