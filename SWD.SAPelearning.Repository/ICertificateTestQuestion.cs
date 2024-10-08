using SWD.SAPelearning.Repository.DTO.TestQuestion;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICertificateTestQuestion
    {
        Task<List<CertificateTestQuestion>> GetAllCertificateTestQuestion();
        Task<CertificateTestQuestion> CreateTestQuestion(CreateTestQuestionDTO request);
        Task<CertificateTestQuestion> UpdateTestQuestion(UpdateTestQuestionDTO request);
        Task<bool> DeleteTestQuestion(int id);
    }
}
