

namespace SWD.SAPelearning.Repository.DTO.CertifficateTestAttempt
{
    public class UpdateAttemptDTO
    {
        public int AttemptId { get; set; } 
        public double? Score { get; set; } 
        public int? CorrectAnswers { get; set; } 
    }
}
