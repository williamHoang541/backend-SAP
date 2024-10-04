using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICertificateSampletest
    {
        Task<List<CertificateSampleTest>> GetAllCertificateSampletest();
    }
}
