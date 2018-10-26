IF (COL_LENGTH('dbo.Employee', 'Email') IS NULL)
BEGIN
    ALTER TABLE Employee ADD Email NVARCHAR(300)        
END
GO