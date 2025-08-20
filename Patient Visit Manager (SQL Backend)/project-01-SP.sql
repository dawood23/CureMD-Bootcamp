-- project-01-SP-safe.sql
-- 8th August, 2025
-- Dawood Nadeem 6601
-- Multi-execution safe stored procedures for Patient Visit Manager

IF OBJECT_ID('stp_AddUserRole', 'P') IS NOT NULL DROP PROCEDURE stp_AddUserRole;
GO
CREATE PROCEDURE stp_AddUserRole
    @RoleName varchar(50),
    @Description varchar(255) = null
AS
BEGIN
    BEGIN TRY
        INSERT INTO UserRoles (RoleName, Description)
        VALUES (@RoleName, @Description);
        SELECT SCOPE_IDENTITY() AS NewRoleID;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

IF OBJECT_ID('stp_GetUserRoles', 'P') IS NOT NULL DROP PROCEDURE stp_GetUserRoles;
GO
CREATE PROCEDURE stp_GetUserRoles
AS
BEGIN
    SELECT * FROM UserRoles;
END
GO

IF OBJECT_ID('stp_UpdateUserRole', 'P') IS NOT NULL DROP PROCEDURE stp_UpdateUserRole;
GO
CREATE PROCEDURE stp_UpdateUserRole
    @RoleID int,
    @RoleName varchar(50),
    @Description varchar(255)
AS
BEGIN
    UPDATE UserRoles
    SET RoleName = @RoleName, Description = @Description
    WHERE RoleID = @RoleID;
END
GO

IF OBJECT_ID('stp_DeleteUserRole', 'P') IS NOT NULL DROP PROCEDURE stp_DeleteUserRole;
GO
CREATE PROCEDURE stp_DeleteUserRole
    @RoleID int
AS
BEGIN
    DELETE FROM UserRoles WHERE RoleID = @RoleID;
END
GO


IF OBJECT_ID('stp_AddVisitType', 'P') IS NOT NULL DROP PROCEDURE stp_AddVisitType;
GO
CREATE PROCEDURE stp_AddVisitType
    @TypeName varchar(50),
    @BaseFee decimal(10,2),
    @EstimatedDuration int
AS
BEGIN
    INSERT INTO VisitTypes (TypeName, BaseFee, EstimatedDuration)
    VALUES (@TypeName, @BaseFee, @EstimatedDuration);
    SELECT SCOPE_IDENTITY() AS NewVisitTypeID;
END
GO

IF OBJECT_ID('stp_GetVisitTypes', 'P') IS NOT NULL DROP PROCEDURE stp_GetVisitTypes;
GO
CREATE PROCEDURE stp_GetVisitTypes
AS
BEGIN
    SELECT * FROM VisitTypes;
END
GO

IF OBJECT_ID('stp_UpdateVisitType', 'P') IS NOT NULL DROP PROCEDURE stp_UpdateVisitType;
GO
CREATE PROCEDURE stp_UpdateVisitType
    @VisitTypeID int,
    @TypeName varchar(50),
    @BaseFee decimal(10,2),
    @EstimatedDuration int
AS
BEGIN
    UPDATE VisitTypes
    SET TypeName = @TypeName, BaseFee = @BaseFee, EstimatedDuration = @EstimatedDuration
    WHERE VisitTypeID = @VisitTypeID;
END
GO

IF OBJECT_ID('stp_DeleteVisitType', 'P') IS NOT NULL DROP PROCEDURE stp_DeleteVisitType;
GO
CREATE PROCEDURE stp_DeleteVisitType
    @VisitTypeID int
AS
BEGIN
    DELETE FROM VisitTypes WHERE VisitTypeID = @VisitTypeID;
END
GO

IF OBJECT_ID('stp_GetVisitTypeById', 'P') IS NOT NULL DROP PROCEDURE stp_GetVisitTypeById;
GO
CREATE PROCEDURE stp_GetVisitTypeById
    @VisitTypeID int
AS
BEGIN
    SELECT * FROM VisitTypes WHERE VisitTypeID = @VisitTypeID;
END
GO


IF OBJECT_ID('stp_AddPatient', 'P') IS NOT NULL DROP PROCEDURE stp_AddPatient;
GO
CREATE PROCEDURE stp_AddPatient
    @FirstName varchar(100),
    @LastName varchar(100),
    @DateOfBirth date = null,
    @PhoneNumber varchar(15) = null,
    @Email varchar(255) = null,
    @Address varchar(500) = null,
    @EmergencyContact varchar(255) = null
AS
BEGIN
    INSERT INTO Patients (FirstName, LastName, DateOfBirth, PhoneNumber, Email, Address, EmergencyContact)
    VALUES (@FirstName, @LastName, @DateOfBirth, @PhoneNumber, @Email, @Address, @EmergencyContact);
    SELECT SCOPE_IDENTITY() AS NewPatientID;
END
GO

IF OBJECT_ID('stp_GetPatients', 'P') IS NOT NULL DROP PROCEDURE stp_GetPatients;
GO
CREATE PROCEDURE stp_GetPatients
AS
BEGIN
    SELECT * FROM Patients;
