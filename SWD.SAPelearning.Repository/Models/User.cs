using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class User
    {
        public User()
        {
            CertificateTestAttempts = new HashSet<CertificateTestAttempt>();
            Enrollments = new HashSet<Enrollment>();
            Instructors = new HashSet<Instructor>();
        }

        public string Id { get; set; } = null!;
        public string? Username { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public string? Password { get; set; } = null!;
        public DateTime? RegistrationDate { get; set; }
        public string? Role { get; set; }
        public string? Fullname { get; set; }
        public string? Education { get; set; }
        public string? Phonenumber { get; set; }
        public string? Gender { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool? IsOnline { get; set; }

        public virtual ICollection<CertificateTestAttempt> CertificateTestAttempts { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Instructor> Instructors { get; set; }
    }
}
