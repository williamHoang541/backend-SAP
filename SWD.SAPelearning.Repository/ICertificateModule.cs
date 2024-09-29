using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICertificateModule
    {
        Task<List<CertificateModule>> GetAllCertificateModule();
    }
}
