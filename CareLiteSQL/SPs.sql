-- project-02-SP-safe.sql
-- 4th September, 2025
-- Dawood Nadeem 6601
-- Multi-execution safe stored procedures for Patient Visit Manager database

------------------------------------------------------------
-- 1) Roles
------------------------------------------------------------

use dawood_nadeem_carelite
IF OBJECT_ID('stp_AddRole', 'P') IS NOT NULL DROP PROCEDURE stp_AddRole;
GO
CREATE PROCEDURE stp_AddRole
    @Name varchar(50),
    @Description varchar(255) = NULL
AS
BEGIN
    INSERT INTO Roles (Name, Description)
    VALUES (@Name, @Description);
    SELECT SCOPE_IDENTITY() AS NewRoleID;
END
GO

IF OBJECT_ID('stp_GetRoles', 'P') IS NOT NULL DROP PROCEDURE stp_GetRoles;
GO
CREATE PROCEDURE stp_GetRoles
AS
BEGIN
    SELECT * FROM Roles;
END
GO

IF OBJECT_ID('stp_GetRoleById', 'P') IS NOT NULL DROP PROCEDURE stp_GetRoleById;
GO
CREATE PROCEDURE stp_GetRoleById
    @RoleID int
AS
BEGIN
    SELECT * FROM Roles WHERE RoleID = @RoleID;
END
GO

IF OBJECT_ID('stp_UpdateRole', 'P') IS NOT NULL DROP PROCEDURE stp_UpdateRole;
GO
CREATE PROCEDURE stp_UpdateRole
    @RoleID int,
    @Name varchar(50),
    @Description varchar(255) = NULL
AS
BEGIN
    UPDATE Roles SET Name = @Name, Description = @Description
    WHERE RoleID = @RoleID;
END
GO

IF OBJECT_ID('stp_DeleteRole', 'P') IS NOT NULL DROP PROCEDURE stp_DeleteRole;
GO
CREATE PROCEDURE stp_DeleteRole
    @RoleID int
AS
BEGIN
    DELETE FROM Roles WHERE RoleID = @RoleID;
END
GO

select * from Users
------------------------------------------------------------
-- 2) Users
------------------------------------------------------------
IF OBJECT_ID('stp_AddUser', 'P') IS NOT NULL DROP PROCEDURE stp_AddUser;
GO
CREATE PROCEDURE stp_AddUser
    @Username varchar(100),
    @PasswordHash varchar(255),
    @Email varchar(150) = NULL,
    @Phone varchar(20) = NULL,
    @RoleID int,
    @Active bit = 1
AS
BEGIN
    INSERT INTO Users (Username, PasswordHash, Email, Phone, RoleID, Active)
    VALUES (@Username, @PasswordHash, @Email, @Phone, @RoleID, @Active);
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
    @Username varchar(100)
AS
BEGIN
    SELECT * FROM Users WHERE Username = @Username;
END
GO

IF OBJECT_ID('stp_UpdateUser', 'P') IS NOT NULL DROP PROCEDURE stp_UpdateUser;
GO
CREATE PROCEDURE stp_UpdateUser
    @UserID int,
    @Username varchar(100),
    @PasswordHash varchar(255),
    @Email varchar(150) = NULL,
    @Phone varchar(20) = NULL,
    @RoleID int,
    @Active bit
AS
BEGIN
    UPDATE Users
    SET Username = @Username,
        PasswordHash = @PasswordHash,
        Email = @Email,
        Phone = @Phone,
        RoleID = @RoleID,
        Active = @Active
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


------------------------------------------------------------
-- 3) Patients
------------------------------------------------------------
IF OBJECT_ID('stp_AddPatient', 'P') IS NOT NULL DROP PROCEDURE stp_AddPatient;
GO
CREATE PROCEDURE stp_AddPatient
    @FirstName varchar(100),
    @LastName varchar(100),
    @DOB date = NULL,
    @Gender varchar(10) = NULL,
    @Phone varchar(20) = NULL,
    @Email varchar(150) = NULL,
    @Address varchar(500) = NULL
