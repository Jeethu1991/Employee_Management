CREATE PROCEDURE GetAllEmployees
AS
BEGIN
    SELECT * FROM LocalWorking.dbo.EmployeeDetails;
END;
