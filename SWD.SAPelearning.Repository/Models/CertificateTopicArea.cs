using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class CertificateTopicArea
    {
        public CertificateTopicArea()
        {
            CertificateQuestions = new HashSet<CertificateQuestion>();
            CourseSessions = new HashSet<CourseSession>();
        }

        public int Id { get; set; }
        public int? CertificateId { get; set; }
        public string? TopicName { get; set; }
        public bool? Status { get; set; }

        public virtual Certificate? Certificate { get; set; }
        public virtual ICollection<CertificateQuestion> CertificateQuestions { get; set; }
        public virtual ICollection<CourseSession> CourseSessions { get; set; }
    }
}
