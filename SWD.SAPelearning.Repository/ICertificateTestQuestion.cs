using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICertificateTestQuestion
    {
        Task<List<CertificateTestQuestion>> GetAllCertificateTestQuestion();
    }
}
