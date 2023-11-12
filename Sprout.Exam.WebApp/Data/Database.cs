namespace Sprout.Exam.WebApp.Data
{
    using Microsoft.Extensions.Configuration;
    using Sprout.Exam.WebApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;

    public class Database<T> where T : new()
    {
        private readonly IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        public List<T> ListAll<T1>(T1 Class)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            var TableName = Class.GetType().Name;
            List<T> results = new List<T>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand($"SELECT * FROM {TableName}", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                T instance = new T();
                                MapDataToInstance(reader, instance);
                                results.Add(instance);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            return results;
        }

        private void MapDataToInstance(SqlDataReader reader, T instance)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;

                // Check if the column exists in the result set
                if (ColumnExists(reader, propertyName) && !reader.IsDBNull(reader.GetOrdinal(propertyName)))
                {
                    object value = reader[propertyName];
                    property.SetValue(instance, value);
                }
            }
        }

        private bool ColumnExists(SqlDataReader reader, string columnName)
        {
            try
            {
                return reader.GetOrdinal(columnName) >= 0;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }
    }
}
