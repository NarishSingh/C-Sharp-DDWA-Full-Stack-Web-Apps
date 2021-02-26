namespace ADO.NET_Queries.Models
{
    /// <summary>
    /// For SWCCorp db, Employee table
    /// </summary>
    public class EmployeePayRate
    {
        public int EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal? MonthlySalary { get; set; }
        public decimal? YearlySalary { get; set; }
    }
}