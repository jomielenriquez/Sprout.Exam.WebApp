namespace Sprout.Exam.WebApp.Data
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Sprout.Exam.WebApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    public class EmployeeRepository
    {
        private readonly IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        public List<Employee> ListAll()
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            List<Employee> list = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand($"SELECT * FROM Employee where IsDeleted = 0", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Employee employee = new Employee();

                                employee.Id = (int)reader["Id"];
                                employee.FullName = reader["FullName"].ToString();
                                IFormatProvider culture = new CultureInfo("en-US", true);
                                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
                                employee.Birthdate = reader["Birthdate"].ToString().Split(" ")[0].Replace("/","-");
                                employee.TIN = reader["TIN"].ToString();
                                employee.TypeId = (int)reader["EmployeeTypeId"];
                                employee.IsDeleted = (bool)reader["IsDeleted"];

                                list.Add(employee);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return list;
        }

        public Employee ListById(int Id)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            Employee employee = new Employee();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand($"SELECT * FROM Employee where IsDeleted = 0 and Id = {Id}", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employee.Id = (int)reader["Id"];
                                employee.FullName = reader["FullName"].ToString();
                                IFormatProvider culture = new CultureInfo("en-US", true);
                                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
                                employee.Birthdate = reader["Birthdate"].ToString().Split(" ")[0].Replace("/", "-");
                                employee.TIN = reader["TIN"].ToString();
                                employee.TypeId = (int)reader["EmployeeTypeId"];
                                employee.IsDeleted = (bool)reader["IsDeleted"];
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            return employee;
        }
        public object UpdateEmployeeWithId(Employee employee)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            object result = new
            {
                success = "true"
            };

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("UPDATE Employee SET FullName = @FullName, Birthdate = @Birthdate, TIN = @TIN, EmployeeTypeId = @EmployeeTypeId WHERE Id = @Id", connection))
                    {
                        // Add parameters to the command
                        command.Parameters.AddWithValue("@Id", employee.Id);
                        command.Parameters.AddWithValue("@FullName", employee.FullName);
                        command.Parameters.AddWithValue("@Birthdate", employee.Birthdate);
                        command.Parameters.AddWithValue("@TIN", employee.TIN);
                        command.Parameters.AddWithValue("@EmployeeTypeId", employee.TypeId);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            result = new
                            {
                                success = "true"
                            };
                        }
                        else
                        {
                            result = new
                            {
                                success = "true"
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            return result;
        }
        public object DeleteEmployeeWithId(int Id)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            object result = new
            {
                success = "true"
            };

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("UPDATE Employee SET IsDeleted = 1WHERE Id = @Id", connection))
                    {
                        // Add parameters to the command
                        command.Parameters.AddWithValue("@Id", Id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            result = new
                            {
                                success = "true"
                            };
                        }
                        else
                        {
                            result = new
                            {
                                success = "true"
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            return result;
        }
        public Employee InsertNewEmployee(Employee employee)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Assuming 'employee' is an instance of the Employee class with new values
                    using (SqlCommand command = new SqlCommand("INSERT INTO Employee (FullName, Birthdate, TIN, EmployeeTypeId, IsDeleted) VALUES (@FullName, @Birthdate, @TIN, @EmployeeTypeId, @IsDeleted); SELECT SCOPE_IDENTITY()", connection))
                    {
                        // Add parameters to the command
                        command.Parameters.AddWithValue("@FullName", employee.FullName);
                        command.Parameters.AddWithValue("@Birthdate", employee.Birthdate);
                        command.Parameters.AddWithValue("@TIN", employee.TIN);
                        command.Parameters.AddWithValue("@EmployeeTypeId", employee.TypeId);
                        command.Parameters.AddWithValue("@IsDeleted", employee.IsDeleted);

                        // Execute the command and get the newly inserted Id
                        int newEmployeeId = Convert.ToInt32(command.ExecuteScalar());

                        employee.Id = newEmployeeId;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            return employee;
        }
    }
}
