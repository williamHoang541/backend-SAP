
using SWD.SAPelearning.Repository.Models;
using SWD.SAPelearning.Repository.DTO.CertificateDTO;
using System.Threading.Tasks;

namespace SWD.SAPelearning.Repository
{
    public interface ICertificate
    {
        Task<List<Certificate>> GetAllCertificate();
        Task<Certificate> GetCertificateById(string id);
        Task<Certificate> CreateCertificateAsync(CreateCertificateDTO request);
    }
}
