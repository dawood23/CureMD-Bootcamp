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
IF OBJECT_ID('stp_AddPatient', 'P') IS NOT NULL 
    DROP PROCEDURE stp_AddPatient;
GO

CREATE PROCEDURE stp_AddPatient
    @FirstName varchar(100),
    @LastName varchar(100),
    @DOB date = NULL,
    @Gender varchar(10) = NULL,
    @Phone varchar(20) = NULL,
    @Email varchar(150) = NULL,
    @Address varchar(500) = NULL,
    @CNIC varchar(13)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Patients WHERE CNIC = @CNIC)
    BEGIN
        DECLARE @ExistingName varchar(250);
        DECLARE @ErrorMessage varchar(500);

        SELECT TOP 1 @ExistingName = FirstName + ' ' + LastName
        FROM Patients
        WHERE CNIC = @CNIC;

        SET @ErrorMessage = 'A patient with this CNIC already exists: ' + @ExistingName;

        THROW 50001, @ErrorMessage, 1;
        RETURN;
    END

    INSERT INTO Patients (FirstName, LastName, DOB, Gender, Phone, Email, Address, CNIC)
    VALUES (@FirstName, @LastName, @DOB, @Gender, @Phone, @Email, @Address, @CNIC);

    SELECT SCOPE_IDENTITY() AS NewPatientID;
END
GO



select * from Patients

IF OBJECT_ID('stp_GetPatients', 'P') IS NOT NULL DROP PROCEDURE stp_GetPatients;
GO
CREATE PROCEDURE stp_GetPatients
AS
BEGIN
    SELECT * FROM Patients;
END
GO

IF OBJECT_ID('stp_GetPatientsPaged', 'P') IS NOT NULL
    DROP PROCEDURE stp_GetPatientsPaged;
GO

CREATE PROCEDURE stp_GetPatientsPaged
    @PageNumber INT,
    @PageSize INT,
    @Search NVARCHAR(100) = ''
AS
BEGIN
    SET NOCOUNT ON;

    -- Put filtered results into a temp table
    CREATE TABLE #FilteredPatients (
        PatientID INT,
        FirstName NVARCHAR(100),
        LastName NVARCHAR(100),
        DOB DATE NULL,
        Gender NVARCHAR(50) NULL,
        Phone NVARCHAR(50) NULL,
        Email NVARCHAR(100) NULL,
        Address NVARCHAR(200) NULL,
        CreatedAt DATETIME
    );

    INSERT INTO #FilteredPatients
    SELECT PatientID, FirstName, LastName, DOB, Gender, Phone, Email, Address, CreatedAt
    FROM Patients
    WHERE 
        @Search = '' OR
        FirstName LIKE '%' + @Search + '%' OR
        LastName LIKE '%' + @Search + '%' OR
        Email LIKE '%' + @Search + '%' OR
        Phone LIKE '%' + @Search + '%';

    -- Paged data
    SELECT *
    FROM #FilteredPatients
    ORDER BY PatientID
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Total count
    SELECT COUNT(*) AS TotalCount
    FROM #FilteredPatients;

    DROP TABLE #FilteredPatients;
END




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
IF OBJECT_ID('stp_GetAppointments', 'P') IS NOT NULL 
    DROP PROCEDURE stp_GetAppointments;
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

        a.PatientID,
        p.FirstName + ' ' + p.LastName AS PatientName,

        a.DoctorID,
        d.DoctorName,

        a.CreatedBy,
        u.Username AS CreatedByName

    FROM Appointments a
    INNER JOIN Patients p ON a.PatientID = p.PatientID
    INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
    INNER JOIN Users u ON a.CreatedBy = u.UserID

    ORDER BY 
        CASE a.Status
            WHEN 'Scheduled' THEN 1
            WHEN 'Canceled'  THEN 2
            WHEN 'Completed' THEN 3
            ELSE 4
        END,
        a.StartTime;
END
GO


IF OBJECT_ID('stp_getAppointmentsByUser','P') IS NOT NULL DROP PROCEDURE stp_getAppointmentsByUser;
GO
CREATE PROCEDURE stp_getAppointmentsByUser
 @PatientID int
