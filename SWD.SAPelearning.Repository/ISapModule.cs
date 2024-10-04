using SWD.SAPelearning.Repository.DTO.SapModuleDTO;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ISapModule
    {
        Task<List<SapModule>> GetAllSapModule();
        //Task<List<SapModule>> GetAllSapModules(SapModuleQueryParameters queryParameters);
        Task<SapModule> CreateSapModule(SapModuleDTO request);
        Task<SapModule> UpdateSapModule(int id, SapModuleDTO request);
        Task<bool> DeleteSapModule(int id);
        Task<SapModule> GetSapModuleById(int id);
    }
}
