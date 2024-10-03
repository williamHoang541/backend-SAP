using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class CertificateQuestion
    {
        public CertificateQuestion()
        {
            CertificateTestQuestions = new HashSet<CertificateTestQuestion>();
        }

        public int Id { get; set; }
        public int? TopicId { get; set; }
        public string? QuestionText { get; set; }
        public string? Answer { get; set; }
        public bool? IsCorrect { get; set; }
        public bool? Status { get; set; }

        public virtual TopicArea? Topic { get; set; }
        public virtual ICollection<CertificateTestQuestion> CertificateTestQuestions { get; set; }
    }
}