AS 
BEGIN
	SET NOCOUNT ON;
	   SELECT 
        a.AppointmentID,
        a.StartTime,
        a.DurationMinutes,
        a.Status,
        a.CreatedAt,

        a.PatientID,
        p.FirstName + ' ' + p.LastName AS PatientName,

        a.DoctorID,
       d.DoctorName,

        a.CreatedBy,
        u.Username AS CreatedByName

    FROM Appointments a
    INNER JOIN Patients p ON a.PatientID = p.PatientID
    INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
    INNER JOIN Users u ON a.CreatedBy = u.UserID

	where a.PatientID=@PatientID
    ORDER BY a.StartTime;
END
GO

IF OBJECT_ID('stp_getAppointmentsByID','P') IS NOT NULL DROP PROCEDURE stp_getAppointmentsByID;
GO
CREATE PROCEDURE stp_getAppointmentsByID
 @AppointmentID int
AS 
BEGIN
	SET NOCOUNT ON;
	   SELECT 
        a.AppointmentID,
        a.StartTime,
        a.DurationMinutes,
        a.Status,
        a.CreatedAt,

        a.PatientID,
        p.FirstName + ' ' + p.LastName AS PatientName,

        a.DoctorID,
       d.DoctorName,

        a.CreatedBy,
        u.Username AS CreatedByName

    FROM Appointments a
    INNER JOIN Patients p ON a.PatientID = p.PatientID
    INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
    INNER JOIN Users u ON a.CreatedBy = u.UserID

	where a.AppointmentID=@AppointmentID
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

    BEGIN TRY
        BEGIN TRANSACTION;

        IF @DurationMinutes NOT IN (15, 30, 60)
        BEGIN
            RAISERROR('Invalid duration. Only 15, 30, or 60 minutes are allowed.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        DECLARE @BusinessStart time = '09:00';
        DECLARE @BusinessEnd time = '17:00';
        DECLARE @EndTime datetime = DATEADD(MINUTE, @DurationMinutes, @StartTime);

        IF (CAST(@StartTime AS time) < @BusinessStart OR CAST(@EndTime AS time) > @BusinessEnd)
        BEGIN
            RAISERROR('Appointment must be within business hours (09:00 - 17:00).', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        IF EXISTS (
            SELECT 1
            FROM Appointments WITH (UPDLOCK, ROWLOCK)
            WHERE DoctorID = @DoctorID
              AND Status = 'Scheduled'
              AND (
                    (@StartTime < DATEADD(MINUTE, DurationMinutes, StartTime) 
                     AND @EndTime > StartTime)
                  )
        )
        BEGIN
            RAISERROR('Conflict: Overlapping appointment exists for this provider.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        INSERT INTO Appointments (PatientID, DoctorID, CreatedBy, StartTime, DurationMinutes, Status)
        VALUES (@PatientID, @DoctorID, @CreatedBy, @StartTime, @DurationMinutes, @Status);

        COMMIT TRANSACTION;

        SELECT SCOPE_IDENTITY() AS NewAppointmentID;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
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
        FROM Appointments WITH (UPDLOCK, ROWLOCK)
        WHERE DoctorID = @DoctorID
          AND Status = 'Scheduled'
          AND AppointmentID <> @AppointmentID
          AND (
                (@StartTime < DATEADD(MINUTE, DurationMinutes, StartTime) 
                 AND @EndTime > StartTime)
              )
    )
    BEGIN
        RAISERROR('Conflict: Overlapping appointment exists for this provider.', 16, 1);
        RETURN;
    END

    IF @Status = 'Completed'
    BEGIN
        IF GETDATE() < @EndTime
        BEGIN
            RAISERROR('Cannot mark appointment as Completed before it has finished.', 16, 1);
            RETURN;
        END
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



IF OBJECT_ID('stp_GetWeeklyCalendar', 'P') IS NOT NULL DROP PROCEDURE stp_GetWeeklyCalendar;
GO
CREATE PROCEDURE stp_GetWeeklyCalendar
    @DoctorID int,
    @WeekStartDate date
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @WeekEndDate date = DATEADD(DAY, 6, @WeekStartDate);
    
    SELECT 
        a.AppointmentID,
        a.StartTime,
        a.DurationMinutes,
        a.Status,
        a.PatientID,
        p.FirstName + ' ' + p.LastName AS PatientName,
        a.DoctorID,
        d.DoctorName,
        CAST(a.StartTime AS date) AS AppointmentDate,
        CAST(a.StartTime AS time) AS AppointmentTime,
        DATEADD(MINUTE, a.DurationMinutes, a.StartTime) AS EndTime
    FROM Appointments a
    INNER JOIN Patients p ON a.PatientID = p.PatientID
    INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
    WHERE a.DoctorID = @DoctorID
      AND CAST(a.StartTime AS date) BETWEEN @WeekStartDate AND @WeekEndDate
      AND a.Status = 'Scheduled'
    ORDER BY a.StartTime;
END
GO

------------------------------------------------------------
-- 6) VisitNotes
------------------------------------------------------------
IF OBJECT_ID('stp_AddVisitNote', 'P') IS NOT NULL
    DROP PROCEDURE stp_AddVisitNote;
GO

CREATE PROCEDURE stp_AddVisitNote
    @AppointmentID INT,
    @Content VARCHAR(MAX)
AS
BEGIN

    IF NOT EXISTS (
        SELECT 1 FROM Appointments 
        WHERE AppointmentID = @AppointmentID AND Status = 'Completed'
    )
    BEGIN
        RAISERROR('Visit can only be created for a completed appointment.', 16, 1);
        RETURN;
    END;

    IF EXISTS (
        SELECT 1 FROM VisitNotes WHERE AppointmentID = @AppointmentID
    )
    BEGIN
        RAISERROR('A visit already exists for this appointment.', 16, 1);
        RETURN;
    END;

    IF EXISTS (
        SELECT 1 
        FROM Bills 
        WHERE AppointmentID = @AppointmentID AND Status = 'Finalized'
    )
    BEGIN
        RAISERROR('Visit cannot be created once billing is finalized.', 16, 1);
        RETURN;
    END;

    INSERT INTO VisitNotes (AppointmentID, Content)
    VALUES (@AppointmentID, @Content);
END
GO



IF OBJECT_ID('stp_GetVisitNotes', 'P') IS NOT NULL
    DROP PROCEDURE stp_GetVisitNotes;
GO

CREATE PROCEDURE stp_GetVisitNotes
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        v.VisitNoteID,
        v.Content,
        v.CreatedAt ,
        v.UpdatedAt ,

        a.AppointmentID,
        a.StartTime,
        a.DurationMinutes,
        a.Status AS AppointmentStatus,

        p.PatientID,
        p.FirstName + ' ' + p.LastName AS PatientName,

        d.DoctorID,
        d.DoctorName

    FROM VisitNotes v
    INNER JOIN Appointments a ON v.AppointmentID = a.AppointmentID
    INNER JOIN Patients p ON a.PatientID = p.PatientID
    INNER JOIN Doctors d ON a.DoctorID = d.DoctorID

    ORDER BY a.StartTime;
END
GO


IF OBJECT_ID('stp_GetVisitNotesById', 'P') IS NOT NULL
    DROP PROCEDURE stp_GetVisitNotesById;
GO

CREATE PROCEDURE stp_GetVisitNotesbyId
	@AppointmentID int
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        v.VisitNoteID,
        v.Content,
        v.CreatedAt,
        v.UpdatedAt,

        a.AppointmentID,
        a.StartTime,
        a.DurationMinutes,
        a.Status AS AppointmentStatus,

        p.PatientID,
        p.FirstName + ' ' + p.LastName AS PatientName,

        d.DoctorID,
        d.DoctorName

    FROM VisitNotes v
    INNER JOIN Appointments a ON v.AppointmentID = a.AppointmentID
    INNER JOIN Patients p ON a.PatientID = p.PatientID
    INNER JOIN Doctors d ON a.DoctorID = d.DoctorID

	where a.AppointmentID=@AppointmentID
    ORDER BY a.StartTime;
END
GO


IF OBJECT_ID('stp_UpdateVisitNote', 'P') IS NOT NULL
    DROP PROCEDURE stp_UpdateVisitNote;
GO

CREATE PROCEDURE stp_UpdateVisitNote
    @VisitNoteID INT,
    @Content VARCHAR(MAX)
AS
BEGIN

    IF EXISTS (
        SELECT 1 
        FROM VisitNotes v
        INNER JOIN Bills b ON v.AppointmentID = b.AppointmentID
        WHERE v.VisitNoteID = @VisitNoteID AND b.Status = 'Finalized'
    )
    BEGIN
        RAISERROR('Visit cannot be updated once billing is finalized.', 16, 1);
        RETURN;
    END;

    UPDATE VisitNotes
    SET Content = @Content,
        UpdatedAt = GETDATE()
    WHERE VisitNoteID = @VisitNoteID;
END
GO


IF OBJECT_ID('stp_DeleteVisitNote', 'P') IS NOT NULL
    DROP PROCEDURE stp_DeleteVisitNote;
GO

CREATE PROCEDURE stp_DeleteVisitNote
    @VisitNoteID INT
AS
BEGIN

    IF EXISTS (
        SELECT 1 
        FROM VisitNotes v
        INNER JOIN Bills b ON v.AppointmentID = b.AppointmentID
        WHERE v.VisitNoteID = @VisitNoteID AND b.Status = 'Finalized'
    )
    BEGIN
        RAISERROR('Visit cannot be deleted once billing is finalized.', 16, 1);
        RETURN;
    END;

    DELETE FROM VisitNotes
    WHERE VisitNoteID = @VisitNoteID;
END
GO



------------------------------------------------------------
-- 7) Bills
------------------------------------------------------------

