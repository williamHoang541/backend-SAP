using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class Enrollment
    {
        public Enrollment()
        {
            Payments = new HashSet<Payment>();
        }

        public string Id { get; set; } = null!;
        public string? UserId { get; set; }
        public string? CourseId { get; set; }
        public string? PaymentId { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public string? Status { get; set; }

        public virtual Course? Course { get; set; }
        public virtual Usertb? User { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
