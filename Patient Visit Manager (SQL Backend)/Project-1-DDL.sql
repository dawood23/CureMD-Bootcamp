-- project-01-DDL-safe.sql
-- 8th August, 2025
-- Dawood Nadeem 6601
-- Description: Multi-execution safe DDL script for Patient Visit Manager database

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'UserRoles')
BEGIN
    CREATE TABLE UserRoles (
        RoleID int PRIMARY KEY IDENTITY(1,1),
        RoleName varchar(50) NOT NULL UNIQUE,
        Description varchar(255)
    );
END


IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'VisitTypes')
BEGIN
    CREATE TABLE VisitTypes (
        VisitTypeID int PRIMARY KEY IDENTITY(1,1),
        TypeName varchar(50) NOT NULL UNIQUE,
        BaseFee decimal(10,2) NOT NULL,
        EstimatedDuration int NOT NULL 
    );
END


IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Patients')
BEGIN
    CREATE TABLE Patients (
        PatientID int PRIMARY KEY IDENTITY(1,1),
        FirstName varchar(100) NOT NULL,
        LastName varchar(100) NOT NULL,
        DateOfBirth date,
        PhoneNumber varchar(15),
        Email varchar(255),
        Address varchar(500),
        EmergencyContact varchar(255)
    );
END


IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Doctors')
BEGIN
    CREATE TABLE Doctors (
        DoctorID int PRIMARY KEY IDENTITY(1,1),
        FirstName varchar(100) NOT NULL,
        LastName varchar(100) NOT NULL,
        PhoneNumber varchar(15),
        Email varchar(255)
    );
END


IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        UserID int PRIMARY KEY IDENTITY(1,1),
        Username varchar(50) NOT NULL UNIQUE,
        PasswordHash varchar(255) NOT NULL,
        RoleID int NOT NULL,
        FirstName varchar(100) NOT NULL,
        LastName varchar(100) NOT NULL,
        FOREIGN KEY (RoleID) REFERENCES UserRoles(RoleID)
    );
END


IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Visits')
BEGIN
    CREATE TABLE Visits (
        VisitID int PRIMARY KEY IDENTITY(1,1),
        PatientID int NOT NULL,
        DoctorID int NULL,
        VisitTypeID int NOT NULL,
        VisitDate date NOT NULL,
        VisitTime time NOT NULL,
        Description varchar(1000),
        Notes varchar(1000),
        Status varchar(20) DEFAULT 'Scheduled',
        Fee decimal(10,2),
        CreatedBy int NOT NULL,
        FOREIGN KEY (PatientID) REFERENCES Patients(PatientID),
        FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID),
        FOREIGN KEY (VisitTypeID) REFERENCES VisitTypes(VisitTypeID),
        FOREIGN KEY (CreatedBy) REFERENCES Users(UserID)
    );
END


IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ActivityLog')
BEGIN
    CREATE TABLE ActivityLog (
        LogID int PRIMARY KEY IDENTITY(1,1),
        UserID int NOT NULL,
        Action varchar(50) NOT NULL, 
        TableAffected varchar(50) NOT NULL,
        RecordID int,
        Status varchar(20) NOT NULL, 
        Timestamp datetime DEFAULT GETDATE(),
        FOREIGN KEY (UserID) REFERENCES Users(UserID)
    );
END



IF NOT EXISTS (
    SELECT 1 FROM sys.check_constraints 
    WHERE name = 'CK_Visits_Fee'
)
BEGIN
    ALTER TABLE Visits 
    ADD CONSTRAINT CK_Visits_Fee CHECK (Fee >= 0);
END


IF NOT EXISTS (
    SELECT 1 FROM sys.check_constraints 
    WHERE name = 'CK_VisitTypes_BaseFee'
)
BEGIN
    ALTER TABLE VisitTypes 
    ADD CONSTRAINT CK_VisitTypes_BaseFee CHECK (BaseFee >= 0);
END


IF NOT EXISTS (
    SELECT 1 FROM sys.check_constraints 
    WHERE name = 'CK_Visits_Status'
)
BEGIN
    ALTER TABLE Visits 
    ADD CONSTRAINT CK_Visits_Status 
    CHECK (Status IN ('Scheduled', 'Completed', 'Cancelled'));
END

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
