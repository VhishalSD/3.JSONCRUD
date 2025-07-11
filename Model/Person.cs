using System;

namespace JSONCRUD.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
    }
}
