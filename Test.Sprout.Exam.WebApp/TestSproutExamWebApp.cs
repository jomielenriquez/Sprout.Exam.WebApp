using System;
using Xunit;
using Sprout.Exam.WebApp.Data;
using Sprout.Exam.WebApp.Controllers;
using Sprout.Exam.WebApp.Models;
using Moq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Test.Sprout.Exam.WebApp
{
    public class TestSproutExamWebApp
    {
        [Fact]
        public async void ShouldReturnListOfEmployees()
        {
            // Arrange
            var employeeRepository = new Mock<EmployeeRepository>();
            employeeRepository.Setup(repo => repo.ListAll()).Returns(new List<Employee>() { 
                new Employee
                {
                    Birthdate = "1993-03-25",
                    FullName = "Jane Doe",
                    Id = 1,
                    TIN = "123215413",
                    TypeId = 1
                },
                new Employee
                {
                    Birthdate = "1993-05-28",
                    FullName = "John Doe",
                    Id = 2,
                    TIN = "957125412",
                    TypeId = 2
                }
            });

            var employeesController = new EmployeesController();
            employeesController.EmployeeRepository = employeeRepository.Object;

            // Act
            var result = await employeesController.Get();

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)result;
            Assert.IsType<List<Employee>>(okObjectResult.Value);

            var employees = (List<Employee>)okObjectResult.Value;

            Assert.Equal(2, employees.Count);
            Assert.Equal("Jane Doe", employees[0].FullName);
            Assert.Equal("John Doe", employees[1].FullName);
        }

        [Fact]
        public async void ShouldReturnEmployee()
        {
            // Arrange
            var employeeRepository = new Mock<EmployeeRepository>();
            employeeRepository.Setup(repo => repo.ListById(2)).Returns(new Employee()
            {
                Birthdate = "1993-05-28",
                FullName = "John Doe",
                Id = 2,
                TIN = "957125412",
                TypeId = 2
            });

            var employeesController = new EmployeesController();
            employeesController.EmployeeRepository = employeeRepository.Object;

            // Act
            var result = await employeesController.GetById(2);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)result;
            Assert.IsType<Employee>(okObjectResult.Value);

            var employee = (Employee)okObjectResult.Value;

            Assert.Equal(2, employee.Id);
            Assert.Equal("John Doe", employee.FullName);
        }

        [Fact]
        public async void ShouldReturnOkOnUpdate()
        {
            // Arrange
            Employee employee = new Employee()
            {
                Birthdate = "1993-05-28",
                FullName = "John Doe",
                Id = 2,
                TIN = "957125412",
                TypeId = 2
            };
            var employeeRepository = new Mock<EmployeeRepository>();
            employeeRepository.Setup(repo => repo.UpdateEmployeeWithId(employee)).Returns(new Result<Employee>()
            {
                Success = true,
            });

            var employeesController = new EmployeesController();
            employeesController.EmployeeRepository = employeeRepository.Object;

            // Act
            var result = await employeesController.Put(employee);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)result;
            Assert.IsType<Employee>(okObjectResult.Value);

            var data = (Employee)okObjectResult.Value;

            Assert.Equal("John Doe", data.FullName);
        }

        [Fact]
        public async void ShouldErrorOnUpdate()
        {
            // Arrange
            Employee employee = new Employee()
            {
                Birthdate = "1993-05-28",
                FullName = "John Doe",
                Id = 2,
                TIN = "957125412",
                TypeId = 2
            };
            var employeeRepository = new Mock<EmployeeRepository>();
            employeeRepository.Setup(repo => repo.UpdateEmployeeWithId(employee)).Returns(new Result<Employee>()
            {
                Success = false,
            });

            var employeesController = new EmployeesController();
            employeesController.EmployeeRepository = employeeRepository.Object;

            // Act
            var result = await employeesController.Put(employee);

            // Assert
            Assert.IsType<ObjectResult>(result);

            var objectResult = (ObjectResult)result;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Null(objectResult.Value);
        }

        [Fact]
        public async void ShouldSuccessOnPost()
        {
            // Arrange
            Employee employee = new Employee()
            {
                Birthdate = "1993-05-28",
                FullName = "John Doe",
                Id = 2,
                TIN = "957125412",
                TypeId = 2
            };

            var employeeRepository = new Mock<EmployeeRepository>();
            employeeRepository.Setup(repo => repo.InsertNewEmployee(employee)).Returns(new Result<Employee>()
            { 
                Success = true,
                Data = employee
            });

            var employeesController = new EmployeesController();
            employeesController.EmployeeRepository = employeeRepository.Object;

            // Act
            var result = await employeesController.Post(employee);

            // Assert
            Assert.IsType<CreatedResult>(result);

            var createdResult = (CreatedResult)result;
            Assert.Equal("/api/employees/2", createdResult.Location);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(2, createdResult.Value);
        }

        [Fact]
        public async void ShouldErrorOnPost()
        {
            // Arrange
            Employee employee = new Employee()
            {
                Birthdate = "1993-05-28",
                FullName = "John Doe",
                Id = 2,
                TIN = "957125412",
                TypeId = 2
            };

            var employeeRepository = new Mock<EmployeeRepository>();
            employeeRepository.Setup(repo => repo.InsertNewEmployee(employee)).Returns(new Result<Employee>()
            {
                Success = false,
                Message = "Error"
            });

            var employeesController = new EmployeesController();
            employeesController.EmployeeRepository = employeeRepository.Object;

            // Act
            var result = await employeesController.Post(employee);

            // Assert
            Assert.IsType<ObjectResult>(result);

            var objectResult = (ObjectResult)result;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Error", objectResult.Value);
        }

        [Fact]
        public async void ShouldSuccessOnDelete()
        {
            // Arrange
            Employee employee = new Employee()
            {
                Birthdate = "1993-05-28",
                FullName = "John Doe",
                Id = 2,
                TIN = "957125412",
                TypeId = 2
            };

            var employeeRepository = new Mock<EmployeeRepository>();
            employeeRepository.Setup(repo => repo.DeleteEmployeeWithId(employee.Id)).Returns(new Result<Employee>()
            {
                Success = true,
                Data = employee
            });

            var employeesController = new EmployeesController();
            employeesController.EmployeeRepository = employeeRepository.Object;

            // Act
            var result = await employeesController.Delete(employee.Id);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)result;
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(2, okObjectResult.Value);
        }

        [Fact]
        public async void ShouldErrorOnDelete()
        {
            // Arrange
            Employee employee = new Employee()
            {
                Birthdate = "1993-05-28",
                FullName = "John Doe",
                Id = 2,
                TIN = "957125412",
                TypeId = 2
            };

            var employeeRepository = new Mock<EmployeeRepository>();
            employeeRepository.Setup(repo => repo.DeleteEmployeeWithId(employee.Id)).Returns(new Result<Employee>()
            {
                Success = false,
                Message = "Error"
            });

            var employeesController = new EmployeesController();
            employeesController.EmployeeRepository = employeeRepository.Object;

            // Act
            var result = await employeesController.Delete(employee.Id);

            // Assert
            Assert.IsType<ObjectResult>(result);

            var objectResult = (ObjectResult)result;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Error", objectResult.Value);
        }

        [Theory]
        [InlineData(2, "0","0", 0)]
        [InlineData(1, "1","0", 16690.91)]
        [InlineData(2, "0","15.5", 7750)]
        public async void ShouldSuccessOnCalculate(int type, string absentDays, string workedDays, Decimal Result)
        {
            // Arrange
            CalculatePayload payload = new CalculatePayload() { 
                id = 2,
                absentDays = absentDays,
                workedDays = workedDays
            };
            var employeeRepository = new Mock<EmployeeRepository>();
            employeeRepository.Setup(repo => repo.ListById(2)).Returns(new Employee()
            {
                Birthdate = "1993-05-28",
                FullName = "John Doe",
                Id = 2,
                TIN = "957125412",
                TypeId = type
            });

            var employeesController = new EmployeesController();
            employeesController.EmployeeRepository= employeeRepository.Object;

            // Act
            var result = await employeesController.Calculate(payload);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)result;
            Assert.Equal(Result, okObjectResult.Value);
        }
    }
}
