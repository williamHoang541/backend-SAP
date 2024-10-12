using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.SapModuleDTO;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ISapModule
    {
        Task<List<SapModuleDTO>> GetAllSapModulesAsync(GetAllDTO getAllDTO);
        Task<SapModuleDTO> CreateSapModule(SapModuleCreateDTO request);
        Task<SapModuleDTO> UpdateSapModule(int id, SapModuleCreateDTO request);
        Task<bool> DeleteSapModule(int id);
        Task<SapModuleDTO> GetSapModuleById(int id);
    }
}
