using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class CourseMaterial
    {
        public string MaterialId { get; set; } = null!;
        public string? CourseId { get; set; }
        public string? MaterialName { get; set; }
        public string? FileMaterial { get; set; }

        public virtual Course? Course { get; set; }
    }
}
