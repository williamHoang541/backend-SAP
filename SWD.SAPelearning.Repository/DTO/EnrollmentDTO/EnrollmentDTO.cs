namespace SWD.SAPelearning.Repository.DTO.EnrollmentDTO
{
    public class EnrollmentDTO
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int? CourseId { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public double? Price { get; set; }
        public string? Status { get; set; }
    }
}
