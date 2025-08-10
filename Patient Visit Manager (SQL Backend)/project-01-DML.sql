-- project-01-DML.sql
-- 8th August, 2025
-- Dawood Nadeem 6601
-- description: data manipulation language script for patient visit manager database

Insert into UserRoles (RoleName, Description) values
('Admin', 'Full system access with all privileges'),
('Receptionist', 'Limited access for patient registration and basic operations');

insert into VisitTypes (TypeName, BaseFee, EstimatedDuration) values
('Consultation', 500.00, 30),
('Follow-up', 300.00, 20),
('Emergency', 1000.00, 45),
('Check-up', 400.00, 25),
('Specialist', 750.00, 40);

insert into Doctors (FirstName, LastName, PhoneNumber, Email) values
('Dr. Ahmed', 'Ali',  '03001234567', 'ahmed.ali@clinic.com'),
('Dr. Fatima', 'Khan', '03009876543', 'fatima.khan@clinic.com'),
('Dr. Muhammad', 'Hassan', '03005555555', 'muhammad.hassan@clinic.com'),
('Dr. Ayesha', 'Sheikh', '03007777777', 'ayesha.sheikh@clinic.com'),
('Dr. Umar', 'Farooq',  '03008888888', 'umar.farooq@clinic.com');

Insert into Users (Username, PasswordHash, RoleID, FirstName, LastName) values
('admin', 'admin123', 1, 'System', 'Administrator'),
('reception1', 'rec123', 2, 'Sara', 'Ahmed'),
('reception2', 'rec456', 2, 'Ali', 'Hussain');

insert into Patients (FirstName, LastName, DateOfBirth, PhoneNumber, Email, Address, EmergencyContact) values
('Muhammad', 'Saleem', '1985-03-15', '03001111111', 'saleem@email.com', 'House 123, Model Town, Lahore', 'Wife: 03002222222'),
('Aisha', 'Butt', '1990-07-22', '03003333333', 'aisha.butt@email.com', 'Flat 456, DHA, Lahore', 'Husband: 03004444444'),
('Omar', 'Sheikh', '1975-11-08', '03005555555', 'omar.sheikh@email.com', 'Street 789, Gulberg, Lahore', 'Son: 03006666666'),
('Fatima', 'Noor', '1988-12-25', '03007777777', 'fatima.noor@email.com', 'House 321, Johar Town, Lahore', 'Sister: 03008888888'),
('Ahmed', 'Khan', '1992-04-10', '03009999999', 'ahmed.khan@email.com', 'Plot 654, Wapda Town, Lahore', 'Father: 03001010101'),
('Zara', 'Ali', '1995-09-18', '03001212121', 'zara.ali@email.com', 'House 987, Defence, Lahore', 'Mother: 03001313131'),
('Hassan', 'Malik', '1983-06-30', '03001414141', 'hassan.malik@email.com', 'Street 246, Garden Town, Lahore', 'Brother: 03001515151'),
('Mariam', 'Ashraf', '1987-01-12', '03001616161', 'mariam.ashraf@email.com', 'Flat 135, Cantt, Lahore', 'Husband: 03001717171'),
('Bilal', 'Qureshi', '1980-08-05', '03001818181', 'bilal.qureshi@email.com', 'House 579, Valencia, Lahore', 'Wife: 03001919191'),
('Sana', 'Tariq', '1993-10-20', '03002020202', 'sana.tariq@email.com', 'Plot 802, Bahria Town, Lahore', 'Father: 03002121212'),
('Kamran', 'Javed', '1978-05-14', '03002222223', 'kamran.javed@email.com', 'House 145, Faisal Town, Lahore', 'Wife: 03002323232'),
('Nadia', 'Farooq', '1991-02-28', '03002424242', 'nadia.farooq@email.com', 'Street 367, Iqbal Town, Lahore', 'Husband: 03002525252'),
('Imran', 'Siddiqui', '1986-09-11', '03002626262', 'imran.siddiqui@email.com', 'Plot 891, Lake City, Lahore', 'Brother: 03002727272'),
('Rabia', 'Hassan', '1989-12-03', '03002828282', 'rabia.hassan@email.com', 'House 234, Township, Lahore', 'Sister: 03002929292'),
('Usman', 'Shah', '1982-07-19', '03003030303', 'usman.shah@email.com', 'Flat 567, EME Society, Lahore', 'Father: 03003131313'),
('Ayesha', 'Riaz', '1994-04-26', '03003232323', 'ayesha.riaz@email.com', 'Street 789, Muslim Town, Lahore', 'Mother: 03003333334'),
('Farhan', 'Ahmed', '1977-11-15', '03003434343', 'farhan.ahmed@email.com', 'House 456, Sui Gas, Lahore', 'Wife: 03003535353'),
('Kiran', 'Akram', '1985-08-07', '03003636363', 'kiran.akram@email.com', 'Plot 123, Shadman, Lahore', 'Husband: 03003737373'),
('Adnan', 'Butt', '1990-01-22', '03003838383', 'adnan.butt@email.com', 'House 678, Samanabad, Lahore', 'Brother: 03003939393'),
('Samina', 'Khan', '1988-06-09', '03004040404', 'samina.khan@email.com', 'Street 345, Allama Iqbal Town, Lahore', 'Sister: 03004141414');