IF OBJECT_ID('stp_GenerateBill', 'P') IS NOT NULL 
    DROP PROCEDURE stp_GenerateBill;
GO

CREATE PROCEDURE stp_GenerateBill
    @AppointmentID INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1 
        FROM Appointments 
        WHERE AppointmentID = @AppointmentID 
          AND Status = 'Completed'
    )
    BEGIN
        RAISERROR('Bill can only be generated if the appointment is completed.', 16, 1);
        RETURN;
    END;

    IF EXISTS (SELECT 1 FROM Bills WHERE AppointmentID = @AppointmentID)
    BEGIN
        SELECT 
            b.BillID,
            b.AppointmentID,
            b.PatientID,
            p.FirstName + ' ' + p.LastName AS PatientName,
            d.DoctorName,
            b.GeneratedAt,
            b.Status,
            b.TotalAmount,
            b.PendingAmount
        FROM Bills b
        INNER JOIN Patients p ON b.PatientID = p.PatientID
        INNER JOIN Appointments a ON b.AppointmentID = a.AppointmentID
        INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
        WHERE b.AppointmentID = @AppointmentID;
        RETURN;
    END;

    DECLARE @PatientID INT, @DoctorID INT, @Duration INT, 
            @Rate DECIMAL(10,2), @Total DECIMAL(10,2);

    SELECT 
        @PatientID = a.PatientID,
        @DoctorID = a.DoctorID,
        @Duration = a.DurationMinutes
    FROM Appointments a
    WHERE a.AppointmentID = @AppointmentID;

    SELECT TOP 1 @Rate = RatePerMinute
    FROM DoctorRates
    WHERE DoctorID = @DoctorID
      AND EffectiveFrom <= CAST(GETDATE() AS date)
    ORDER BY EffectiveFrom DESC;

    SET @Total = @Duration * @Rate;

    INSERT INTO Bills (AppointmentID, PatientID, TotalAmount, PendingAmount)
    VALUES (@AppointmentID, @PatientID, @Total, @Total);

    SELECT 
        b.BillID,
        b.AppointmentID,
        b.PatientID,
        p.FirstName + ' ' + p.LastName AS PatientName,
        d.DoctorName,
        b.GeneratedAt,
        b.Status,
        b.TotalAmount,
        b.PendingAmount
    FROM Bills b
    INNER JOIN Patients p ON b.PatientID = p.PatientID
    INNER JOIN Appointments a ON b.AppointmentID = a.AppointmentID
    INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
    WHERE b.AppointmentID = @AppointmentID;
