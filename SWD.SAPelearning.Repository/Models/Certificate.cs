using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class Certificate
    {
        public Certificate()
        {
            CertificateQuestions = new HashSet<CertificateQuestion>();
            CertificateSampletests = new HashSet<CertificateSampletest>();
            Courses = new HashSet<Course>();
            Modules = new HashSet<CertificateModule>();
        }

        public string CertificateId { get; set; } = null!;
        public string? CertificateName { get; set; }
        public string? Description { get; set; }
        public string? Level { get; set; }
        public string? Environment { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<CertificateQuestion> CertificateQuestions { get; set; }
        public virtual ICollection<CertificateSampletest> CertificateSampletests { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        public virtual ICollection<CertificateModule> Modules { get; set; }
    }
}