END
GO

IF OBJECT_ID('stp_GetPatientById', 'P') IS NOT NULL DROP PROCEDURE stp_GetPatientById;
GO
CREATE PROCEDURE stp_GetPatientById
    @PatientID int
AS
BEGIN
    SELECT * FROM Patients WHERE PatientID = @PatientID;
END
GO

IF OBJECT_ID('stp_UpdatePatient', 'P') IS NOT NULL DROP PROCEDURE stp_UpdatePatient;
GO
CREATE PROCEDURE stp_UpdatePatient
    @PatientID int,
    @FirstName varchar(100),
    @LastName varchar(100),
    @DateOfBirth date = null,
    @PhoneNumber varchar(15) = null,
    @Email varchar(255) = null,
    @Address varchar(500) = null,
    @EmergencyContact varchar(255) = null
AS
BEGIN
    UPDATE Patients
    SET FirstName = @FirstName,
        LastName = @LastName,
        DateOfBirth = @DateOfBirth,
        PhoneNumber = @PhoneNumber,
        Email = @Email,
        Address = @Address,
        EmergencyContact = @EmergencyContact
    WHERE PatientID = @PatientID;
END
GO

IF OBJECT_ID('stp_DeletePatient', 'P') IS NOT NULL DROP PROCEDURE stp_DeletePatient;
GO
CREATE PROCEDURE stp_DeletePatient
    @PatientID int
AS
BEGIN
    DELETE FROM Patients WHERE PatientID = @PatientID;
END
GO


IF OBJECT_ID('stp_AddDoctor', 'P') IS NOT NULL DROP PROCEDURE stp_AddDoctor;
GO
CREATE PROCEDURE stp_AddDoctor
    @FirstName varchar(100),
    @LastName varchar(100),
    @PhoneNumber varchar(15) = null,
    @Email varchar(255) = null
AS
BEGIN
    INSERT INTO Doctors (FirstName, LastName, PhoneNumber, Email)
    VALUES (@FirstName, @LastName, @PhoneNumber, @Email);
    SELECT SCOPE_IDENTITY() AS NewDoctorID;
END
GO

IF OBJECT_ID('stp_GetDoctors', 'P') IS NOT NULL DROP PROCEDURE stp_GetDoctors;
GO
CREATE PROCEDURE stp_GetDoctors
AS
BEGIN
    SELECT * FROM Doctors;
END
GO

IF OBJECT_ID('stp_UpdateDoctor', 'P') IS NOT NULL DROP PROCEDURE stp_UpdateDoctor;
GO
CREATE PROCEDURE stp_UpdateDoctor
    @DoctorID int,
    @FirstName varchar(100),
    @LastName varchar(100),
    @PhoneNumber varchar(15) = null,
    @Email varchar(255) = null
AS
BEGIN
    UPDATE Doctors
    SET FirstName = @FirstName,
        LastName = @LastName,
        PhoneNumber = @PhoneNumber,
        Email = @Email
    WHERE DoctorID = @DoctorID;
END
GO

IF OBJECT_ID('stp_DeleteDoctor', 'P') IS NOT NULL DROP PROCEDURE stp_DeleteDoctor;
GO
CREATE PROCEDURE stp_DeleteDoctor
    @DoctorID int
AS
BEGIN
    DELETE FROM Doctors WHERE DoctorID = @DoctorID;
END
GO

IF OBJECT_ID('stp_GetDoctorById', 'P') IS NOT NULL DROP PROCEDURE stp_GetDoctorById;
GO
CREATE PROCEDURE stp_GetDoctorById
    @DoctorID int
AS
BEGIN
    SELECT * FROM Doctors WHERE DoctorID = @DoctorID;
END
GO


IF OBJECT_ID('stp_AddUser', 'P') IS NOT NULL DROP PROCEDURE stp_AddUser;
GO
CREATE PROCEDURE stp_AddUser
    @Username varchar(50),
    @PasswordHash varchar(255),
    @RoleID int,
    @FirstName varchar(100),
    @LastName varchar(100)
AS
BEGIN
    INSERT INTO Users (Username, PasswordHash, RoleID, FirstName, LastName)
    VALUES (@Username, @PasswordHash, @RoleID, @FirstName, @LastName);
    SELECT SCOPE_IDENTITY() AS NewUserID;
END
GO

IF OBJECT_ID('stp_GetUsers', 'P') IS NOT NULL DROP PROCEDURE stp_GetUsers;
GO
CREATE PROCEDURE stp_GetUsers
AS
BEGIN
    SELECT * FROM Users;
END
GO

IF OBJECT_ID('stp_UpdateUser', 'P') IS NOT NULL DROP PROCEDURE stp_UpdateUser;
GO
CREATE PROCEDURE stp_UpdateUser
    @UserID int,
    @Username varchar(50),
    @PasswordHash varchar(255),
    @RoleID int,
    @FirstName varchar(100),
    @LastName varchar(100)
