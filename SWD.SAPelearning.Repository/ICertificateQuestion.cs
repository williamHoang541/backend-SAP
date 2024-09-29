using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICertificateQuestion
    {
        Task<List<CertificateQuestion>> GetAllCertificateQuestion();
    }
}
