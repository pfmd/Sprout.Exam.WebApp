using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.WebApp.Data;
using Microsoft.EntityFrameworkCore;
using Sprout.Exam.WebApp.Models;
using Sprout.Exam.WebApp.Salary;
using Sprout.Exam.Business.DataTransferObjects.Employees;

namespace Sprout.Exam.WebApp.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>

        public EmployeesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var employeesDto = new List<EmployeeDto>();
                var employees = await _dbContext.Employee.Where(x => x.IsDeleted == false).ToListAsync();

                foreach (var employee in employees)
                {
                    var employeeDTO = new EmployeeDto
                    {
                        Birthdate = employee.Birthdate.ToString("yyyy-MM-dd"),
                        FullName = employee.FullName,
                        Id = employee.Id,
                        Tin = employee.TIN,
                        TypeId = employee.EmployeeTypeId

                    };
                    employeesDto.Add(employeeDTO);
                }

                return Ok(employeesDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var employee = await _dbContext.Employee.FindAsync(id);
                var employeeDto = new EmployeeDto
                {
                    Id = employee.Id,
                    FullName = employee.FullName,
                    Birthdate = employee.Birthdate.ToString("yyyy-MM-dd"),
                    Tin = employee.TIN,
                    TypeId = employee.EmployeeTypeId
                };
                return Ok(employeeDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EditEmployeeDto editEmployeeDto)
        {
          
            try
            {
                var employee = await _dbContext.Employee.FindAsync(id);
                if (employee == null) return NotFound();

                employee.Id = editEmployeeDto.Id;
                employee.Birthdate = editEmployeeDto.Birthdate;
                employee.FullName = editEmployeeDto.FullName;
                employee.TIN = editEmployeeDto.Tin;
                employee.EmployeeTypeId = editEmployeeDto.TypeId;

                await _dbContext.SaveChangesAsync();


                return Ok(editEmployeeDto);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeDto input)
        {
            try
            {
                var id = await _dbContext.Employee.MaxAsync(m => m.Id) + 1;
                var employee = new Employee
                {
                    //Id = id,
                    Birthdate = DateTime.Parse(input.Birthdate.ToString("yyyy-MM-dd")),
                    FullName = input.FullName,
                    TIN = input.Tin,
                    EmployeeTypeId = input.TypeId
                };

                await _dbContext.Employee.AddAsync(employee);
                await _dbContext.SaveChangesAsync();

                return Created($"/api/employees/{id}", id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var employee = await _dbContext.Employee.FindAsync(id);
                if (employee == null) return NotFound();
                employee.IsDeleted = true;
                await _dbContext.SaveChangesAsync();

                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        /// <summary>
        /// Refactor this method to go through proper layers and use Factory pattern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>
        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> Calculate(int id, SalaryParams salaryParams)
        {
            try
            {
                var employee = await _dbContext.Employee.FindAsync(id);
                var employeeType = (EmployeeTypeEnum)employee.EmployeeTypeId;

                ISalary salary = Salary.Salary.Compute(employeeType);
                decimal netIncome = salary.NetIncome(salaryParams);

                return Ok(netIncome.ToString("0.00"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            

        }

    }
}
