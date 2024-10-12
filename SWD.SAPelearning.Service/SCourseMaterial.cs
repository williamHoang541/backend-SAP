using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.CourseMaterialDTO;
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


        public async Task<CourseMaterialDTO> CreateCourseMaterial(CourseMaterialDTO request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "CourseMaterialDTO cannot be null.");
            }

            var courseMaterial = new CourseMaterial
            {
                CourseId = request.CourseId,
                MaterialName = request.MaterialName,
                FileMaterial = request.FileMaterial
            };

            await context.CourseMaterials.AddAsync(courseMaterial);
            await context.SaveChangesAsync();

            return new CourseMaterialDTO
            {
                CourseId = courseMaterial.CourseId,
                MaterialName = courseMaterial.MaterialName,
                FileMaterial = courseMaterial.FileMaterial
            };
        }



        public async Task<CourseMaterialDTO?> GetCourseMaterialById(int id)
        {
            var material = await context.CourseMaterials.FindAsync(id);

            if (material == null)
            {
                return null;
            }

            return new CourseMaterialDTO
            {
                CourseId = material.CourseId,
                MaterialName = material.MaterialName,
                FileMaterial = material.FileMaterial
            };
        }

        // Update existing course material
        public async Task<CourseMaterialDTO?> UpdateCourseMaterial(int id, CourseMaterialDTO request)
        {
            var existingMaterial = await context.CourseMaterials.FindAsync(id);

            if (existingMaterial == null)
            {
                return null;
            }

            existingMaterial.CourseId = request.CourseId;
            existingMaterial.MaterialName = request.MaterialName;
            existingMaterial.FileMaterial = request.FileMaterial;

            context.CourseMaterials.Update(existingMaterial);
            await context.SaveChangesAsync();

            return new CourseMaterialDTO
            {
                CourseId = existingMaterial.CourseId,
                MaterialName = existingMaterial.MaterialName,
                FileMaterial = existingMaterial.FileMaterial
            };
        }

        // Delete course material by ID
        public async Task<bool> DeleteCourseMaterial(int id)
        {
            var existingMaterial = await context.CourseMaterials.FindAsync(id);

            if (existingMaterial == null)
            {
                return false;
            }

            context.CourseMaterials.Remove(existingMaterial);
            await context.SaveChangesAsync();

            return true;
        }
    }
}

    

