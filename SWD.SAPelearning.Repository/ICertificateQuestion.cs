using SWD.SAPelearning.Repository.DTO.CertificateQuestion;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICertificateQuestion
    {
        Task<List<CertificateQuestion>> GetAllCertificateQuestion();
        Task<CertificateQuestion> CreateQuestion(CreateCertificateQuestionDTO request);
        Task<CertificateQuestion> UpdateQuestion(UpdateCertificateQuestionDTO request);
        Task<bool> DeleteQuestion(int id);
    }
}
