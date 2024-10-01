using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class CertificateQuestion
    {
        public CertificateQuestion()
        {
            CertificateTestQuestions = new HashSet<CertificateTestQuestion>();
        }

        public string Id { get; set; } = null!;
        public string? CertificateId { get; set; }
        public int? Numberofquestion { get; set; }
        public string? Name { get; set; }
        public double? Score { get; set; }
        public bool? Status { get; set; }

        public virtual Certificate? Certificate { get; set; }
        public virtual ICollection<CertificateTestQuestion> CertificateTestQuestions { get; set; }
    }
}