insert into Patients (FirstName, LastName, DateOfBirth, PhoneNumber, Email, Address, EmergencyContact) values
('Tariq', 'Mahmood', '1979-03-22', '03004141415', 'tariq.mahmood@email.com', 'House 890, Cavalry Ground, Lahore', 'Wife: 03004242424'),
('Farah', 'Iqbal', '1992-11-17', '03004343434', 'farah.iqbal@email.com', 'Street 123, Jail Road, Lahore', 'Father: 03004444445'),
('Waqas', 'Ahmad', '1984-07-03', '03004545454', 'waqas.ahmad@email.com', 'Plot 456, Phase 5, DHA, Lahore', 'Brother: 03004646464'),
('Sidra', 'Waseem', '1989-05-12', '03004747474', 'sidra.waseem@email.com', 'House 789, Gulshan Ravi, Lahore', 'Husband: 03004848484'),
('Kashif', 'Raza', '1976-09-28', '03004949494', 'kashif.raza@email.com', 'Street 321, Shalimar Link Road, Lahore', 'Son: 03005050505'),
('Hina', 'Pervez', '1987-12-08', '03005151515', 'hina.pervez@email.com', 'House 654, Thokar Niaz Baig, Lahore', 'Sister: 03005252525'),
('Salman', 'Haider', '1981-04-15', '03005353535', 'salman.haider@email.com', 'Plot 987, Green Town, Lahore', 'Wife: 03005454545'),
('Rubina', 'Siddique', '1994-08-27', '03005555556', 'rubina.siddique@email.com', 'Street 147, Badami Bagh, Lahore', 'Mother: 03005656565'),
('Asif', 'Nazir', '1983-01-19', '03005757575', 'asif.nazir@email.com', 'House 258, Walled City, Lahore', 'Father: 03005858585'),
('Sadia', 'Yousuf', '1991-06-11', '03005959595', 'sadia.yousuf@email.com', 'Flat 369, Mall Road, Lahore', 'Husband: 03006060606');

insert into ActivityLog (UserID, Action, TableAffected, RecordID, Status) values
(2, 'Add', 'Visits', 1, 'Success'),
(2, 'Update', 'Visits', 1, 'Success'),
(3, 'Add', 'Visits', 3, 'Success'),
(2, 'Search', 'Visits', null, 'Success'),
(1, 'Add', 'Doctors', 5, 'Success'),
(2, 'Update', 'Patients', 1, 'Success'),
(3, 'Delete', 'Visits', 25, 'Success'),
(2, 'Add', 'Patients', 20, 'Success'),
(3, 'Search', 'Patients', null, 'Success'),
(1, 'Add', 'Users', 3, 'Success');