END
GO


IF OBJECT_ID('stp_GetBills', 'P') IS NOT NULL
    DROP PROCEDURE stp_GetBills;
GO

CREATE PROCEDURE stp_GetBills
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        b.BillID,
        b.AppointmentID,
        b.PatientID,
        (p.FirstName + ' ' + p.LastName) AS PatientName,
        d.DoctorName,
        b.GeneratedAt,
        b.Status,
        b.TotalAmount,
        b.PendingAmount
    FROM Bills b
    INNER JOIN Patients p ON b.PatientID = p.PatientID
    INNER JOIN Appointments a ON b.AppointmentID = a.AppointmentID
    INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
    ORDER BY 
        CASE b.Status 
            WHEN 'Open' THEN 1
            WHEN 'Partial' THEN 2
            WHEN 'Paid' THEN 3
            ELSE 4
        END,
        b.GeneratedAt DESC; 
END
GO



IF OBJECT_ID('stp_GetBillByID', 'P') IS NOT NULL
    DROP PROCEDURE stp_GetBillByID;
GO

CREATE PROCEDURE stp_GetBillByID
	@BillID int
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        b.BillID,
        b.AppointmentID,
        b.PatientID,
        (p.FirstName + ' ' + p.LastName) AS PatientName,
        d.DoctorName,
        b.GeneratedAt,
        b.Status,
        b.TotalAmount,
        b.PendingAmount
    FROM Bills b
    INNER JOIN Patients p ON b.PatientID = p.PatientID
    INNER JOIN Appointments a ON b.AppointmentID = a.AppointmentID
    INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
	WHERE b.BillID=@BillID
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
IF OBJECT_ID('stp_AddPayment', 'P') IS NOT NULL 
    DROP PROCEDURE stp_AddPayment;
