namespace HelloDoc2.Models.ViewModel
{
    public class PatientRequestForMeAndSomeone
    {
        public string Symptoms { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public string Room { get; set; }
        public IFormFile? Filepath { get; set; } = null;

        public string Relation { get; set; }

        public string Comments { get; set; }
    }
}