AS
BEGIN
    UPDATE Users
    SET Username = @Username,
        PasswordHash = @PasswordHash,
        RoleID = @RoleID,
        FirstName = @FirstName,
        LastName = @LastName
    WHERE UserID = @UserID;
END
GO

IF OBJECT_ID('stp_DeleteUser', 'P') IS NOT NULL DROP PROCEDURE stp_DeleteUser;
GO
CREATE PROCEDURE stp_DeleteUser
    @UserID int
AS
BEGIN
    DELETE FROM Users WHERE UserID = @UserID;
END
GO

IF OBJECT_ID('stp_GetUserById', 'P') IS NOT NULL DROP PROCEDURE stp_GetUserById;
GO
CREATE PROCEDURE stp_GetUserById
    @UserID int
AS
BEGIN
    SELECT * FROM Users WHERE UserID = @UserID;
END
GO


IF OBJECT_ID('stp_GetUserByName', 'P') IS NOT NULL DROP PROCEDURE stp_GetUserByName;
GO
CREATE PROCEDURE stp_GetUserByName
    @Username varchar(50)
AS
BEGIN
    SELECT * FROM Users WHERE Username = @Username;
END
GO


IF OBJECT_ID('stp_AddVisit', 'P') IS NOT NULL DROP PROCEDURE stp_AddVisit;
GO
CREATE PROCEDURE stp_AddVisit
    @PatientID int,
    @DoctorID int = null,
    @VisitTypeID int,
    @VisitDate date,
    @VisitTime time,
    @Description varchar(1000) = null,
    @Notes varchar(1000) = null,
    @Status varchar(20) = 'Scheduled',
    @Fee decimal(10,2) = null,
    @CreatedBy int
AS
BEGIN
    INSERT INTO Visits (PatientID, DoctorID, VisitTypeID, VisitDate, VisitTime, Description, Notes, Status, Fee, CreatedBy)
    VALUES (@PatientID, @DoctorID, @VisitTypeID, @VisitDate, @VisitTime, @Description, @Notes, @Status, @Fee, @CreatedBy);
    SELECT SCOPE_IDENTITY() AS NewVisitID;
END
GO

IF OBJECT_ID('stp_GetVisits', 'P') IS NOT NULL DROP PROCEDURE stp_GetVisits;
GO
CREATE PROCEDURE stp_GetVisits
AS
BEGIN
    SELECT * FROM Visits;
END
GO

IF OBJECT_ID('stp_UpdateVisit', 'P') IS NOT NULL DROP PROCEDURE stp_UpdateVisit;
GO
CREATE PROCEDURE stp_UpdateVisit
    @VisitID int,
    @PatientID int,
    @DoctorID int = null,
    @VisitTypeID int,
    @VisitDate date,
    @VisitTime time,
    @Description varchar(1000) = null,
    @Notes varchar(1000) = null,
    @Status varchar(20) = 'Scheduled',
    @Fee decimal(10,2) = null,
    @CreatedBy int
AS
BEGIN
    UPDATE Visits
    SET PatientID = @PatientID,
        DoctorID = @DoctorID,
        VisitTypeID = @VisitTypeID,
        VisitDate = @VisitDate,
        VisitTime = @VisitTime,
        Description = @Description,
        Notes = @Notes,
        Status = @Status,
        Fee = @Fee,
        CreatedBy = @CreatedBy
    WHERE VisitID = @VisitID;
END
GO

IF OBJECT_ID('stp_DeleteVisit', 'P') IS NOT NULL DROP PROCEDURE stp_DeleteVisit;
GO
CREATE PROCEDURE stp_DeleteVisit
    @VisitID int
AS
BEGIN
    DELETE FROM Visits WHERE VisitID = @VisitID;
END
GO


IF OBJECT_ID('stp_AddActivityLog', 'P') IS NOT NULL DROP PROCEDURE stp_AddActivityLog;
GO
CREATE PROCEDURE stp_AddActivityLog
    @UserID int,
    @Action varchar(50),
    @TableAffected varchar(50),
    @RecordID int,
    @Status varchar(20)
AS
BEGIN
    INSERT INTO ActivityLog (UserID, Action, TableAffected, RecordID, Status)
    VALUES (@UserID, @Action, @TableAffected, @RecordID, @Status);
    SELECT SCOPE_IDENTITY() AS NewLogID;
END
GO

IF OBJECT_ID('stp_GetActivityLogs', 'P') IS NOT NULL DROP PROCEDURE stp_GetActivityLogs;
GO
CREATE PROCEDURE stp_GetActivityLogs
AS
BEGIN
    SELECT * FROM ActivityLog;
END
GO

IF OBJECT_ID('stp_DeleteActivityLog', 'P') IS NOT NULL DROP PROCEDURE stp_DeleteActivityLog;
GO
CREATE PROCEDURE stp_DeleteActivityLog
    @LogID int
AS
BEGIN
    DELETE FROM ActivityLog WHERE LogID = @LogID;
END
GO


select * from patients
select * from ActivityLog


select * from users

select * from userroles

select * from visits

select * from doctors

update doctors set firstName = 'dawood' where doctorId = 1