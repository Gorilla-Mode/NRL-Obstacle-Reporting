param(
    [Switch]$c # runs a clean install
)

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
New-Item -Path $scriptDir -Name ".env" -ItemType "File" -Force

$databaseRootPwd = Read-Host -Prompt "Enter db password"
$databaseName = Read-Host "Enter db name: "

Add-Content -Path ($scriptDir + "/.env") -Value ("MYSQL_ROOT_PASSWORD=$databaseRootPwd")
Add-Content -Path ($scriptDir +"/.env") -Value ("MYSQL_DATABASE=$databaseName")
Add-Content -Path ($scriptDir +"/.env") -Value ("INTERNALCONNECTION=server=db;port=3306;database=$databaseName;user=root;password=$databaseRootPwd;")
Add-Content -Path ($scriptDir +"/.env") -Value ("EXTERNALCONNECTION=server=localhost;port=3306;database=$databaseName;user=root;password=$databaseRootPwd;")

cd $scriptDir

if ($c)
{
    docker-compose down -v --rmi "all"
}

docker-compose up
