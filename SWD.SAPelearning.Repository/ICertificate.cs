
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICertificate
    {
        Task<List<Certificate>> GetAllCertificate();



        Task<Certificate> CreateCertificate(CertificateDTO request);
        Task<Certificate> GetCertificateById(int id);
        Task<CertificateDTO> UpdateCertificate(int id, CertificateDTO request);
        
    }
}
