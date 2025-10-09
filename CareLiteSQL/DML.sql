------------------------------------------------------------
-- 1) Insert Roles
------------------------------------------------------------
use dawood_nadeem_carelite

INSERT INTO Roles (Name, Description) VALUES 
('Admin', 'System administrator with full access'),
('Staff', 'Front desk staff for registration and scheduling'),
('Clinician', 'Healthcare provider for clinical documentation'),
('Receptionist', 'Reception and basic patient management');

------------------------------------------------------------
-- 2) Insert Users
------------------------------------------------------------
INSERT INTO Users (Username, PasswordHash, Email, Phone, Active, RoleID) VALUES 
('admin', 'hashed_admin_password', 'admin@carelite.com', '+92-300-1234567', 1, 1),
('staff1', 'hashed_staff1_password', 'staff1@carelite.com', '+92-300-2345678', 1, 2),
('staff2', 'hashed_staff2_password', 'staff2@carelite.com', '+92-300-3456789', 1, 2),
('dr.ahmed', 'hashed_doctor1_password', 'ahmed@carelite.com', '+92-300-4567890', 1, 3),
('dr.sara', 'hashed_doctor2_password', 'sara@carelite.com', '+92-300-5678901', 1, 3),
('receptionist', 'hashed_receptionist_password', 'reception@carelite.com', '+92-300-6789012', 1, 4);

------------------------------------------------------------
-- 3) Insert Patients
------------------------------------------------------------
INSERT INTO Patients (FirstName, LastName, DOB, Gender, Phone, Email, Address) VALUES 
('Muhammad', 'Ali', '1985-03-15', 'Male', '+92-301-1111111', 'mali@email.com', 'House 123, Block A, Gulberg III, Lahore'),
('Fatima', 'Khan', '1990-07-22', 'Female', '+92-301-2222222', 'fkhan@email.com', 'Flat 45, DHA Phase 5, Lahore'),
('Ahmed', 'Hassan', '1982-11-08', 'Male', '+92-301-3333333', 'ahassan@email.com', 'Street 15, Model Town, Lahore'),
('Aisha', 'Malik', '1995-05-12', 'Female', '+92-301-4444444', 'amalik@email.com', 'House 67, Johar Town, Lahore'),
('Omar', 'Sheikh', '1978-09-25', 'Male', '+92-301-5555555', 'osheikh@email.com', 'Block C, Garden Town, Lahore'),
('Zainab', 'Ahmad', '1988-01-18', 'Female', '+92-301-6666666', 'zahmad@email.com', 'House 234, Wapda Town, Lahore'),
('Hassan', 'Raza', '1992-04-30', 'Male', '+92-301-7777777', 'hraza@email.com', 'Street 8, Faisal Town, Lahore'),
('Maryam', 'Siddique', '1987-12-03', 'Female', '+92-301-8888888', 'msiddique@email.com', 'Block D, Punjab Society, Lahore');

------------------------------------------------------------
-- 4) Insert Doctors
------------------------------------------------------------
INSERT INTO Doctors (DoctorName, Specialization) VALUES 
('Dr. Ahmed', 'General Medicine'),
('Dr. Sara', 'Pediatrics');

------------------------------------------------------------
-- 5) Insert Sample Appointments
------------------------------------------------------------
INSERT INTO Appointments (PatientID, DoctorID, CreatedBy, StartTime, DurationMinutes, Status) VALUES 
(1, 1, 2, '2025-09-05 09:00:00', 30, 'Scheduled'),
(2, 2, 2, '2025-09-05 10:00:00', 60, 'Scheduled'),
(3, 1, 3, '2025-09-05 11:30:00', 15, 'Completed'),
(4, 2, 2, '2025-09-05 14:00:00', 30, 'Completed'),
(5, 1, 3, '2025-09-05 15:00:00', 30, 'No-Show'),
(6, 2, 2, '2025-09-06 09:00:00', 60, 'Scheduled'),
(7, 1, 3, '2025-09-06 10:30:00', 30, 'Completed'),
(8, 2, 2, '2025-09-06 16:00:00', 15, 'Canceled');

