-- project-02-DDL-safe.sql
-- 4th September, 2025
-- Dawood Nadeem 6601
-- Description: Multi-execution safe DDL script for Patient Visit Manager database

------------------------------------------------------------
-- 1) Roles Table
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Roles')
BEGIN
    CREATE TABLE Roles (
        RoleID int PRIMARY KEY IDENTITY(1,1),
        Name varchar(50) NOT NULL UNIQUE,
        Description varchar(255)
    );
END


------------------------------------------------------------
-- 2) Users Table
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        UserID int PRIMARY KEY IDENTITY(1,1),
        Username varchar(100) NOT NULL UNIQUE,
        PasswordHash varchar(255) NOT NULL,
        Email varchar(150) UNIQUE,
        Phone varchar(20),
        Active bit DEFAULT 1,
        RoleID int NOT NULL,
        FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
    );
END


------------------------------------------------------------
-- 3) Patients Table
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Patients')
BEGIN
    CREATE TABLE Patients (
        PatientID int PRIMARY KEY IDENTITY(1,1),
        FirstName varchar(100) NOT NULL,
        LastName varchar(100) NOT NULL,
        DOB date,
        Gender varchar(10),
        Phone varchar(20),
        Email varchar(150),
        Address varchar(500),
        CreatedAt datetime DEFAULT GETDATE()
    );
END

select * from Users

------------------------------------------------------------
-- 4) Doctors Table
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Doctors')
BEGIN
    CREATE TABLE Doctors (
        DoctorID int PRIMARY KEY IDENTITY(1,1),
		DoctorName varchar(50),
        Specialization varchar(100),
    );
END


------------------------------------------------------------
-- 5) Appointments Table
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Appointments')
BEGIN
    CREATE TABLE Appointments (
        AppointmentID int PRIMARY KEY IDENTITY(1,1),
        PatientID int NOT NULL,
        DoctorID int NOT NULL,
        CreatedBy int NOT NULL,
        StartTime datetime NOT NULL,
        DurationMinutes int NOT NULL,
        Status varchar(20) NOT NULL DEFAULT 'Scheduled',
        CreatedAt datetime DEFAULT GETDATE(),
        FOREIGN KEY (PatientID) REFERENCES Patients(PatientID),
        FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID),
        FOREIGN KEY (CreatedBy) REFERENCES Users(UserID)
    );
END


------------------------------------------------------------
-- 6) VisitNotes Table
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'VisitNotes')
BEGIN
    CREATE TABLE VisitNotes (
        VisitNoteID int PRIMARY KEY IDENTITY(1,1),
        AppointmentID int UNIQUE NOT NULL,
        DoctorID int NOT NULL,
        Content varchar(max),
        CreatedAt datetime DEFAULT GETDATE(),
        UpdatedAt datetime DEFAULT GETDATE(),
        FOREIGN KEY (AppointmentID) REFERENCES Appointments(AppointmentID),
        FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID)
    );
END


------------------------------------------------------------
-- 7) Bills Table
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Bills')
BEGIN
    CREATE TABLE Bills (
        BillID int PRIMARY KEY IDENTITY(1,1),
        AppointmentID int UNIQUE NOT NULL,
        PatientID int NOT NULL,
        GeneratedAt datetime DEFAULT GETDATE(),
        Status varchar(20) NOT NULL DEFAULT 'Open', 
        TotalAmount decimal(10,2) NOT NULL,
        PendingAmount decimal(10,2) NOT NULL,
        FOREIGN KEY (AppointmentID) REFERENCES Appointments(AppointmentID),
        FOREIGN KEY (PatientID) REFERENCES Patients(PatientID)
    );
END


------------------------------------------------------------
-- 8) Payments Table
------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Payments')
BEGIN
    CREATE TABLE Payments (
        PaymentID int PRIMARY KEY IDENTITY(1,1),
        BillID int NOT NULL,
        Amount decimal(10,2) NOT NULL,
        Method varchar(50) NOT NULL, 
        PaidAt datetime DEFAULT GETDATE(),
        RecordedBy int NOT NULL,
        FOREIGN KEY (BillID) REFERENCES Bills(BillID),
        FOREIGN KEY (RecordedBy) REFERENCES Users(UserID)
    );
END


------------------------------------------------------------
-- 9) Logs Table
------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'logs')
BEGIN
    CREATE TABLE logs (
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

select * from Users
update Users set PasswordHash='JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=' where UserID=1

select * from logs
------------------------------------------------------------
-- Constraints
------------------------------------------------------------
IF NOT EXISTS (
    SELECT 1 FROM sys.check_constraints 
    WHERE name = 'CK_Appointments_Duration'
)
BEGIN
    ALTER TABLE Appointments 
    ADD CONSTRAINT CK_Appointments_Duration 
    CHECK (DurationMinutes IN (15, 30, 60));
END


IF NOT EXISTS (
    SELECT 1 FROM sys.check_constraints 
    WHERE name = 'CK_Bills_PendingAmount'
)
BEGIN
    ALTER TABLE Bills 
    ADD CONSTRAINT CK_Bills_PendingAmount 
    CHECK (PendingAmount >= 0);
END

