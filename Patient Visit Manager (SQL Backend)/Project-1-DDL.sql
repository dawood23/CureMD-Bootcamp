-- project-01-DDL.sql
-- 8th August, 2025
-- Dawood Nadeem 6601
-- Description: Data Definition Language script for Patient Visit Manager database


create table UserRoles (
    RoleID int primary key identity(1,1),
    RoleName varchar(50) not null unique,
    Description varchar(255)
);

create table VisitTypes (
    VisitTypeID int primary key identity(1,1),
    TypeName varchar(50) not null unique,
    BaseFee decimal(10,2) not null,
    EstimatedDuration int not null 
);

create table Patients (
    PatientID int primary key identity(1,1),
    FirstName varchar(100) not null,
    LastName varchar(100) not null,
    DateOfBirth date,
    PhoneNumber varchar(15),
    Email varchar(255),
    Address varchar(500),
    EmergencyContact varchar(255)
);

create table Doctors (
    DoctorID int primary key identity(1,1),
    FirstName varchar(100) not null,
    LastName varchar(100) not null,
    PhoneNumber varchar(15),
    Email varchar(255)
);

create table Users (
    UserID int primary key identity(1,1),
    Username varchar(50) not null unique,
    PasswordHash varchar(255) not null,
    RoleID int not null,
    FirstName varchar(100) not null,
    LastName varchar(100) not null,
    foreign key (RoleID) references UserRoles(RoleID)
);

create table Visits (
    VisitID int primary key identity(1,1),
    PatientID int not null,
    DoctorID int null,
    VisitTypeID int not null,
    VisitDate date not null,
    VisitTime time not null,
    Description varchar(1000),
    Notes varchar(1000),
    Status varchar(20) default 'Scheduled', -- scheduled, completed, cancelled
    Fee decimal(10,2),
    CreatedBy int not null,
    foreign key (PatientID) references Patients(PatientID),
    foreign key (DoctorID) references Doctors(DoctorID),
    foreign key (VisitTypeID) references VisitTypes(VisitTypeID),
    foreign key (CreatedBy) references Users(UserID)
);

create table ActivityLog (
    LogID int primary key identity(1,1),
    UserID int not null,
    Action varchar(50) not null, 
    TableAffected varchar(50) not null,
    RecordID int,
    Status varchar(20) not null, 
    Timestamp datetime default getdate(),
    foreign key (UserID) references Users(UserID)
);

-- extra constraints
alter table Visits add constraint CK_Visits_Fee check (Fee >= 0);
alter table VisitTypes add constraint CK_VisitTypes_BaseFee check (BaseFee >= 0);

alter table Visits add constraint CK_Visits_Status 
    check (Status in ('Scheduled', 'Completed', 'Cancelled'));



/*
1NF (First Normal Form):
- All tables have atomic values (no repeating groups)
- Each row is unique (enforced by primary keys)

2NF (Second Normal Form):
- All tables are in 1NF
- All non-key attributes are fully dependent only on the primary key
- No partial dependencies (all tables use single-column primary keys (no composite primary keys))

3NF (Third Normal Form):
- All tables are in 2NF
- No transitive dependencies
*/