AS
BEGIN
    INSERT INTO Patients (FirstName, LastName, DOB, Gender, Phone, Email, Address)
    VALUES (@FirstName, @LastName, @DOB, @Gender, @Phone, @Email, @Address);
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
    @DOB date = NULL,
    @Gender varchar(10) = NULL,
    @Phone varchar(20) = NULL,
    @Email varchar(150) = NULL,
    @Address varchar(500) = NULL
AS
BEGIN
    UPDATE Patients
    SET FirstName = @FirstName,
        LastName = @LastName,
        DOB = @DOB,
        Gender = @Gender,
        Phone = @Phone,
        Email = @Email,
        Address = @Address
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

------------------------------------------------------------
-- 4) Doctors
------------------------------------------------------------
-- Drop and recreate AddDoctor procedure
IF OBJECT_ID('stp_AddDoctor', 'P') IS NOT NULL DROP PROCEDURE stp_AddDoctor;
GO
CREATE PROCEDURE stp_AddDoctor
    @DoctorName varchar(50),
    @Specialization varchar(100) = NULL
AS
BEGIN
    INSERT INTO Doctors (DoctorName, Specialization)
    VALUES (@DoctorName, @Specialization);
    SELECT SCOPE_IDENTITY() AS NewDoctorID;
END
GO

-- Drop and recreate GetDoctors procedure
IF OBJECT_ID('stp_GetDoctors', 'P') IS NOT NULL DROP PROCEDURE stp_GetDoctors;
GO
CREATE PROCEDURE stp_GetDoctors
AS
BEGIN
    SELECT DoctorID, DoctorName, Specialization
    FROM Doctors;
END
GO

-- Drop and recreate UpdateDoctor procedure
IF OBJECT_ID('stp_UpdateDoctor', 'P') IS NOT NULL DROP PROCEDURE stp_UpdateDoctor;
GO
CREATE PROCEDURE stp_UpdateDoctor
    @DoctorID int,
    @DoctorName varchar(50) = NULL,
    @Specialization varchar(100) = NULL
AS
BEGIN
    UPDATE Doctors
    SET 
        DoctorName = COALESCE(@DoctorName, DoctorName),
        Specialization = COALESCE(@Specialization, Specialization)
    WHERE DoctorID = @DoctorID;
END
GO

-- Drop and recreate DeleteDoctor procedure
IF OBJECT_ID('stp_DeleteDoctor', 'P') IS NOT NULL DROP PROCEDURE stp_DeleteDoctor;
GO
CREATE PROCEDURE stp_DeleteDoctor
    @DoctorID int
AS
BEGIN
    DELETE FROM Doctors WHERE DoctorID = @DoctorID;
END
GO

------------------------------------------------------------
-- 5) Appointments
------------------------------------------------------------
IF OBJECT_ID('stp_GetAppointments', 'P') IS NOT NULL DROP PROCEDURE stp_GetAppointments;
GO
CREATE PROCEDURE stp_GetAppointments
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        a.AppointmentID,
        a.StartTime,
        a.DurationMinutes,
        a.Status,
        a.CreatedAt,

        -- Patient info
        a.PatientID,
        p.FirstName + ' ' + p.LastName AS PatientName,

        -- Doctor info
        a.DoctorID,
       d.DoctorName,

        -- User who created
        a.CreatedBy,
        u.Username AS CreatedByName

    FROM Appointments a
    INNER JOIN Patients p ON a.PatientID = p.PatientID
    INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
    INNER JOIN Users u ON a.CreatedBy = u.UserID

    ORDER BY a.StartTime;
END
GO

IF OBJECT_ID('stp_AddAppointment', 'P') IS NOT NULL DROP PROCEDURE stp_AddAppointment;
GO
CREATE PROCEDURE stp_AddAppointment
    @PatientID int,
    @DoctorID int,
    @CreatedBy int,
    @StartTime datetime,
    @DurationMinutes int,
    @Status varchar(20) = 'Scheduled'
