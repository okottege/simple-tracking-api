IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='employee' AND xtype = 'U')
BEGIN
    CREATE TABLE dbo.employee(
        employeeId  INT IDENTITY(1, 1),
        firstName NVARCHAR(50) NOT NULL,
        lastName NVARCHAR(50) NOT NULL,
        dateOfBirth DATE NOT NULL,
        startDate DATE NOT NULL,

        CONSTRAINT PK_employee_employeeId PRIMARY KEY (employeeId)
    );
END
GO

IF (COL_LENGTH('dbo.Employee', 'Email') IS NULL)
BEGIN
    ALTER TABLE Employee ADD Email NVARCHAR(300)        
END
GO