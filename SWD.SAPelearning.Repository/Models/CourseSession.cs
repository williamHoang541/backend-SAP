using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class CourseSession
    {
        public int Id { get; set; }
        public int? CourseId { get; set; }
        public int? InstructorId { get; set; }
        public int? TopicId { get; set; }
        public string? SessionName { get; set; }
        public string? SessionDescription { get; set; }
        public DateTime? SessionDate { get; set; }
        public bool? Status { get; set; }

        public virtual Course? Course { get; set; }
        public virtual Instructor? Instructor { get; set; }
        public virtual TopicArea? Topic { get; set; }
    }
}