AS
BEGIN
    SET NOCOUNT ON;

    IF @DurationMinutes NOT IN (15, 30, 60)
    BEGIN
        RAISERROR('Invalid duration. Only 15, 30, or 60 minutes are allowed.', 16, 1);
        RETURN;
    END

    DECLARE @BusinessStart time = '09:00';
    DECLARE @BusinessEnd time = '17:00';
    DECLARE @EndTime datetime = DATEADD(MINUTE, @DurationMinutes, @StartTime);

    IF (CAST(@StartTime AS time) < @BusinessStart OR CAST(@EndTime AS time) > @BusinessEnd)
    BEGIN
        RAISERROR('Appointment must be within business hours (09:00 - 17:00).', 16, 1);
        RETURN;
    END

    IF EXISTS (
        SELECT 1
        FROM Appointments
        WHERE DoctorID = @DoctorID
          AND Status = 'Scheduled'
          AND (
                (@StartTime < DATEADD(MINUTE, DurationMinutes, StartTime) AND @EndTime > StartTime)
              )
    )
    BEGIN
        RAISERROR('Conflict: Overlapping appointment exists for this provider.', 16, 1);
        RETURN;
    END

    INSERT INTO Appointments (PatientID, DoctorID, CreatedBy, StartTime, DurationMinutes, Status)
    VALUES (@PatientID, @DoctorID, @CreatedBy, @StartTime, @DurationMinutes, @Status);

    SELECT SCOPE_IDENTITY() AS NewAppointmentID;
END
GO


IF OBJECT_ID('stp_UpdateAppointment', 'P') IS NOT NULL DROP PROCEDURE stp_UpdateAppointment;
GO
CREATE PROCEDURE stp_UpdateAppointment
    @AppointmentID int,
    @PatientID int,
    @DoctorID int,
    @StartTime datetime,
    @DurationMinutes int,
    @Status varchar(20)
AS
BEGIN

    IF @DurationMinutes NOT IN (15, 30, 60)
    BEGIN
        RAISERROR('Invalid duration. Only 15, 30, or 60 minutes are allowed.', 16, 1);
        RETURN;
    END

    DECLARE @BusinessStart time = '09:00';
    DECLARE @BusinessEnd time = '17:00';
    DECLARE @EndTime datetime = DATEADD(MINUTE, @DurationMinutes, @StartTime);

    IF (CAST(@StartTime AS time) < @BusinessStart OR CAST(@EndTime AS time) > @BusinessEnd)
    BEGIN
        RAISERROR('Appointment must be within business hours (09:00 - 17:00).', 16, 1);
        RETURN;
    END

    IF EXISTS (
        SELECT 1
        FROM Appointments
        WHERE DoctorID = @DoctorID
          AND Status = 'Scheduled'
          AND AppointmentID <> @AppointmentID
          AND (
                (@StartTime < DATEADD(MINUTE, DurationMinutes, StartTime) AND @EndTime > StartTime)
              )
    )
    BEGIN
        RAISERROR('Conflict: Overlapping appointment exists for this provider.', 16, 1);
        RETURN;
    END


    UPDATE Appointments
    SET PatientID = @PatientID,
        DoctorID = @DoctorID,
        StartTime = @StartTime,
        DurationMinutes = @DurationMinutes,
        Status = @Status
    WHERE AppointmentID = @AppointmentID;
END
GO


------------------------------------------------------------
-- 6) VisitNotes
------------------------------------------------------------
IF OBJECT_ID('stp_AddVisitNote', 'P') IS NOT NULL DROP PROCEDURE stp_AddVisitNote;
GO
CREATE PROCEDURE stp_AddVisitNote
    @AppointmentID int,
    @DoctorID int,
    @Content varchar(max)
AS
BEGIN
    INSERT INTO VisitNotes (AppointmentID, DoctorID, Content)
    VALUES (@AppointmentID, @DoctorID, @Content);
    SELECT SCOPE_IDENTITY() AS NewVisitNoteID;
END
GO

IF OBJECT_ID('stp_GetVisitNotes', 'P') IS NOT NULL DROP PROCEDURE stp_GetVisitNotes;
GO
CREATE PROCEDURE stp_GetVisitNotes
AS
BEGIN
    SELECT * FROM VisitNotes;
END
GO

