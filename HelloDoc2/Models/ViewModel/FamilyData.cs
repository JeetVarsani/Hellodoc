using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace HelloDoc2.Models.ViewModel
{
    public class FamilyData
    {
        public string F_FirstName { get; set; }

        public string F_LastName { get; set; }
        public string F_Phone { get; set; }
        public string F_Email { get; set; }
        public string Relation { get; set; }  
        public string Symptoms { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string? Room { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string File { get; set; }

    }
}
