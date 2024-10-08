using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SAPelearning.Repository.DTO.TestQuestion
{
    public class CreateTestQuestionDTO
    {
        public int? SampleTestId { get; set; }
        public int? QuestionId { get; set; }
        public int? DisplayInTest { get; set; }
        public bool? Status { get; set; }
    }
}
