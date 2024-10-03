using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class CertificateTestQuestion
    {
        public int Id { get; set; }
        public int? SampleTestId { get; set; }
        public int? QuestionId { get; set; }
        public int? DisplayInTest { get; set; }
        public bool? Status { get; set; }

        public virtual CertificateQuestion? Question { get; set; }
        public virtual CertificateSampleTest? SampleTest { get; set; }
    }
}
