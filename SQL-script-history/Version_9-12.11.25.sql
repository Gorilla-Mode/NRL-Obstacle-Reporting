create table Illuminated
(
    Illuminated     int          not null
        primary key,
    IlluminatedName varchar(100) null
);

create table Marking
(
    Marking   int          not null
        primary key,
    MarkingName varchar(100) not null
);

create table ObstacleType
(
    Type             int          not null
        primary key,
    ObstacleTypeName varchar(100) null
);

create table Status
(
    Status     int          not null
        primary key,
    StatusName varchar(100) null
);

create table test
(
    test_column int null
);

INSERT INTO ObstacleType (Type, ObstacleTypeName) VALUES (0, 'AirSpan');
INSERT INTO ObstacleType (Type, ObstacleTypeName) VALUES (1, 'PoleOrTower');
INSERT INTO ObstacleType (Type, ObstacleTypeName) VALUES (2, 'Building');
INSERT INTO ObstacleType (Type, ObstacleTypeName) VALUES (3, 'Construction');
INSERT INTO ObstacleType (Type, ObstacleTypeName) VALUES (4, 'Bridge');
INSERT INTO ObstacleType (Type, ObstacleTypeName) VALUES (5, 'Other');

INSERT INTO Illuminated (Illuminated, IlluminatedName) VALUES (0, 'Unknown');
INSERT INTO Illuminated (Illuminated, IlluminatedName) VALUES (1, 'Not illuminated');
INSERT INTO Illuminated (Illuminated, IlluminatedName) VALUES (2, 'Illuminated');

INSERT INTO Status (Status, StatusName) VALUES (0, 'Draft');
INSERT INTO Status (Status, StatusName) VALUES (1, 'Pending');
INSERT INTO Status (Status, StatusName) VALUES (2, 'Approved');
INSERT INTO Status (Status, StatusName) VALUES (3, 'Rejected');
INSERT INTO Status (Status, StatusName) VALUES (4, 'Deleted');

INSERT INTO Marking (Marking, MarkingName) VALUES (0, 'Unknown');
INSERT INTO Marking (Marking, MarkingName) VALUES (1, 'Not marked');
INSERT INTO Marking (Marking, MarkingName) VALUES (2, 'Marked');

create table if not EXISTS AspNetRoles
(
    Id varchar(255) not null,
    Name varchar(255),
    NormalizedName  varchar(255),
    ConcurrencyStamp  varchar(255),
    CONSTRAINT U_ROLE_ID_PK PRIMARY KEY (Id)
    );

create table if not EXISTS AspNetUsers
(
    Id varchar(255) not null unique,
    UserName varchar(255),
    NormalizedUserName varchar(255),
    Email varchar(255),
    NormalizedEmail varchar(255),
    EmailConfirmed bit not null,
    PasswordHash varchar(255),
    SecurityStamp varchar(255),
    ConcurrencyStamp varchar(255),
    PhoneNumber varchar(50),
    PhoneNumberConfirmed bit not null,
    TwoFactorEnabled bit not null,
    LockoutEnd TIMESTAMP,
    LockoutEnabled bit not null,
    AccessFailedCount int not null,
    CONSTRAINT PK_AspNetUsers PRIMARY KEY (Id)
    );
create table if not EXISTS AspNetUserTokens
(
    UserId varchar(255) not null,
    LoginProvider varchar(255) not null ,
    Name  varchar(255) not null,
    Value  varchar(255),
    CONSTRAINT PK_AspNetUserTokens PRIMARY KEY (UserId, LoginProvider)
    );

create table if not EXISTS AspNetRoleClaims
(
    Id int UNIQUE auto_increment,
    ClaimType varchar(255) not null ,
    ClaimValue  varchar(255) not null,
    RoleId  varchar(255),
    CONSTRAINT PK_AspNetRoleClaims PRIMARY KEY (Id),
    foreign key(RoleId)
    references AspNetRoles(Id)
    );

create table if not EXISTS AspNetUserClaims
(
    Id int UNIQUE auto_increment,
    ClaimType varchar(255) ,
    ClaimValue  varchar(255),
    UserId  varchar(255),
    CONSTRAINT PK_AspNetRoleClaims PRIMARY KEY (Id),
    foreign key(UserId)
    references AspNetUsers(Id)
    );

