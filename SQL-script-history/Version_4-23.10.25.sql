create table Illuminated
(
    Illuminated     int          not null
        primary key,
    IlluminatedName varchar(100) null
);

create table ObstacleType
(
    Type             int          not null
        primary key,
    ObstacleTypeName varchar(100) null
);

create table Obstacle
(
    ObstacleID      int           not null
        primary key,
    Heightmeter     int           null,
    GeometryGeoJson varchar(5000)  null,
    Name            varchar(100)  null,
    Description     varchar(1000) null,
    Illuminated     int           null,
    Type            int           null,
    constraint Illuminated_fk
        foreign key (Illuminated) references Illuminated (Illuminated),
    constraint Type_fk
        foreign key (Type) references ObstacleType (Type)
);

create table Pilot
(
    PilotID      int          not null
        primary key,
    Organization varchar(100) null,
    Name         varchar(100) null
);

create table Registrar
(
    RegistrarID  int          not null
        primary key,
    Name         varchar(100) null,
    Role         varchar(100) null,
    Organization varchar(100) null
);

create table Status
(
    Status     int          not null
        primary key,
    StatusName varchar(100) null
);

create table Report
(
    ReportID int  not null
        primary key,
    Date     date null,
    Status   int  null,
    PilotID  int  null,
    constraint PilotID_fk
        foreign key (PilotID) references Pilot (PilotID),
    constraint Status_fk
        foreign key (Status) references Status (Status)
);

create table Authority
(
    RegistrarID int not null,
    ReportID    int not null,
    constraint RegistrarID_fk
        foreign key (RegistrarID) references Registrar (RegistrarID),
    constraint ReportID_fk
        foreign key (ReportID) references Report (ReportID)
);

create table test
(
    test_column int null
);

INSERT INTO ObstacleType (Type, ObstacleTypeName) VALUES (0, 'Powerline');
INSERT INTO ObstacleType (Type, ObstacleTypeName) VALUES (1, 'Pole');
INSERT INTO ObstacleType (Type, ObstacleTypeName) VALUES (2, 'Building');
INSERT INTO ObstacleType (Type, ObstacleTypeName) VALUES (3, 'Construction');
INSERT INTO ObstacleType (Type, ObstacleTypeName) VALUES (4, 'Natural');
INSERT INTO ObstacleType (Type, ObstacleTypeName) VALUES (5, 'Other');

INSERT INTO Illuminated (Illuminated, IlluminatedName) VALUES (0, 'Unknown');
INSERT INTO Illuminated (Illuminated, IlluminatedName) VALUES (1, 'Not illuminated');
INSERT INTO Illuminated (Illuminated, IlluminatedName) VALUES (2, 'Illuminated');

INSERT INTO Status (Status, StatusName) VALUES (0, 'Draft');
INSERT INTO Status (Status, StatusName) VALUES (1, 'Pending');
INSERT INTO Status (Status, StatusName) VALUES (2, 'Approved');
INSERT INTO Status (Status, StatusName) VALUES (3, 'Rejected');
INSERT INTO Status (Status, StatusName) VALUES (4, 'Deleted');




