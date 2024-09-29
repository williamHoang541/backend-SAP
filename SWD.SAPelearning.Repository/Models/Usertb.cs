using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class Usertb
    {
        public Usertb()
        {
            CertificateTestAttempts = new HashSet<CertificateTestAttempt>();
            Courses = new HashSet<Course>();
            Enrollments = new HashSet<Enrollment>();
            Roles = new HashSet<Role>();
        }

        public string Userid { get; set; } = null!;
        public string? Roleid { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? Fullname { get; set; }
        public string? Education { get; set; }
        public string? Phonenumber { get; set; }
        public string? Gender { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool? IsOnline { get; set; }

        public virtual ICollection<CertificateTestAttempt> CertificateTestAttempts { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
