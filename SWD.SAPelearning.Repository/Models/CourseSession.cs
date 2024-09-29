using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class CourseSession
    {
        public string SessionId { get; set; } = null!;
        public string? CourseId { get; set; }
        public string? SessionName { get; set; }
        public string? SessionDescription { get; set; }
        public DateTime SessionDate { get; set; }
        public string? Status { get; set; }

        public virtual Course? Course { get; set; }
    }
}
