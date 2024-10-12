
using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.CertificateDTO;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICertificate
    {
        Task<List<CertificateDTO>> GetAllCertificateAsync(GetAllDTO getAllDTO);
        Task<Certificate> CreateCertificate(CertificateCreateDTO request);
        Task<CertificateDTO> GetCertificateById(int id);
        Task<CertificateDTO> UpdateCertificate(int id, CertificateCreateDTO request);
        Task<bool> DeleteCertificate(int id);

    }
}
