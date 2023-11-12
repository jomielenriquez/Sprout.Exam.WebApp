﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.WebApp.Data;
using Sprout.Exam.WebApp.Models;
using Newtonsoft.Json;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            EmployeeRepository employeeRepository = new EmployeeRepository();
            var result = await Task.FromResult(employeeRepository.ListAll());
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository();
            var result = await Task.FromResult(employeeRepository.ListById(id));
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Employee input)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository();
            Result result = employeeRepository.UpdateEmployeeWithId(input);
            
            if(result.Success)
            {
                return Ok(input);
            }
            else
            {
                return StatusCode(500, result.Message);
            }
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(Employee input)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository();
            Employee result = employeeRepository.InsertNewEmployee(input);
            
            return Created($"/api/employees/{result.Id}", result.Id);
        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository();
            object result = employeeRepository.DeleteEmployeeWithId(id);
            return Ok(id);
        }



        /// <summary>
        /// Refactor this method to go through proper layers and use Factory pattern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>
        [HttpPost("{id}/calculate")]
        //public async Task<IActionResult> Calculate(int id, decimal absentDays, string workedDays)
        public async Task<IActionResult> Calculate(CalculatePayload input)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository();
            var result = employeeRepository.ListById(input.id);

            if (result == null) return NotFound();

            var type = (EmployeeType) result.TypeId;
            return type switch
            {
                EmployeeType.Regular =>
                    //create computation for regular.
                    Ok(CalculateRegular(decimal.Parse(input.absentDays))),
                EmployeeType.Contractual =>
                    //create computation for contractual.
                    Ok(CalculateContractual(decimal.Parse(input.workedDays))),
                _ => NotFound("Employee Type not found")
            };
        }

        private decimal CalculateRegular(decimal absentDays)
        {
            decimal result = 20000M - (absentDays * (20000M / 22M)) - (20000M * 0.12M);
            result = Math.Round(result, 2);
            return result;
        }

        private decimal CalculateContractual(decimal workedDays)
        {
            decimal result = 500M * workedDays;
            result = Math.Round(result, 2);
            return result;
        }
    }
}
