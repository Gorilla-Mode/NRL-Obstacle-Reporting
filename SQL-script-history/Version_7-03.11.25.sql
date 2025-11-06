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

create table Obstacle
(
    ObstacleID      int           not null
        primary key,
    Heightmeter     int           not null,
    GeometryGeoJson varchar(5000) null,
    Name            varchar(100)  null,
    Description     varchar(1000) null,
    Illuminated     int default 0 not null,
    Type            int           not null,
    Status          int default 0 not null,
    Marking         int default 0 not null,
    constraint ObstacleIlluminated_fk
        foreign key (Illuminated) references Illuminated (Illuminated),
    constraint ObstacleMarking_fk
        foreign key (Marking) references Marking (Marking),
    constraint ObstacleStatus_fk
        foreign key (Status) references Status (Status),
    constraint ObstacleType_fk
        foreign key (Type) references ObstacleType (Type)
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

insert into AspNetRoles(id, Name, NormalizedName) values('Administrator', 'Administrator', 'Administrator');
insert into AspNetRoles(id, Name, NormalizedName) values('Pilot', 'Pilot', 'Pilot');
insert into AspNetRoles(id, Name, NormalizedName) values('Registrar', 'Registrar', 'Registrar');