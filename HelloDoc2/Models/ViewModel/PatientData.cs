using System.ComponentModel.DataAnnotations;

namespace HelloDoc2.Models.ViewModel
{
    public class PatientData
    {
        public string Symptoms { get; set; }

        [Required(ErrorMessage = "Please Enter the First Name")]
        public string FirstName { get; set; }
            
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter the Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter the Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please Enter the Birth Date")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Please Enter  Password")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Street")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Please Enter City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please Enter State")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please Enter Zip Code")]
        public string ZipCode { get; set; }

        public string? Room { get; set; }

        public string Documents { get; set; }
        public string Status { get; set; }  
        public string CreatedDate { get; set; }

        public IFormFile? Filepath { get; set; } = null;

        public int Count { get; set; }
    }
}   