create table if not EXISTS AspNetUserLogins
(
    LoginProvider int UNIQUE auto_increment,
    ProviderKey varchar(255) not null ,
    ProviderDisplayName  varchar(255) not null,
    UserId  varchar(255) not null,
    CONSTRAINT PK_AspNetUserLogins PRIMARY KEY (LoginProvider),
    foreign key(UserId)
    references AspNetUsers(Id)
    );

create table if not EXISTS AspNetUserRoles
(
    UserId varchar(255) not null,
    RoleId varchar(255) not null,
    CONSTRAINT PK_AspNetUserRoles PRIMARY KEY (UserId,RoleId),
    foreign key(UserId)
    references AspNetUsers(Id),
    foreign key(RoleId)
    references AspNetRoles(Id)
    );

create table Obstacle
(
    ObstacleID      varchar(300)  not null
        primary key,
    Heightmeter     int           not null,
    GeometryGeoJson varchar(5000) null,
    Name            varchar(100)  null,
    Description     varchar(1000) null,
    Illuminated     int default 0 not null,
    Type            int           not null,
    Status          int default 0 not null,
    Marking         int default 0 not null,
    CreationTime    datetime      null,
    UpdatedTime     datetime      null,
    UserId          varchar(255)  not null,
    constraint ObstacleIlluminated_fk
        foreign key (Illuminated) references Illuminated (Illuminated),
    constraint ObstacleMarking_fk
        foreign key (Marking) references Marking (Marking),
    constraint ObstacleStatus_fk
        foreign key (Status) references Status (Status),
    constraint ObstacleType_fk
        foreign key (Type) references ObstacleType (Type),
    constraint Obstacle_UserId_fk
        foreign key (UserId) references AspNetUserRoles (UserId)
);

create index Obstacle_CreationTime_index
    on Obstacle (CreationTime);

create index Obstacle_Heightmeter_index
    on Obstacle (Heightmeter);

insert into AspNetRoles(id, Name, NormalizedName) values('Administrator', 'Administrator', 'Administrator');
insert into AspNetRoles(id, Name, NormalizedName) values('Pilot', 'Pilot', 'Pilot');
insert into AspNetRoles(id, Name, NormalizedName) values('Registrar', 'Registrar', 'Registrar');

INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash,
                         SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled,
                         LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES ('b5154125-4050-4522-afd0-d71a84938df3', 'admin@admin.com', 'ADMIN@ADMIN.COM', 'admin@admin.com', 'ADMIN@ADMIN.COM', true,
        'AQAAAAIAAYagAAAAEE5UOcXNc0M2tDbFivSsqOqsMZrlLO+GUZXozDcylCr+NukWhQD7ZaPzwBp4RvIRpA==',
        '6daad3e4-367b-4961-98bc-c1846778e7f9', '81d4a027-bd4f-4bad-ab8f-60f1dd1e615d', null, false, false, null, false,
        0);
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash,
                         SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled,
                         LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES ('658bd502-a855-4a06-b0bd-a48e874e1803', 'pilot@pilot.com', 'PILOT@PILOT.COM', 'pilot@pilot.com', 'PILOT@PILOT.COM', true,
        'AQAAAAIAAYagAAAAELOYsX4PP8SOfJdSoj+16nnkZii3rwcnsH39fQJ0RpCD1RZGNQiVfUjqfpKXcYD7cw==',
        'c851a964-3dc1-459f-9e99-c80c47f98414', '772efdb9-8209-482e-9606-d75b711cf3e7', null, false, false, null, false,
        0);

INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash,
                         SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled,
                         LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES ('51d67027-dc61-4b05-b33b-d02481e9cb2b', 'registrar@registrar.com', 'REGISTRAR@REGISTRAR.COM', 'registrar@registrar.com',
        'REGISTRAR@REGISTRAR.COM', true,
        'AQAAAAIAAYagAAAAEDBuir2sH0Q4eCYdyWluFA+4ZiMaW72/Wcu0J28hu96bdJ4q160KQy5BygfW0K3L9g==',
        '265585ee-c429-4815-bbd8-57e27824d377', '600b3f4e-e723-4301-8a13-2e51a77dddab', null, false, false, null, false,
        0);

INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES ('b5154125-4050-4522-afd0-d71a84938df3', 'Administrator');

INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES ('658bd502-a855-4a06-b0bd-a48e874e1803', 'Pilot');

INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES ('51d67027-dc61-4b05-b33b-d02481e9cb2b', 'Registrar');

