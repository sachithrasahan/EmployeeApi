namespace EmployeeApi.Models
{
    public class EmployeeUpdateRequestDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
    }
}
