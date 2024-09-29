using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class CertificateTestQuestion
    {
        public string TestQuestionId { get; set; } = null!;
        public string? SampleTestId { get; set; }
        public string? QuestionId { get; set; }
        public int? Question { get; set; }
        public int? Answer { get; set; }
        public bool? Status { get; set; }

        public virtual CertificateQuestion? QuestionNavigation { get; set; }
        public virtual CertificateSampletest? SampleTest { get; set; }
    }
}
