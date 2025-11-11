$scriptAbsolutePath = Split-Path -Parent $MyInvocation.MyCommand.Definition
$sciptoutputname = "mockdataoutput.sql"
$scriptoutputpath = ($scriptAbsolutePath + "/" + $sciptoutputname)
$envAbsolutePath = $scriptAbsolutePath+"/.env"

New-Item -Path $scriptAbsolutePath -Name $sciptoutputname -ItemType "File" -Force

$rowNum = Read-Host "Input number of rows to generate"

$Users = [System.Collections.Generic.List[string]]::new() #store user id

for ($i = 0; $i -le $rowNum; $i++)
{
    $Users.Add("$i")
}

for ($i = 1; $i -le $rowNum; $i++) #creates most basic user possible 
{
    Add-Content -Path ($scriptoutputpath) -Value (
    "INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash,
                            SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled,
                            LockoutEnd, LockoutEnabled, AccessFailedCount)
    VALUES ('$i', null, null, null, null, true, null, null, null,
            null, false, false, null, false, 0);")
}

for ($i = 1; $i -le $rowNum; $i++) #creates most basic user possible 
{
    #obstacle table fields
    $userId = $Users[(Get-Random -Maximum $Users.Count)] #TODO: add later when users are assigned to obstacle
    $obstacleId = $i
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

docker cp $scriptoutputpath db:/
docker exec db sh -c "mariadb $($envHash['MYSQL_DATABASE']) -u root -p$($envHash['MYSQL_ROOT_PASSWORD']) <$sciptoutputname"

