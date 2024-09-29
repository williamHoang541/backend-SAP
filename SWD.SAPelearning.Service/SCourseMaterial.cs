using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.Models;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SCourseMaterial : ICourseMaterial
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningContext context;

        public SCourseMaterial(SAPelearningContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }


        public async Task<List<CourseMaterial>> GetAllCourseMaterial()
        {
            try
            {
                var a = await this.context.CourseMaterials.ToListAsync();
                return a;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }

        }
    }
}
