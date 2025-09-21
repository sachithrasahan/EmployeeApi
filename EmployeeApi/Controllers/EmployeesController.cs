using EmployeeApi.Models;
using EmployeeApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _service;
        private readonly IValidator<EmployeeRequestDto> _validator;
        private readonly IValidator<EmployeeUpdateRequestDto> _updateValidator;


        public EmployeesController(
            IEmployeeService service, 
            IValidator<EmployeeRequestDto> validator,
            IValidator<EmployeeUpdateRequestDto> updateValidator
        ) 
        { 
            _service = service;
            _validator = validator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.GetEmployeesAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var emp = await _service.GetEmployeeByIdAsync(id);
            if (emp == null) return NotFound();
            return Ok(emp);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmployeeRequestDto dto)
        {
            var result = await _validator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                return BadRequest(new
                {
                    Errors = result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                });
            }
            var id = await _service.InsertEmployeeAsync(dto);
            return Ok(new { EmployeeId = id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeUpdateRequestDto dto)
        {
            if (id != dto.EmployeeId)
                return BadRequest(new { message = "EmployeeId in URL and body do not match" });

            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    Errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                });
            }

            var success = await _service.UpdateEmployeeAsync(dto);
            if (!success)
                return NotFound(new { message = "Employee not found" });

            return Ok(new { message = "Employee updated successfully" });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteEmployeeAsync(id);
            return Ok();
        }
    }
}
