-- ===========================================================
-- Enumeration tables: used by obstacle
-- ============================================================

CREATE TABLE IF NOT EXISTS Illuminated
(
    Illuminated     INT          NOT NULL PRIMARY KEY,
    IlluminatedName VARCHAR(100) NULL
);

CREATE TABLE IF NOT EXISTS Marking
(
    Marking   INT          NOT NULL PRIMARY KEY,
    MarkingName VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS ObstacleType
(
    Type             INT          NOT NULL PRIMARY KEY,
    ObstacleTypeName VARCHAR(100) NULL
);

CREATE TABLE IF NOT EXISTS Status
(
    Status     INT          NOT NULL PRIMARY KEY,
    StatusName VARCHAR(100) NULL
);

-- ===========================================================
-- Test table: Used ONLY for integration testing
-- ===========================================================

CREATE TABLE IF NOT EXISTS test
(
    test_column INT NULL
);

-- ===========================================================
-- Enumeration tables: insert default values
-- ===========================================================

INSERT INTO ObstacleType (Type, ObstacleTypeName)
SELECT 0, 'AirSpan' WHERE NOT EXISTS (SELECT 1 FROM ObstacleType WHERE Type = 0);
INSERT INTO ObstacleType (Type, ObstacleTypeName)
SELECT 1, 'PoleOrTower' WHERE NOT EXISTS (SELECT 1 FROM ObstacleType WHERE Type = 1);
INSERT INTO ObstacleType (Type, ObstacleTypeName)
SELECT 2, 'Building' WHERE NOT EXISTS (SELECT 1 FROM ObstacleType WHERE Type = 2);
INSERT INTO ObstacleType (Type, ObstacleTypeName)
SELECT 3, 'Construction' WHERE NOT EXISTS (SELECT 1 FROM ObstacleType WHERE Type = 3);
INSERT INTO ObstacleType (Type, ObstacleTypeName)
SELECT 4, 'Bridge' WHERE NOT EXISTS (SELECT 1 FROM ObstacleType WHERE Type = 4);
INSERT INTO ObstacleType (Type, ObstacleTypeName)
SELECT 5, 'Other' WHERE NOT EXISTS (SELECT 1 FROM ObstacleType WHERE Type = 5);

INSERT INTO Illuminated (Illuminated, IlluminatedName)
SELECT 0, 'Unknown' WHERE NOT EXISTS (SELECT 1 FROM Illuminated WHERE Illuminated = 0);
INSERT INTO Illuminated (Illuminated, IlluminatedName)
SELECT 1, 'Not illuminated' WHERE NOT EXISTS (SELECT 1 FROM Illuminated WHERE Illuminated = 1);
INSERT INTO Illuminated (Illuminated, IlluminatedName)
SELECT 2, 'Illuminated' WHERE NOT EXISTS (SELECT 1 FROM Illuminated WHERE Illuminated = 2);

INSERT INTO Status (Status, StatusName)
SELECT 0, 'Draft' WHERE NOT EXISTS (SELECT 1 FROM Status WHERE Status = 0);
INSERT INTO Status (Status, StatusName)
SELECT 1, 'Pending' WHERE NOT EXISTS (SELECT 1 FROM Status WHERE Status = 1);
INSERT INTO Status (Status, StatusName)
SELECT 2, 'Approved' WHERE NOT EXISTS (SELECT 1 FROM Status WHERE Status = 2);
INSERT INTO Status (Status, StatusName)
SELECT 3, 'Rejected' WHERE NOT EXISTS (SELECT 1 FROM Status WHERE Status = 3);
INSERT INTO Status (Status, StatusName)
SELECT 4, 'Deleted' WHERE NOT EXISTS (SELECT 1 FROM Status WHERE Status = 4);

INSERT INTO Marking (Marking, MarkingName)
SELECT 0, 'Unknown' WHERE NOT EXISTS (SELECT 1 FROM Marking WHERE Marking = 0);
INSERT INTO Marking (Marking, MarkingName)
SELECT 1, 'Not marked' WHERE NOT EXISTS (SELECT 1 FROM Marking WHERE Marking = 1);
INSERT INTO Marking (Marking, MarkingName)
SELECT 2, 'Marked' WHERE NOT EXISTS (SELECT 1 FROM Marking WHERE Marking = 2);

-- ===========================================================
-- ASP.NET Identity tables: From Microsoft
-- ===========================================================

CREATE TABLE IF NOT EXISTS AspNetRoles
(
    Id VARCHAR(255) NOT NULL,
    Name VARCHAR(255),
    NormalizedName  VARCHAR(255),
    ConcurrencyStamp  VARCHAR(255),
    CONSTRAINT U_ROLE_ID_PK PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS AspNetUsers
(
    Id VARCHAR(255) NOT NULL UNIQUE,
    UserName VARCHAR(255),
    NormalizedUserName VARCHAR(255),
    Email VARCHAR(255),
    NormalizedEmail VARCHAR(255),
    EmailConfirmed BIT NOT NULL,
    PasswordHash VARCHAR(255),
    SecurityStamp VARCHAR(255),
    ConcurrencyStamp VARCHAR(255),
    PhoneNumber VARCHAR(50),
    PhoneNumberConfirmed BIT NOT NULL,
    TwoFactorEnabled BIT NOT NULL,
    LockoutEnd TIMESTAMP,
    LockoutEnabled BIT NOT NULL,
    AccessFailedCount INT NOT NULL,
    CONSTRAINT PK_AspNetUsers PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS AspNetUserTokens
(
    UserId VARCHAR(255) NOT NULL,
    LoginProvider VARCHAR(255) NOT NULL,
    Name VARCHAR(255) NOT NULL,
    Value VARCHAR(255),
    CONSTRAINT PK_AspNetUserTokens PRIMARY KEY (UserId, LoginProvider)
);

CREATE TABLE IF NOT EXISTS AspNetRoleClaims
(
    Id INT UNIQUE AUTO_INCREMENT,
    ClaimType VARCHAR(255) NOT NULL,
    ClaimValue VARCHAR(255) NOT NULL,
    RoleId VARCHAR(255),
    CONSTRAINT PK_AspNetRoleClaims PRIMARY KEY (Id),
    FOREIGN KEY(RoleId)
        REFERENCES AspNetRoles(Id)
);

CREATE TABLE IF NOT EXISTS AspNetUserClaims
(
    Id INT UNIQUE AUTO_INCREMENT,
    ClaimType VARCHAR(255),
    ClaimValue VARCHAR(255),
    UserId VARCHAR(255),
    CONSTRAINT PK_AspNetRoleClaims PRIMARY KEY (Id),
    FOREIGN KEY(UserId)
        REFERENCES AspNetUsers(Id)
);

CREATE TABLE IF NOT EXISTS AspNetUserLogins
(
    LoginProvider INT UNIQUE AUTO_INCREMENT,
    ProviderKey VARCHAR(255) NOT NULL,
    ProviderDisplayName VARCHAR(255) NOT NULL,
    UserId VARCHAR(255) NOT NULL,
    CONSTRAINT PK_AspNetUserLogins PRIMARY KEY (LoginProvider),
    FOREIGN KEY(UserId)
        REFERENCES AspNetUsers(Id)
);

CREATE TABLE IF NOT EXISTS AspNetUserRoles
(
    UserId VARCHAR(255) NOT NULL,
    RoleId VARCHAR(255) NOT NULL,
    CONSTRAINT PK_AspNetUserRoles PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY(UserId)
        REFERENCES AspNetUsers(Id),
    FOREIGN KEY(RoleId)
        REFERENCES AspNetRoles(Id)
);

-- ===========================================================
-- Obstacle: Main table, including keys to related tables
-- ===========================================================

CREATE TABLE IF NOT EXISTS Obstacle
(
    ObstacleID      VARCHAR(300)  NOT NULL
        PRIMARY KEY,
    Heightmeter     INT           NOT NULL,
    GeometryGeoJson VARCHAR(5000) NULL,
    Name            VARCHAR(100)  NULL,
    Description     VARCHAR(1000) NULL,
    Illuminated     INT DEFAULT 0 NOT NULL,
    Type            INT           NOT NULL,
    Status          INT DEFAULT 0 NOT NULL,
    Marking         INT DEFAULT 0 NOT NULL,
    CreationTime    DATETIME      NULL,
    UpdatedTime     DATETIME      NULL,
    UserId          VARCHAR(255)  NOT NULL,
    CONSTRAINT ObstacleIlluminated_fk
        FOREIGN KEY (Illuminated) REFERENCES Illuminated (Illuminated),
    CONSTRAINT ObstacleMarking_fk
        FOREIGN KEY (Marking) REFERENCES Marking (Marking),
    CONSTRAINT ObstacleStatus_fk
        FOREIGN KEY (Status) REFERENCES Status (Status),
    CONSTRAINT ObstacleType_fk
        FOREIGN KEY (Type) REFERENCES ObstacleType (Type),
    CONSTRAINT Obstacle_UserId_fk
        FOREIGN KEY (UserId) REFERENCES AspNetUserRoles (UserId)
);

-- ===========================================================
-- Index: Used for searching
-- ===========================================================

CREATE INDEX IF NOT EXISTS Obstacle_CreationTime_index
    ON Obstacle (CreationTime);

CREATE INDEX IF NOT EXISTS Obstacle_Heightmeter_index
    ON Obstacle (Heightmeter);

-- ===========================================================
-- Views: Combine relevant tables for easy querying
-- ===========================================================

CREATE VIEW IF NOT EXISTS view_ObstacleUser AS
SELECT UserId, ObstacleID, UserName, NormalizedUserName, Heightmeter, GeometryGeoJson, Name, Description, Illuminated,
       type, Status, Marking, CreationTime, UpdatedTime, Email, NormalizedEmail, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed
FROM Obstacle AS o
         INNER JOIN AspNetUsers AS u ON o.UserId = u.Id;

CREATE VIEW IF NOT EXISTS view_UserRole AS
SELECT UserId, RoleId, Name, NormalizedName, UserName, NormalizedUserName, Email, NormalizedEmail, r.ConcurrencyStamp AS RoleConcurrencyStamp,
       PhoneNumber, EmailConfirmed, SecurityStamp, u.ConcurrencyStamp AS UserConcurrencyStamp, PhoneNumberConfirmed,
       TwoFactorEnabled, LockoutEnabled, LockoutEnd, AccessFailedCount
FROM AspNetUserRoles AS ur
         INNER JOIN AspNetUsers AS u ON ur.UserId = u.Id
         INNER JOIN AspNetRoles AS r ON r.Id = ur.RoleId;

-- ===========================================================
-- ASP.NET Identity tables: insert default users with roles
-- ===========================================================

INSERT INTO AspNetRoles(id, Name, NormalizedName)
SELECT 'Administrator', 'Administrator', 'Administrator' WHERE NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE id = 'Administrator');

INSERT INTO AspNetRoles(id, Name, NormalizedName)
SELECT 'Pilot', 'Pilot', 'Pilot' WHERE NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE id = 'Pilot');

INSERT INTO AspNetRoles(id, Name, NormalizedName)
SELECT 'Registrar', 'Registrar', 'Registrar' WHERE NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE id = 'Registrar');

INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash,
                         SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled,
                         LockoutEnd, LockoutEnabled, AccessFailedCount)
SELECT 'b5154125-4050-4522-afd0-d71a84938df3', 'admin@admin.com', 'ADMIN@ADMIN.COM', 'admin@admin.com', 'ADMIN@ADMIN.COM', TRUE,
       'AQAAAAIAAYagAAAAEE5UOcXNc0M2tDbFivSsqOqsMZrlLO+GUZXozDcylCr+NukWhQD7ZaPzwBp4RvIRpA==',
       '6daad3e4-367b-4961-98bc-c1846778e7f9', '81d4a027-bd4f-4bad-ab8f-60f1dd1e615d', NULL, FALSE, FALSE, NULL, FALSE,
       0 WHERE NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE Id = 'b5154125-4050-4522-afd0-d71a84938df3');

INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash,
                         SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled,
                         LockoutEnd, LockoutEnabled, AccessFailedCount)
SELECT '658bd502-a855-4a06-b0bd-a48e874e1803', 'pilot@pilot.com', 'PILOT@PILOT.COM', 'pilot@pilot.com', 'PILOT@PILOT.COM', TRUE,
       'AQAAAAIAAYagAAAAELOYsX4PP8SOfJdSoj+16nnkZii3rwcnsH39fQJ0RpCD1RZGNQiVfUjqfpKXcYD7cw==',
       'c851a964-3dc1-459f-9e99-c80c47f98414', '772efdb9-8209-482e-9606-d75b711cf3e7', NULL, FALSE, FALSE, NULL, FALSE,
       0 WHERE NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE Id = '658bd502-a855-4a06-b0bd-a48e874e1803');

INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash,
                         SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled,
                         LockoutEnd, LockoutEnabled, AccessFailedCount)
SELECT '51d67027-dc61-4b05-b33b-d02481e9cb2b', 'registrar@registrar.com', 'REGISTRAR@REGISTRAR.COM', 'registrar@registrar.com',
       'REGISTRAR@REGISTRAR.COM', TRUE,
       'AQAAAAIAAYagAAAAEDBuir2sH0Q4eCYdyWluFA+4ZiMaW72/Wcu0J28hu96bdJ4q160KQy5BygfW0K3L9g==',
       '265585ee-c429-4815-bbd8-57e27824d377', '600b3f4e-e723-4301-8a13-2e51a77dddab', NULL, FALSE, FALSE, NULL, FALSE,
       0 WHERE NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE Id = '51d67027-dc61-4b05-b33b-d02481e9cb2b');

INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT 'b5154125-4050-4522-afd0-d71a84938df3', 'Administrator' WHERE NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId = 'b5154125-4050-4522-afd0-d71a84938df3' AND RoleId = 'Administrator');

INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT '658bd502-a855-4a06-b0bd-a48e874e1803', 'Pilot' WHERE NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId = '658bd502-a855-4a06-b0bd-a48e874e1803' AND RoleId = 'Pilot');

INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT '51d67027-dc61-4b05-b33b-d02481e9cb2b', 'Registrar' WHERE NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId = '51d67027-dc61-4b05-b33b-d02481e9cb2b' AND RoleId = 'Registrar');