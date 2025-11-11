param(
    [Switch]$h, #helper
    [Switch]$c, #clear
    [Switch]$i, #inject
    [Switch]$r #rebuld
)

if($h)
{
    Write-Host "WARNING: script will not handle any errors when executing script inside container"
    Write-Host "Generates a sql script to insert new users and obstacles"
    Write-Host "    -h: helper. Displays what you're reading rn"
    Write-Host "    -i: inject. Injects and executes sql in container"
    Write-Host "        -c: clear. Deletes generated sql file from root folder. must run -i for this option"
    return
}

$scriptAbsolutePath = Split-Path -Parent $MyInvocation.MyCommand.Definition
$sciptoutputname = "mockdataoutput.sql"
$scriptoutputpath = ($scriptAbsolutePath + "/" + $sciptoutputname)
$envAbsolutePath = $scriptAbsolutePath+"/.env"

#stores env file as hashtable, for ease of access
$envHash = @{}

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

New-Item -Path $scriptAbsolutePath -Name $sciptoutputname -ItemType "File" -Force

$rowNum = Read-Host "Input number of rows to generate"

$Users = [System.Collections.Generic.List[string]]::new() #store user id

for ($index = 0; $index -le $rowNum; $index++)
{
    $Users.Add("$index")
}

for ($index = 1; $index -le $rowNum; $index++) #creates most basic user possible 
{
    Add-Content -Path ($scriptoutputpath) -Value (
    "INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash,
                            SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled,
                            LockoutEnd, LockoutEnabled, AccessFailedCount)
    VALUES ('$index', null, null, null, null, true, null, null, null,
            null, false, false, null, false, 0);")
}

for ($index = 1; $index -le $rowNum; $index++) #creates most basic user possible 
{
    #obstacle table fields
    $userId = $Users[(Get-Random -Maximum $Users.Count)] #TODO: add later when users are assigned to obstacle
    $obstacleId = $index
    $heightMeter = (Get-Random -Maximum 150)
    $geometryGeoJson = (Get-Random -Maximum 5000)
    $name = (Get-Random -Maximum 5000)
    $description = (Get-Random -Maximum 5000)
    $illuminated = (Get-Random -Maximum 3) #illuminated table has 3 options from 0-2
    $type = (Get-Random -Maximum 6) #type  table has 6 options from 0-5
    $status = (Get-Random -Maximum 5) #status table has 4 options from 0-4
    $marking = (Get-Random -Maximum 3) #marking table has 2 options from 0-2
    
    Add-Content -Path ($scriptoutputpath) -Value (
    "INSERT INTO Obstacle (ObstacleID, Heightmeter, GeometryGeoJson, Name, Description, Illuminated, Type, Status, Marking)
    VALUES ($obstacleId, $heightMeter, '$geometryGeoJson', '$name', '$description', $illuminated, $type, $status, $marking);")
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
        Write-Host "    Database cleared"

        #runs sql script on container
        Write-Host "    Executing sql script on $($envHash['MYSQL_DATABASE'])..."
        docker exec db sh -c "mariadb $($envHash['MYSQL_DATABASE']) -u root -p$($envHash['MYSQL_ROOT_PASSWORD']) <db.sql"
        Write-Host "        Sql script executed, tables built"
    }
    catch
    {
        return
    }
}

if ($i) #inject into container
{
    docker cp $scriptoutputpath db:/
    docker exec db sh -c "mariadb $($envHash['MYSQL_DATABASE']) -u root -p$($envHash['MYSQL_ROOT_PASSWORD']) <$sciptoutputname" 
    
    if($c) #delete script from local machine
    {
        Remove-Item -Path $scriptoutputpath
    }
}


