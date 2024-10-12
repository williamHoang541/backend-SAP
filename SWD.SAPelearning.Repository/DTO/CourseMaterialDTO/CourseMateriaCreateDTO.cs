using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SAPelearning.Repository.DTO.CourseMaterialDTO
{
    public class CourseMateriaCreateDTO
    {
        public int? CourseId { get; set; }
        public string? MaterialName { get; set; }
        public string? FileMaterial { get; set; }
    }
}
