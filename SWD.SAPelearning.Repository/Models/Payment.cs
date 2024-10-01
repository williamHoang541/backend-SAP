using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class Payment
    {
        public string Id { get; set; } = null!;
        public string? EnrollmentId { get; set; }
        public double? Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? TransactionId { get; set; }
        public string? Status { get; set; }

        public virtual Enrollment? Enrollment { get; set; }
    }
}
