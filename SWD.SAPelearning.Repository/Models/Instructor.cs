using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class Instructor
    {
        public Instructor()
        {
            CourseSessions = new HashSet<CourseSession>();
            Courses = new HashSet<Course>();
        }

        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public string? Phonenumber { get; set; }
        public bool? Status { get; set; }

        public virtual Usertb? User { get; set; }
        public virtual ICollection<CourseSession> CourseSessions { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
