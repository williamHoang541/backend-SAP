using SWD.SAPelearning.Repository.DTO.CourseMaterialDTO;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICourseMaterial
    {
        Task<List<CourseMaterial>> GetAllCourseMaterial();
        Task<CourseMaterialDTO> CreateCourseMaterial(CourseMaterialDTO request);
        Task<CourseMaterialDTO?> GetCourseMaterialById(int id);
        Task<CourseMaterialDTO?> UpdateCourseMaterial(int id, CourseMaterialDTO request);
        Task<bool> DeleteCourseMaterial(int id);
    }
}
