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
    Write-Host "    -r: rebuild. Drops current database, rebuilds database and injects db.sql script"
    return
}

if(!$i -and $c)
{
    Write-Host "    ERROR: Illegal flag. Must run -i to run -c"
    return
}

if ($r)
{
    Write-Host "WARNING: Selected flag will drop database!"
    $conf = Read-Host "     Confirm [Y]"

    if ($conf -notlike "Y")
    {
        Write-Host "     Aborted"
        return
    }
}

#Important variables!
$scriptAbsolutePath = Split-Path -Parent $MyInvocation.MyCommand.Definition
$sciptoutputname = "mockdataoutput.sql"
$scriptoutputpath = ($scriptAbsolutePath + "/" + $sciptoutputname)
$envAbsolutePath = $scriptAbsolutePath+"/.env"
$sqlAbsolutePath = $scriptAbsolutePath+"/db.sql"

$Users = [System.Collections.Generic.List[string]]::new() #store user id
$envHash = @{} #stores env file as hashtable, for ease of access

# ests
$sqlFileExists = Test-Path -Path $sqlAbsolutePath
$envFileExists = Test-Path -Path $envAbsolutePath
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

# ----------------- main script functionality below -----------------------------------

try
{
    New-Item -Path $scriptAbsolutePath -Name $sciptoutputname -ItemType "File" -Force
}
catch
{
    return
}

$rowNum = Read-Host "Input number of rows to generate"

for ($index = 0; $index -le $rowNum; $index++)
{
    $Users.Add("$index")
}

#generate sql for users
Write-Host "Generating $rowNum user inserts"
for ($index = 1; $index -le $rowNum; $index++) #creates most basic user possible 
{
    Add-Content -Path ($scriptoutputpath) -Value (
    "INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash,
                            SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled,
                            LockoutEnd, LockoutEnabled, AccessFailedCount)
    VALUES ('$index', null, null, null, null, true, null, null, null,
            null, false, false, null, false, 0);")
}
Write-Host "    $rowNum user inserts generated"

Write-Host "Generating $rowNum userRole inserts"
for ($index = 1; $index -le $rowNum; $index++) #creates most basic user possible 
{
    Add-Content -Path ($scriptoutputpath) -Value (
    "INSERT INTO AspNetUserRoles (UserId, RoleId)
     VALUES ('$index', 'Pilot');")
}
Write-Host "    $rowNum userRole inserts generated"

#generate sql for obstacles
Write-Host "Generating $rowNum obstacle inserts"
for ($index = 1; $index -le $rowNum; $index++) 
{
    #obstacle table fields
    $userId = $Users[(Get-Random -Minimum 1 -Maximum $Users.Count)]
    $obstacleId = $index
    $heightMeter = (Get-Random -Maximum 150)
    $geometryGeoJson = (Get-Random -Maximum 5000)
    $name = (Get-Random -Maximum 5000)
    $description = (Get-Random -Maximum 5000)
    $illuminated = (Get-Random -Maximum 3) #illuminated table has 3 options from 0-2
    $type = (Get-Random -Maximum 6) #type  table has 6 options from 0-5
    $status = (Get-Random -Maximum 5) #status table has 4 options from 0-4
    $marking = (Get-Random -Maximum 3) #marking table has 2 options from 0-2
    # date time format YYYY:MM:DD HH:MM:SS
    $creationTime = ("$(Get-Random -Minimum 2021 -Maximum 2026)-$(Get-Random -Minimum 1 -Maximum 13)-$(Get-Random -Minimum 1 -Maximum 29) $(Get-Random -Minimum 1 -Maximum 24):$(Get-Random -Minimum 1 -Maximum 60):$(Get-Random -Minimum 1 -Maximum 60)")
    
    Add-Content -Path ($scriptoutputpath) -Value (
    "INSERT INTO Obstacle (ObstacleID, Heightmeter, GeometryGeoJson, Name, Description, Illuminated, Type, Status, Marking, CreationTime, UserId)
    VALUES ($obstacleId, $heightMeter, '$geometryGeoJson', '$name', '$description', $illuminated, $type, $status, $marking, '$creationTime', $userId);")
}
Write-Host "    $rowNum obstacle inserts generated"

#----------------------------------injecting script into docker container------------------------------------------
#   Clear database
if($r) 
{
    #TODO: goofy script maybe redo
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
    try
    {
        Write-Host "Injecting and executing script on container"
        Write-Host "    Injecting sql from @ $scriptoutputpath to container..."
        docker cp $scriptoutputpath db:/
        Write-Host "        Mock data script copied to container"

        Write-Host "    Executing sql script on $($envHash['MYSQL_DATABASE'])..."
        docker exec db sh -c "mariadb $($envHash['MYSQL_DATABASE']) -u root -p$($envHash['MYSQL_ROOT_PASSWORD']) <$sciptoutputname"
        Write-Host "COMPLETE: Sql executed, generated rows inserted"
        
        if($c) #delete script from local machine
        {
            Remove-Item -Path $scriptoutputpath
        } 
    }
    catch
    {
        return
    }
}