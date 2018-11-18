CREATE TABLE dbo.timesheet (
    timesheetId INT IDENTITY (1, 1),
    employeeId INT NOT NULL,
    createdDate DATETIME NOT NULL,
    modifiedDate DATETIME NOT NULL,
    createdBy NVARCHAR(30) NOT NULL,
    modifiedBy NVARCHAR(30) NOT NULL
)