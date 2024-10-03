using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class SapModule
    {
        public SapModule()
        {
            Certificates = new HashSet<Certificate>();
        }

        public int Id { get; set; }
        public string? ModuleName { get; set; }
        public string? ModuleDescription { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<Certificate> Certificates { get; set; }
    }
}
