using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class CertificateTestAttempt
    {
        public CertificateTestAttempt()
        {
            CertificateSampletests = new HashSet<CertificateSampletest>();
        }

        public string AttemptId { get; set; } = null!;
        public string? Userid { get; set; }
        public DateTime AttemptDate { get; set; }
        public bool? Status { get; set; }

        public virtual Usertb? User { get; set; }
        public virtual ICollection<CertificateSampletest> CertificateSampletests { get; set; }
    }
}
