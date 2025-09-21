using AutoMapper;
using EmployeeApi.Data;
using EmployeeApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EmployeeApi.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EmployeeService(AppDbContext context, IMapper mapper)
        {
            _context = context; _mapper = mapper;
        }

        public async Task<List<EmployeeResponseDto>> GetEmployeesAsync()
        {
            var list = await _context.Employees.FromSqlRaw("EXEC GetEmployees").ToListAsync();
            return _mapper.Map<List<EmployeeResponseDto>>(list);
        }

        public async Task<EmployeeResponseDto?> GetEmployeeByIdAsync(int id)
        {
            var list = await _context.Employees
                .FromSqlRaw($"EXEC GetEmployeeById @EmployeeId={id}")
                .ToListAsync();
            return _mapper.Map<EmployeeResponseDto?>(list?.FirstOrDefault());
        }

        public async Task<int> InsertEmployeeAsync(EmployeeRequestDto dto)
        {
            // use raw DbConnection + SqlCommand to get scalar (new id)
            await using var conn = (SqlConnection)_context.Database.GetDbConnection();
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "InsertEmployee";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@EmployeeName", dto.EmployeeName));
            cmd.Parameters.Add(new SqlParameter("@Designation", dto.Designation));
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateEmployeeAsync(EmployeeUpdateRequestDto dto)
        {
            var rows = await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC UpdateEmployee @EmployeeId={dto.EmployeeId}, @EmployeeName={dto.EmployeeName}, @Designation={dto.Designation}");

            return rows > 0;
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC DeleteEmployee @EmployeeId={0}", id);
        }

        public async Task<int> GetTotalEmployeesAsync()
        {
            await using var conn = (SqlConnection)_context.Database.GetDbConnection();
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "GetTotalEmployees";
            cmd.CommandType = CommandType.StoredProcedure;
            var outParam = new SqlParameter("@TotalCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outParam);
            await cmd.ExecuteNonQueryAsync();
            return (int)outParam.Value!;
        }
    }
}
