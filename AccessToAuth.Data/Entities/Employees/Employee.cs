using System.ComponentModel.DataAnnotations;

namespace AccessToAuth.Data.Entities.Employees
{
    public class Employee
    {

        [Key]
        public Guid ID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? City { get; set; }
        public string? Gender { get; set; }
    }
}