GO

CREATE PROCEDURE stp_AddPayment
    @BillID int,
    @Amount decimal(10,2),
    @Method varchar(50),
    @RecordedBy int
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON; 

    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @PendingAmount decimal(10,2);

        SELECT @PendingAmount = PendingAmount
        FROM Bills WITH (UPDLOCK, ROWLOCK)
        WHERE BillID = @BillID;

        IF @PendingAmount < @Amount
        BEGIN
            THROW 50002, 'Payment amount exceeds pending amount.', 1;
        END

        INSERT INTO Payments (BillID, Amount, Method, RecordedBy)
        VALUES (@BillID, @Amount, @Method, @RecordedBy);

        UPDATE Bills
        SET PendingAmount = PendingAmount - @Amount
        WHERE BillID = @BillID;

        COMMIT TRANSACTION;

        SELECT SCOPE_IDENTITY() AS NewPaymentID;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END
GO


IF OBJECT_ID('stp_GetPayments', 'P') IS NOT NULL
    DROP PROCEDURE stp_GetPayments;
GO

CREATE PROCEDURE stp_GetPayments
AS
BEGIN
    SELECT
        p.PaymentID,
        p.BillID,
        b.AppointmentID,
        a.DoctorID,
        d.DoctorName,
        a.PatientID,
        CONCAT(pt.FirstName, ' ', pt.LastName) AS PatientName,
        p.Amount,
        p.Method,
        p.PaidAt,
        p.RecordedBy
    FROM Payments p
    INNER JOIN Bills b ON p.BillID = b.BillID
    INNER JOIN Appointments a ON b.AppointmentID = a.AppointmentID
    INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
    INNER JOIN Patients pt ON a.PatientID = pt.PatientID
    ORDER BY p.PaidAt DESC;
END
GO


IF OBJECT_ID('stp_GetPaymentByID', 'P') IS NOT NULL
    DROP PROCEDURE stp_GetPaymentByID;
GO

