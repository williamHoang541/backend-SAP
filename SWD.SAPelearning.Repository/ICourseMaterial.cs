using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.CourseMaterialDTO;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICourseMaterial
    {
        Task<List<CourseMaterialDTO>> GetAllCourseMaterialsAsync(GetAllDTO getAllDTO);
        Task<CourseMaterialDTO> CreateCourseMaterial(CourseMateriaCreateDTO request);
        Task<CourseMaterialDTO?> GetCourseMaterialById(int id);
        Task<CourseMaterialDTO?> UpdateCourseMaterial(int id, CourseMateriaCreateDTO request);
        Task<bool> DeleteCourseMaterial(int id);
    }
}
