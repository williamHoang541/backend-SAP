using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class CertificateModule
    {
        public CertificateModule()
        {
            Certificates = new HashSet<Certificate>();
        }

        public string Id { get; set; } = null!;
        public string? ModuleName { get; set; }
        public string? ModuleDescription { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Certificate> Certificates { get; set; }
    }
}