INSERT INTO Appointments (PatientID, DoctorID, CreatedBy, StartTime, DurationMinutes, Status) VALUES 
(1, 1, 2, '2025-09-15 09:00:00', 30, 'Scheduled'),
(2, 2, 3, '2025-09-15 10:15:00', 15, 'Scheduled'),
(3, 1, 2, '2025-09-15 13:30:00', 30, 'Scheduled'),

(4, 2, 2, '2025-09-16 09:15:00', 30, 'Scheduled'),
(5, 1, 3, '2025-09-16 11:00:00', 60, 'Scheduled'),
(6, 2, 2, '2025-09-16 15:45:00', 30, 'Scheduled'),

(7, 1, 2, '2025-09-17 09:30:00', 30, 'Scheduled'),
(8, 2, 3, '2025-09-17 10:45:00', 15, 'Scheduled'),
(1, 1, 2, '2025-09-17 14:00:00', 30, 'Scheduled'),

(2, 2, 2, '2025-09-18 09:00:00', 60, 'Scheduled'),
(3, 1, 3, '2025-09-18 11:15:00', 30, 'Scheduled'),
(4, 2, 2, '2025-09-18 16:30:00', 15, 'Scheduled'),

(5, 1, 2, '2025-09-19 09:45:00', 30, 'Scheduled'),
(6, 2, 3, '2025-09-19 13:15:00', 30, 'Scheduled'),
(7, 1, 2, '2025-09-19 15:00:00', 15, 'Scheduled');


select * from logs
select * from Appointments

select * from Payments
------------------------------------------------------------
-- 6) Insert Visit Notes (only for completed appointments)
------------------------------------------------------------
INSERT INTO VisitNotes (AppointmentID, DoctorID, Content) VALUES 
(3, 1, 'Patient complained of mild headache and fatigue. Vital signs normal. Prescribed rest and OTC pain medication. Follow-up if symptoms persist.'),
(4, 2, 'Routine pediatric checkup. Child is developing normally. Administered required vaccinations. Next visit scheduled in 6 months.'),
(7, 1, 'Follow-up visit for hypertension. Blood pressure well controlled with current medication. Continue current regimen.');

------------------------------------------------------------
-- 7) Insert Bills (for appointments with visit notes)
------------------------------------------------------------
INSERT INTO Bills (AppointmentID, PatientID, TotalAmount, PendingAmount, Status) VALUES 
(3, 3, 1500.00, 1500.00, 'Open'),
(4, 4, 2000.00, 1000.00, 'Partial'),
(7, 7, 1200.00, 0.00, 'Paid');

------------------------------------------------------------
-- 8) Insert Payments
------------------------------------------------------------
INSERT INTO Payments (BillID, Amount, Method, RecordedBy) VALUES 
(1, 1500.00, 'Cash', 2),
(2, 1000.00, 'Cash', 2),
(3, 1200.00, 'Card', 3);

------------------------------------------------------------
-- 9) Insert Sample Logs
------------------------------------------------------------
INSERT INTO Logs (UserID, Action, TableAffected, RecordID, Status) VALUES 
(2, 'INSERT', 'Patients', 1, 'Success'),
(2, 'INSERT', 'Appointments', 1, 'Success'),
(4, 'INSERT', 'VisitNotes', 1, 'Success'),
(2, 'INSERT', 'Bills', 1, 'Success'),
(2, 'INSERT', 'Payments', 1, 'Success'),
(3, 'UPDATE', 'Appointments', 5, 'Success'),
(1, 'DELETE', 'Users', 999, 'Failed');



INSERT INTO DoctorRates (DoctorID, RatePerMinute, EffectiveFrom)
SELECT DoctorID, 10.00, GETDATE()  
FROM Doctors WHERE DoctorName = 'Dr. Ahmed';

INSERT INTO DoctorRates (DoctorID, RatePerMinute, EffectiveFrom)
SELECT DoctorID, 12.50, GETDATE() 
FROM Doctors WHERE DoctorName = 'Dr. Sara';

