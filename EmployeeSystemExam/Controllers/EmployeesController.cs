using Microsoft.AspNetCore.Mvc;
using EmployeeSystemExam.Models;
using EmployeeSystemExam.Repositories;


namespace EmployeeSystemExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeeRepository employeeRepository, ILogger<EmployeesController> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            try
            {
                var employees = await _employeeRepository.GetEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving employees.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployee(id);
                if (employee == null)
                {
                    return NotFound("Employee not found");
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the employee with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(AddEmployee addemployee)
        {
            try
            {
                var employee = new Employee
                {
                    FirstName = addemployee.FirstName,
                    MiddleName = addemployee.MiddleName,
                    LastName = addemployee.LastName
                };

                var newEmployee = await _employeeRepository.AddEmployee(employee);
                return CreatedAtAction(nameof(GetEmployee), new { id = newEmployee.Id }, newEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new employee.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, UpdateEmployee updateemployee)
        {
            try
            {
                
                var employee = new Employee
                {
                    Id = id,
                    FirstName = updateemployee.FirstName,
                    MiddleName = updateemployee.MiddleName,
                    LastName = updateemployee.LastName
                };

                var updatedEmployee = await _employeeRepository.UpdateEmployee(employee);
                if (updatedEmployee == null)
                {
                    return NotFound("Employee not found");
                }

                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the employee with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _employeeRepository.DeleteEmployee(id);
                if (employee == null)
                {
                    return NotFound("Employee not found");
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the employee with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
