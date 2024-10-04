using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ISapModule
    {
        Task<List<SapModule>> GetAllSapModule();
    }
}
