using FluentAssertions;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sprout.Exam.Business.DataTransferObjects.Employees;
using Sprout.Exam.WebApp.Controllers;
using Sprout.Exam.WebApp.Data;
using Sprout.Exam.WebApp.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sprout.Exam.WebApp.Test.Systems.Controllers
{
    public class TestEmployeeController
    {

        private async Task<ApplicationDbContext> GetDatabaseContext()
        {
            var options= new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName:"TESTDB")
                .Options;
            var operationalStoreOptions = Options.Create(new OperationalStoreOptions());
            var databaseContext = new ApplicationDbContext(options , operationalStoreOptions);
            databaseContext.Database.EnsureCreated();

            if(await databaseContext.Employee.CountAsync()<=0)
            {
                for(int i=0;i<10;i++)
                {
                    databaseContext.Employee.Add(
                        new Employee
                        {
                            FullName ="MARK DAVE TEST" ,
                            Birthdate =new DateTime(2000,2,2),
                            EmployeeTypeId=1,
                            IsDeleted=false,
                            TIN="111010011"
                        }
                        );
                    await databaseContext.SaveChangesAsync();
                }
            }

            return databaseContext;
        }
      
        [Fact]
        public async void EmployeeController_GetEmployee_ReturnsEmployee()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var employeeController = new EmployeesController(dbContext);

            //act
            var result = employeeController.Get();

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async void EmployeeController_GetEmployeeById_ReturnsEmployee()
        {
            //Arrange
            var id = 1;
            var dbContext = await GetDatabaseContext();
            var employeeController = new EmployeesController(dbContext);

            //act
            var result = employeeController.GetById(id);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async void EmployeeController_AddEmployee_ReturnsEmployee()
        {

            //Arrange
            var employeeDto = new CreateEmployeeDto
            {
                FullName = "MARK DAVE TEST",
                Birthdate = new DateTime(2000, 2, 2),
                TypeId = 1,
                Tin = "111010011"
            };
            var dbContext = await GetDatabaseContext();
            var employeeController = new EmployeesController(dbContext);

            //act
            var result = employeeController.Post( employeeDto);

            //Assert
            result.Should().NotBeNull();
        }


        [Fact]
        public async void EmployeeController_EditEmployee_ReturnsEmployee()
        {
            //Arrange
            var id = 1;
            var employeeDto = new EditEmployeeDto
            {
                FullName = "MARK DAVE TEST",
                Birthdate = new DateTime(2000, 2, 2),
                TypeId = 1,
                Tin = "111010011"
            };
            var dbContext = await GetDatabaseContext();
            var employeeController = new EmployeesController(dbContext);

            //act
            var result = employeeController.Put(id, employeeDto);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async void EmployeeController_DeleteEmployee_ReturnsEmployee()
        {
            //Arrange
            var id = 3;
           
            var dbContext = await GetDatabaseContext();
            var employeeController = new EmployeesController(dbContext);

            //act
            var result = employeeController.Delete(id);

            //Assert
            result.Should().NotBeNull();
        }


    }
}
