using EmployeeApi.Models;

namespace EmployeeApi.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeResponseDto>> GetEmployeesAsync();
        Task<EmployeeResponseDto?> GetEmployeeByIdAsync(int id);
        Task<int> InsertEmployeeAsync(EmployeeRequestDto dto);
        Task<bool> UpdateEmployeeAsync(EmployeeUpdateRequestDto dto);
        Task DeleteEmployeeAsync(int id);
        Task<int> GetTotalEmployeesAsync();
    }
}
