IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='timesheet' AND xtype = 'U')
BEGIN
    CREATE TABLE dbo.timesheet (
        timesheetId INT IDENTITY (1, 1),
        employeeId INT NOT NULL,
        workDate DATETIME NOT NULL,
        createdDate DATETIME NOT NULL,
        createdBy NVARCHAR(30) NOT NULL,
        modifiedDate DATETIME NOT NULL,        
        modifiedBy NVARCHAR(30) NOT NULL,

        CONSTRAINT PK_timesheet_timesheetId PRIMARY KEY (timesheetId),
        CONSTRAINT FK_employee_timesheet_employeeId FOREIGN KEY (employeeId) REFERENCES employee (employeeId)
    )
END
GO
