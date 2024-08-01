using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Business.DataTransferObjects.EmployeeTypes;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.WebApp.Data;
using Sprout.Exam.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeTypesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployeeTypes()
        {
            UpdateDB();
            var employeeTypes = await _dbContext.EmployeeType.ToListAsync();
            var employeeTypesDto = new List<EmployeeTypeDto>();
            foreach (var employeeType in employeeTypes)
            {
                var employeeTypeDto = new EmployeeTypeDto
                {
                    Id = employeeType.Id,
                    TypeName = employeeType.TypeName
                };
                employeeTypesDto.Add(employeeTypeDto);
            }
            if (employeeTypesDto.Count == 0) return NotFound();

            return Ok(employeeTypesDto);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetEmployeeTypeById(int id)
        {
            var employeeType = await _dbContext.EmployeeType.FindAsync(id);
            if (employeeType == null) return NotFound();
            return Ok(employeeType);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateEmployeeType(int id, EmployeeTypeDto employeeTypeDto)
        {
            var employeeType = await _dbContext.EmployeeType.FindAsync(id);
            if (employeeType == null) return NotFound();

            return Ok(employeeType);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeType(CreateEmployeeTypeDto createEmployeeTypeDto)
        {
            var employeeType = new Models.EmployeeType
            {
                Id = createEmployeeTypeDto.Id,
                TypeName = createEmployeeTypeDto.TypeName
            };
            await _dbContext.EmployeeType.AddAsync(employeeType);
            await _dbContext.SaveChangesAsync();
            return Ok(employeeType);
        }

       private void UpdateDB()
        {
            
            foreach(var name in Enum.GetNames(typeof(EmployeeTypeEnum)))
            {
                var id =(int)Enum.Parse(typeof(EmployeeTypeEnum), name);

                if(_dbContext.EmployeeType.Find(id)==null)
                {
                    var employeeType = new EmployeeType
                    {
                        Id = int.Parse(id.ToString()),
                        TypeName = name
                    };
                    _dbContext.EmployeeType.Add(employeeType);
                    _dbContext.SaveChanges();
                }
            }
        }

}
}
