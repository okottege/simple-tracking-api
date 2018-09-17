IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='dbo.employee' AND xtype = 'U')
BEGIN
    CREATE TABLE dbo.employee(
        employeeId  INT IDENTITY(1, 1),
        firstName NVARCHAR(50),
        lastName NVARCHAR(50),
        dateOfBirth DATE,
        startDate DATE
    );

    ALTER TABLE dbo.employee ADD CONSTRAINT PK_employee_employeeId PRIMARY KEY
END
GO;

