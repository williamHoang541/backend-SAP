using SWD.SAPelearning.Repository.DTO.CertifficateTestAttempt;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICertificateTestAttempt
    {
        Task<List<CertificateTestAttempt>> GetAllCertificateTestAttempt();
        Task<CertificateTestAttempt> CreateAttemp(string userId);
        Task<CertificateTestAttempt> UpdateStatusAttemptByUserId(string userId);
        Task<CertificateTestAttempt> UpdateStatusAttempt(string attemptId);
        Task<int> CountAttemptsByUserId(string userId);
        Task<bool> DeleteAttempt(RemoveADTO attemptId);
    }
}
