﻿using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class Enrollment
    {
        public Enrollment()
        {
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public string? UserId { get; set; }
        public int? CourseId { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public double? Price { get; set; }
        public string? Status { get; set; }

        public virtual Course? Course { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
