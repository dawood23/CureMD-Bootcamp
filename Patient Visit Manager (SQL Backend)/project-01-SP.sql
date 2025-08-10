-- project-01-SP.sql
-- 8th August, 2025
-- Author: Dawood Nadeem 6601
-- Description: Stored Procedures for Patient Visit Manager database


create procedure stp_AddUserRole
    @RoleName varchar(50),
    @Description varchar(255) = null
as
begin
    set nocount on;
    begin try
        insert into UserRoles (RoleName, Description)
        values (@RoleName, @Description);
        select SCOPE_IDENTITY() as NewRoleID;
    end try
    begin catch
        throw;
    end catch
end
go

create procedure stp_GetUserRoles
as
begin
    set nocount on;
    select * from UserRoles;
end
go

create procedure stp_UpdateUserRole
    @RoleID int,
    @RoleName varchar(50),
    @Description varchar(255)
as
begin
    set nocount on;
    update UserRoles
    set RoleName = @RoleName, Description = @Description
    where RoleID = @RoleID;
end
go

-- delete
create procedure stp_DeleteUserRole
    @RoleID int
as
begin
    set nocount on;
    delete from UserRoles where RoleID = @RoleID;
end
go


create procedure stp_AddVisitType
    @TypeName varchar(50),
    @BaseFee decimal(10,2),
    @EstimatedDuration int
as
begin
    set nocount on;
    insert into VisitTypes (TypeName, BaseFee, EstimatedDuration)
    values (@TypeName, @BaseFee, @EstimatedDuration);
    select SCOPE_IDENTITY() as NewVisitTypeID;
end
go

create procedure stp_GetVisitTypes
as
begin
    set nocount on;
    select * from VisitTypes;
end
go

create procedure stp_UpdateVisitType
    @VisitTypeID int,
    @TypeName varchar(50),
    @BaseFee decimal(10,2),
    @EstimatedDuration int
as
begin
    set nocount on;
    update VisitTypes
    set TypeName = @TypeName, BaseFee = @BaseFee, EstimatedDuration = @EstimatedDuration
    where VisitTypeID = @VisitTypeID;
end
go

create procedure stp_DeleteVisitType
    @VisitTypeID int
as
begin
    set nocount on;
    delete from VisitTypes where VisitTypeID = @VisitTypeID;
end
go


create procedure stp_AddPatient
    @FirstName varchar(100),
    @LastName varchar(100),
    @DateOfBirth date = null,
    @PhoneNumber varchar(15) = null,
    @Email varchar(255) = null,
    @Address varchar(500) = null,
    @EmergencyContact varchar(255) = null
as
begin
    set nocount on;
    insert into Patients (FirstName, LastName, DateOfBirth, PhoneNumber, Email, Address, EmergencyContact)
    values (@FirstName, @LastName, @DateOfBirth, @PhoneNumber, @Email, @Address, @EmergencyContact);
    select SCOPE_IDENTITY() as NewPatientID;
end
go

create procedure stp_GetPatients
as
begin
    set nocount on;
    select * from Patients;
end
go

create procedure stp_UpdatePatient
    @PatientID int,
    @FirstName varchar(100),
    @LastName varchar(100),
    @DateOfBirth date = null,
    @PhoneNumber varchar(15) = null,
    @Email varchar(255) = null,
    @Address varchar(500) = null,
    @EmergencyContact varchar(255) = null
as
begin
    set nocount on;
    update Patients
    set FirstName = @FirstName,
        LastName = @LastName,
        DateOfBirth = @DateOfBirth,
        PhoneNumber = @PhoneNumber,
        Email = @Email,
        Address = @Address,
        EmergencyContact = @EmergencyContact
    where PatientID = @PatientID;
end
go

create procedure stp_DeletePatient
    @PatientID int
as
begin
    set nocount on;
    delete from Patients where PatientID = @PatientID;
end
go


create procedure stp_AddDoctor
    @FirstName varchar(100),
    @LastName varchar(100),
    @PhoneNumber varchar(15) = null,
    @Email varchar(255) = null
