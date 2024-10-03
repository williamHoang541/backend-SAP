using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class Certificate
    {
        public Certificate()
        {
            CertificateSampleTests = new HashSet<CertificateSampleTest>();
            Courses = new HashSet<Course>();
            TopicAreas = new HashSet<TopicArea>();
            Modules = new HashSet<SapModule>();
        }

        public int Id { get; set; }
        public string CertificateName { get; set; } = null!;
        public string? Description { get; set; }
        public string? Level { get; set; }
        public string? Environment { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<CertificateSampleTest> CertificateSampleTests { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<TopicArea> TopicAreas { get; set; }

        public virtual ICollection<SapModule> Modules { get; set; }
    }
}
