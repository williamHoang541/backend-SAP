using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class CertificateSampleTest
    {
        public CertificateSampleTest()
        {
            CertificateTestAttempts = new HashSet<CertificateTestAttempt>();
            CertificateTestQuestions = new HashSet<CertificateTestQuestion>();
        }

        public int Id { get; set; }
        public int? CertificateId { get; set; }
        public string? SampleTestName { get; set; }
        public bool? Status { get; set; }

        public virtual Certificate? Certificate { get; set; }
        public virtual ICollection<CertificateTestAttempt> CertificateTestAttempts { get; set; }
        public virtual ICollection<CertificateTestQuestion> CertificateTestQuestions { get; set; }
    }
}