as
begin
    set nocount on;
    insert into Doctors (FirstName, LastName, PhoneNumber, Email)
    values (@FirstName, @LastName, @PhoneNumber, @Email);
    select SCOPE_IDENTITY() as NewDoctorID;
end
go

create procedure stp_GetDoctors
as
begin
    set nocount on;
    select * from Doctors;
end
go

create procedure stp_UpdateDoctor
    @DoctorID int,
    @FirstName varchar(100),
    @LastName varchar(100),
    @PhoneNumber varchar(15) = null,
    @Email varchar(255) = null
as
begin
    set nocount on;
    update Doctors
    set FirstName = @FirstName,
        LastName = @LastName,
        PhoneNumber = @PhoneNumber,
        Email = @Email
    where DoctorID = @DoctorID;
end
go

create procedure stp_DeleteDoctor
    @DoctorID int
as
begin
    set nocount on;
    delete from Doctors where DoctorID = @DoctorID;
end
go


create procedure stp_AddUser
    @Username varchar(50),
    @PasswordHash varchar(255),
    @RoleID int,
    @FirstName varchar(100),
    @LastName varchar(100)
as
begin
    set nocount on;
    insert into Users (Username, PasswordHash, RoleID, FirstName, LastName)
    values (@Username, @PasswordHash, @RoleID, @FirstName, @LastName);
    select SCOPE_IDENTITY() as NewUserID;
end
go

create procedure stp_GetUsers
as
begin
    set nocount on;
    select * from Users;
end
go

create procedure stp_UpdateUser
    @UserID int,
    @Username varchar(50),
    @PasswordHash varchar(255),
    @RoleID int,
    @FirstName varchar(100),
    @LastName varchar(100)
as
begin
    set nocount on;
    update Users
    set Username = @Username,
        PasswordHash = @PasswordHash,
        RoleID = @RoleID,
        FirstName = @FirstName,
        LastName = @LastName
    where UserID = @UserID;
end
go

create procedure stp_DeleteUser
    @UserID int
as
begin
    set nocount on;
    delete from Users where UserID = @UserID;
end
go


create procedure stp_AddVisit
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
as
begin
    set nocount on;
    insert into Visits (PatientID, DoctorID, VisitTypeID, VisitDate, VisitTime, Description, Notes, Status, Fee, CreatedBy)
    values (@PatientID, @DoctorID, @VisitTypeID, @VisitDate, @VisitTime, @Description, @Notes, @Status, @Fee, @CreatedBy);
    select SCOPE_IDENTITY() as NewVisitID;
end
go

create procedure stp_GetVisits
as
begin
    set nocount on;
    select * from Visits;
end
go

create procedure stp_UpdateVisit
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
as
begin
    set nocount on;
    update Visits
    set PatientID = @PatientID,
        DoctorID = @DoctorID,
        VisitTypeID = @VisitTypeID,
        VisitDate = @VisitDate,
        VisitTime = @VisitTime,
        Description = @Description,
        Notes = @Notes,
        Status = @Status,
        Fee = @Fee,
        CreatedBy = @CreatedBy
    where VisitID = @VisitID;
end
go

create procedure stp_DeleteVisit
    @VisitID int
as
begin
    set nocount on;
    delete from Visits where VisitID = @VisitID;
end
go


create procedure stp_AddActivityLog
    @UserID int,
    @Action varchar(50),
    @TableAffected varchar(50),
    @RecordID int,
    @Status varchar(20)
as
begin
    set nocount on;
    insert into ActivityLog (UserID, Action, TableAffected, RecordID, Status)
    values (@UserID, @Action, @TableAffected, @RecordID, @Status);
    select SCOPE_IDENTITY() as NewLogID;
end
go

create procedure stp_GetActivityLogs
as
begin
    set nocount on;
    select * from ActivityLog;
end
go

create procedure stp_DeleteActivityLog
    @LogID int
as
begin
    set nocount on;
    delete from ActivityLog where LogID = @LogID;
end
go