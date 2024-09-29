
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICertificate
    {
        Task<List<Certificate>> GetAllCertificate();
    }
}
