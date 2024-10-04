using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class CourseMaterial
    {
        public int Id { get; set; }
        public int? CourseId { get; set; }
        public string? MaterialName { get; set; }
        public string? FileMaterial { get; set; }

        public virtual Course? Course { get; set; }
    }
}
