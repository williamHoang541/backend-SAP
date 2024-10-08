using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SAPelearning.Repository.DTO.CertificateQuestion
{
    public class UpdateCertificateQuestionDTO
    {
        public int Id { get; set; } // ID is required for update
        public int? TopicId { get; set; }
        public string? QuestionText { get; set; }
        public string? Answer { get; set; }
        public bool? IsCorrect { get; set; }
        public bool? Status { get; set; }
    }
}
