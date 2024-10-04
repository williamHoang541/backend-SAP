using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class Enrollment
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int? CourseId { get; set; }
        public int? PaymentId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public double Price { get; set; }
        public string Status { get; set; } = null!;

        public virtual Course? Course { get; set; }
        public virtual Payment? Payment { get; set; }
        public virtual User? User { get; set; }
    }
}
