namespace SWD.SAPelearning.Repository.DTO
{
    public class CourseDTO
    {
        public int? InstructorId { get; set; }
        public string? InstructorName { get; set; } // Displayed after fetching from Instructor table
        public int? CertificateId { get; set; }
        public string? CourseName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Mode { get; set; }
        public double? Price { get; set; }
        public int? TotalStudent { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public string? Location { get; set; }
        public bool? Status { get; set; }
        

    }
}
