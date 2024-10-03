using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class CertificateTestAttempt
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int? SampleTestId { get; set; }
        public DateTime? AttemptDate { get; set; }
        public double? Score { get; set; }
        public int? CorrectAnswers { get; set; }
        public int? TotalAnswers { get; set; }
        public bool? Status { get; set; }

        public virtual CertificateSampleTest? SampleTest { get; set; }
        public virtual Usertb? User { get; set; }
    }
}
