using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class Course
    {
        public Course()
        {
            CourseMaterials = new HashSet<CourseMaterial>();
            CourseSessions = new HashSet<CourseSession>();
            Enrollments = new HashSet<Enrollment>();
        }

        public string CourseId { get; set; } = null!;
        public string? Userid { get; set; }
        public string? CertificateId { get; set; }
        public string? CourseName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Mode { get; set; }
        public double? Price { get; set; }
        public int? TotalStudent { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public string? Location { get; set; }
        public string? Status { get; set; }

        public virtual Certificate? Certificate { get; set; }
        public virtual Usertb? User { get; set; }
        public virtual ICollection<CourseMaterial> CourseMaterials { get; set; }
        public virtual ICollection<CourseSession> CourseSessions { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
