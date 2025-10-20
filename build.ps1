param(
    [Switch]$c, # runs a clean install
    [Switch]$f, # overwrites existing env file
    [Switch]$nc,
    [Switch]$h # help
)

if($h)
{
    Write-Host "    -c: clean. Cleans up previous install. Removes any containers, images and volumes defined in docker-compose"
    Write-Host "    -f: force. Forces creation of .env file, overwriting if one already exists"
    Write-Host "    -nc: no compose. Stops docker-compose up from running"
    Write-Host "    -h: helper. Displays what you're reading rn"
    
    return
}

# retiveves the path of the this script
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition

# --- env ----
try
{
    if($f)
    {
        # overwrites file if it exists
        New-Item -Path $scriptDir -Name ".env" -ItemType "File" -Force
    }
    else
    {
        # checks if file already exists
        if(-not(Test-Path -Path ($scriptDir + "./env")))
        {
            throw
        }
        
        New-Item -Path $scriptDir -Name ".env" -ItemType "File"
    }
}
catch
{
    Write-Output "ERROR: .env file aready exists in directory, use -f to overwrite"
    return
}


#Passowrd stored as secure string to hide input mostly
$secureDatabaseRootPwd = Read-Host "Create database password: " -AsSecureString 
$databaseName = Read-Host "Create database name: "
#returns password to plain text
$DatabaseRootPwd =[Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($secureDatabaseRootPwd))

try
{
    #populates env file
    Write-Host "    Populating .env @ $scriptDir"
    
    Add-Content -Path ($scriptDir + "/.env") -Value ("MYSQL_ROOT_PASSWORD=$databaseRootPwd")
    Add-Content -Path ($scriptDir +"/.env") -Value ("MYSQL_DATABASE=$databaseName")
    Add-Content -Path ($scriptDir +"/.env") -Value ("INTERNALCONNECTION=server=db;port=3306;database=$databaseName;user=root;password=$databaseRootPwd;")
    Add-Content -Path ($scriptDir +"/.env") -Value ("EXTERNALCONNECTION=server=localhost;port=3306;database=$databaseName;user=root;password=$databaseRootPwd;")
    
    Write-Host "    Populated successfully"
}
catch
{
    return
}

if ($nc)
{
    return
}

# ---- Docker ----

cd $scriptDir

if ($c)
{
    #removes any previous containers, images and volumes defined in docker-compose.yml
    #this is done so that new password applies to database.
    docker-compose down -v --rmi "all"
}

docker-compose up
