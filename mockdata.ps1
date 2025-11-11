$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
$sciptoutputname = "mockdataoutput.sql"
$scriptoutputpath = ($scriptDir + "/" + $sciptoutputname)

New-Item -Path $scriptDir -Name $sciptoutputname -ItemType "File" -Force

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


