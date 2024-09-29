using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICourseMaterial
    {
        Task<List<CourseMaterial>> GetAllCourseMaterial();
    }
}
