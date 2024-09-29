using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class CertificateSampletest
    {
        public CertificateSampletest()
        {
            CertificateTestQuestions = new HashSet<CertificateTestQuestion>();
        }

        public string SampleTestId { get; set; } = null!;
        public string? AttemptId { get; set; }
        public string? CertificateId { get; set; }
        public string? SampleTestName { get; set; }
        public bool? Status { get; set; }

        public virtual CertificateTestAttempt? Attempt { get; set; }
        public virtual Certificate? Certificate { get; set; }
        public virtual ICollection<CertificateTestQuestion> CertificateTestQuestions { get; set; }
    }
}
