using SWD.SAPelearning.Repository.DTO.CertifficateTestAttempt;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICertificateTestAttempt
    {
        Task<List<CertificateTestAttempt>> GetAllCertificateTestAttempt();
        Task<CertificateTestAttempt> CreateAttempt(CreateAttemptDTO request);
        Task<CertificateTestAttempt> UpdateAttempt(UpdateAttemptDTO request);
        Task<CertificateTestAttempt> UpdateStatusAttemptByUserId(string userId);
        Task<CertificateTestAttempt> UpdateStatusAttempt(int attemptId);
        Task<int> CountAttemptsByUserId(string userId);
        Task<bool> DeleteAttempt(RemoveADTO attemptId);
    }
}
