using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SAPelearning.Repository.DTO.CertificateDTO
{
    public class CertificateCreateDTO
    {
        public string? CertificateName { get; set; } = null!;
        public string? Description { get; set; }
        public string? Level { get; set; }
        public string? Environment { get; set; }
        public bool? Status { get; set; }
        public List<int> ModuleIds { get; set; }
    }
}
