using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SAPelearning.Repository.DTO.CourseSessionDTO
{




    public class CourseSessionDTO
    {
        public int? CourseId { get; set; }
        public string? CourseName { get; set; }// Foreign key for the Course
        public int? InstructorId { get; set; }
        public string? InstructorName { get; set; }
        public int? TopicId { get; set; }
        public string? TopicName { get; set; } 
        public string? SessionName { get; set; }  // Name of the session
        public string? SessionDescription { get; set; }  // Description of the session
        public DateTime? SessionDate { get; set; }  // Date of the session
        public bool? Status { get; set; }  // Status of the session (active/inactive)

        
        
    }
}


