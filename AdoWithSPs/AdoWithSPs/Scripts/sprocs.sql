USE SWCCorp;
GO

-- list employees with their pay rate data
IF EXISTS(
        SELECT *
        FROM INFORMATION_SCHEMA.ROUTINES
        WHERE ROUTINE_NAME = 'EmployeeRatesSelect'
    )
    BEGIN
        DROP PROCEDURE EmployeeRatesSelect
    end
GO

CREATE PROCEDURE EmployeeRatesSelect
AS
SELECT E.EmpID, E.FirstName, E.LastName, PR.HourlyRate, PR.MonthlySalary, PR.YearlySalary
FROM Employee E
         JOIN PayRates PR on E.EmpID = PR.EmpID;
GO

-- param - get list of employee with pay rate data by city
IF EXISTS(
        SELECT *
        FROM INFORMATION_SCHEMA.ROUTINES
        WHERE ROUTINE_NAME = 'EmployeeRatesSelectByCity'
    )
    BEGIN
        DROP PROCEDURE EmployeeRatesSelectByCity
    end
GO

CREATE PROCEDURE EmployeeRatesSelectByCity(
    @City VARCHAR(20)
)
AS
SELECT E.EmpID, E.FirstName, E.LastName, PR.HourlyRate, PR.MonthlySalary, PR.YearlySalary
FROM Employee E
         JOIN PayRates PR on E.EmpID = PR.EmpID
         JOIN Location L on L.LocationID = E.LocationID
WHERE L.City = @City;
GO