CREATE PROCEDURE stp_GetPaymentByID
@PaymentID int
AS
BEGIN
    SELECT
        p.PaymentID,
        p.BillID,
        b.AppointmentID,
        a.DoctorID,
        d.DoctorName,
        a.PatientID,
        CONCAT(pt.FirstName, ' ', pt.LastName) AS PatientName,
        p.Amount,
        p.Method,
        p.PaidAt,
        p.RecordedBy
    FROM Payments p
    INNER JOIN Bills b ON p.BillID = b.BillID
    INNER JOIN Appointments a ON b.AppointmentID = a.AppointmentID
    INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
    INNER JOIN Patients pt ON a.PatientID = pt.PatientID
    Where p.PaymentID=@PaymentID
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
    @Status varchar(20),
	@Description varchar(100)
AS
BEGIN
    INSERT INTO Logs (UserID, Action, TableAffected,  Status,Description)
    VALUES (@UserID, @Action, @TableAffected, @Status,@Description);
    SELECT SCOPE_IDENTITY() AS NewLogID;
END
GO

IF OBJECT_ID('stp_GetLogs', 'P') IS NOT NULL DROP PROCEDURE stp_GetLogs;
GO.
CREATE PROCEDURE stp_GetLogs
AS
BEGIN
    SELECT * FROM Logs;
END
GO

IF OBJECT_ID('stp_RecordPayment', 'P') IS NOT NULL
    DROP PROCEDURE stp_RecordPayment;
GO

CREATE PROCEDURE stp_RecordPayment
    @BillID INT,
    @Amount DECIMAL(10,2),
    @Method VARCHAR(50),
    @RecordedBy INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Amount <= 0
    BEGIN
        RAISERROR('Payment amount must be greater than zero.', 16, 1);
        RETURN;
    END

    DECLARE @Pending DECIMAL(10,2);
    DECLARE @NewPending DECIMAL(10,2);
    DECLARE @BillExists INT;

    SELECT @BillExists = COUNT(1) 
    FROM Bills
    WHERE BillID = @BillID;

    IF @BillExists = 0
    BEGIN
        RAISERROR('Bill not found.', 16, 1);
        RETURN;
    END

    SELECT @Pending = PendingAmount
    FROM Bills
    WHERE BillID = @BillID;

    IF @Amount > @Pending
    BEGIN
        DECLARE @PendingStr VARCHAR(20);
        SET @PendingStr = CAST(@Pending AS VARCHAR(20));

        RAISERROR('Payment amount exceeds outstanding balance. Outstanding: %s', 16, 1, @PendingStr);
        RETURN;
    END

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Payments (BillID, Amount, Method, RecordedBy)
        VALUES (@BillID, @Amount, @Method, @RecordedBy);

        DECLARE @NewPaymentID INT = SCOPE_IDENTITY();

        SET @NewPending = @Pending - @Amount;

        UPDATE b
        SET PendingAmount = @NewPending,
            Status = CASE 
                       WHEN @NewPending <= 0 THEN 'Paid'
                       WHEN @NewPending < b.TotalAmount THEN 'Partial'
                       ELSE 'Open'
                     END
        FROM Bills b
        WHERE b.BillID = @BillID;

        INSERT INTO logs (UserID, Action, TableAffected, Status)
        VALUES (@RecordedBy, 'RecordPayment', 'Payments', 'Success');
		
        COMMIT TRANSACTION;

        SELECT 
            @NewPaymentID AS PaymentID,
            @NewPending AS RemainingPending,
            CASE 
                WHEN @NewPending <= 0 THEN 'Paid'
                WHEN @NewPending < b.TotalAmount THEN 'Partial'
                ELSE 'Open'
            END AS BillStatus
        FROM Bills b
        WHERE b.BillID = @BillID;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR('Error recording payment: %s', 16, 1, @ErrMsg);
        RETURN;
    END CATCH
END
GO



select * from Roles

select * from Users

select * from logs

select * from Patients
select * from Appointments

select * from Bills


update Patients
set FirstName='Najeed',LastName='Mubasher',email='najeebmubasher31@gmail.com' where PatientID=23
