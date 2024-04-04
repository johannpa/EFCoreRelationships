using EFCoreRelationships.Data;
using EFCoreRelationships.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreRelationships.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public EmployeeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        //Read all employees from the database
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetEmployees()
        {
            var employees = await _appDbContext.Employees.ToListAsync();
            return Ok(employees);
        }

        // Read one employee
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _appDbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee != null)
            {
                return Ok(employee);
            }

            return NotFound();
        }

        //Add employee
        [HttpPost]
        public async Task<ActionResult<List<Employee>>> AddEmployee(Employee newEmployee)
        {
            if (newEmployee != null)
            {
                _appDbContext.Employees.Add(newEmployee);
                await _appDbContext.SaveChangesAsync();

                var employees = await _appDbContext.Employees.ToListAsync();
                return Ok(employees);
            }
            return BadRequest();
        }

        //Delete
        [HttpDelete]
        public async Task<ActionResult<List<Employee>>> DeleteEmployee(int id)
        {
            var employee = await _appDbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee != null)
            {
                _appDbContext.Employees.Remove(employee);
                await _appDbContext.SaveChangesAsync();
                var employees = await _appDbContext.Employees.ToListAsync();
                return Ok(employees);
            }
            return BadRequest();
        }


        [HttpPut]
        public async Task<ActionResult<Employee>> UpdateEmployee(Employee updateEmployee)
        {
            if(updateEmployee != null)
            {
                var employee = await _appDbContext.Employees.FirstOrDefaultAsync(e => e.Id == updateEmployee.Id);
                if(employee != null)
                {
                    employee.Name = updateEmployee.Name;
                    employee.Age = updateEmployee.Age;
                    await _appDbContext.SaveChangesAsync();

                    var employees = await _appDbContext.Employees.ToListAsync();
                    return Ok(employees);
                }
                return NotFound();
            }

            return BadRequest();
        }
    }
}
