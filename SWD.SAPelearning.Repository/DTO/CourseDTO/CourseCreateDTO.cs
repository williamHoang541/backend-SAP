

namespace SWD.SAPelearning.Repository.DTO.CourseDTO
{
    public  class CourseCreateDTO
    {
       
        public int? InstructorId { get; set; }
        public int? CertificateId { get; set; }
        public string? CourseName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Mode { get; set; } // Should be either "Online" or "Offline"
        public double? Price { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public string? Location { get; set; }
        public bool? Status { get; set; }
    }
}
