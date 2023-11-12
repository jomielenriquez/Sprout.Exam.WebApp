using Newtonsoft.Json;
using System;

namespace Sprout.Exam.WebApp.Models
{
    public class AppModel
    {
    }

    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Birthdate { get; set; }
        public string TIN { get; set; }
        public int TypeId { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class CalculatePayload
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("workedDays")]
        public string workedDays { get; set; }
        [JsonProperty("absentDays")]
        public string absentDays { get; set; }
    }
}