IF OBJECT_ID('stp_UpdateVisitNote', 'P') IS NOT NULL DROP PROCEDURE stp_UpdateVisitNote;
GO
CREATE PROCEDURE stp_UpdateVisitNote
    @VisitNoteID int,
    @Content varchar(max)
AS
BEGIN
    UPDATE VisitNotes
    SET Content = @Content,
        UpdatedAt = GETDATE()
    WHERE VisitNoteID = @VisitNoteID;
END
GO

IF OBJECT_ID('stp_DeleteVisitNote', 'P') IS NOT NULL DROP PROCEDURE stp_DeleteVisitNote;
GO
CREATE PROCEDURE stp_DeleteVisitNote
    @VisitNoteID int
AS
BEGIN
    DELETE FROM VisitNotes WHERE VisitNoteID = @VisitNoteID;
END
GO


------------------------------------------------------------
-- 7) Bills
------------------------------------------------------------
IF OBJECT_ID('stp_AddBill', 'P') IS NOT NULL DROP PROCEDURE stp_AddBill;
GO
CREATE PROCEDURE stp_AddBill
    @AppointmentID int,
    @PatientID int,
    @TotalAmount decimal(10,2)
AS
BEGIN
    INSERT INTO Bills (AppointmentID, PatientID, TotalAmount, PendingAmount)
    VALUES (@AppointmentID, @PatientID, @TotalAmount, @TotalAmount);
    SELECT SCOPE_IDENTITY() AS NewBillID;
END
GO

IF OBJECT_ID('stp_GetBills', 'P') IS NOT NULL DROP PROCEDURE stp_GetBills;
GO
CREATE PROCEDURE stp_GetBills
AS
BEGIN
    SELECT * FROM Bills;
END
GO

IF OBJECT_ID('stp_UpdateBillStatus', 'P') IS NOT NULL DROP PROCEDURE stp_UpdateBillStatus;
GO
CREATE PROCEDURE stp_UpdateBillStatus
    @BillID int,
    @Status varchar(20)
AS
BEGIN
    UPDATE Bills SET Status = @Status WHERE BillID = @BillID;
END
GO


------------------------------------------------------------
-- 8) Payments
------------------------------------------------------------
IF OBJECT_ID('stp_AddPayment', 'P') IS NOT NULL DROP PROCEDURE stp_AddPayment;
GO
CREATE PROCEDURE stp_AddPayment
    @BillID int,
    @Amount decimal(10,2),
    @Method varchar(50),
    @RecordedBy int
AS
BEGIN
    INSERT INTO Payments (BillID, Amount, Method, RecordedBy)
    VALUES (@BillID, @Amount, @Method, @RecordedBy);

    -- Update pending amount
    UPDATE Bills
    SET PendingAmount = PendingAmount - @Amount
    WHERE BillID = @BillID;

    SELECT SCOPE_IDENTITY() AS NewPaymentID;
END
GO

IF OBJECT_ID('stp_GetPayments', 'P') IS NOT NULL DROP PROCEDURE stp_GetPayments;
GO
CREATE PROCEDURE stp_GetPayments
AS
BEGIN
    SELECT * FROM Payments;
END
GO


------------------------------------------------------------
-- 9) Logs
------------------------------------------------------------
IF OBJECT_ID('stp_AddLog', 'P') IS NOT NULL DROP PROCEDURE stp_AddLog;
GO
CREATE PROCEDURE stp_AddLog
    @UserID int,
    @Action varchar(50),
    @TableAffected varchar(50),
    @RecordID int,
    @Status varchar(20)
AS
BEGIN
    INSERT INTO Logs (UserID, Action, TableAffected, RecordID, Status)
    VALUES (@UserID, @Action, @TableAffected, @RecordID, @Status);
    SELECT SCOPE_IDENTITY() AS NewLogID;
END
GO

IF OBJECT_ID('stp_GetLogs', 'P') IS NOT NULL DROP PROCEDURE stp_GetLogs;
GO
CREATE PROCEDURE stp_GetLogs
AS
BEGIN
    SELECT * FROM Logs;
END
GO


update Users 
set PasswordHash='A6xnQhbz4Vx2HuGl4lXwZ5U2I8izjLRFnhP5eNfIRvQ=' where UserID=1
