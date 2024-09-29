using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICertificateTestAttempt
    {
        Task<List<CertificateTestAttempt>> GetAllCertificateTestAttempt();
    }
}
