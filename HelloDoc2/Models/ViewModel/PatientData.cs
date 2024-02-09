namespace HelloDoc2.Models.ViewModel
{
    public class PatientData
    {
        public String Symptoms { get; set; }
        public string FirstName { get; set; }
            
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public string PasswordHash { get; set; }
        
        public string Email { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string? Room { get; set; }

        public string File { get; set; }

    }
}   
