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
            try
            {
                // Validate input
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "CourseMaterialDTO cannot be null.");
                }

                // Map DTO to Entity
                var courseMaterial = new CourseMaterial
                {
                    CourseId = request.CourseId, // Correctly assigning CourseId from DTO
                    MaterialName = request.MaterialName,
                    FileMaterial = request.FileMaterial
                };

                // Add new course material to the context
                await context.CourseMaterials.AddAsync(courseMaterial);
                await context.SaveChangesAsync();

                // Return the newly created course material as DTO
                return new CourseMaterialDTO
                {
                    Id = courseMaterial.Id,
                    CourseId = courseMaterial.CourseId, // Correctly assigning CourseId
                    MaterialName = courseMaterial.MaterialName,
                    FileMaterial = courseMaterial.FileMaterial
                };
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("There was an error saving the CourseMaterial to the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating the CourseMaterial: {ex.Message}", ex);
            }
        }

        public async Task<CourseMaterialDTO> UpdateCourseMaterial(int id, CourseMaterialDTO request)
        {
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "CourseMaterialDTO cannot be null.");
                }

                var existingMaterial = await context.CourseMaterials.FindAsync(id);
                if (existingMaterial == null)
                {
                    throw new Exception($"No course material found with ID {id}.");
                }

                // Update the properties
                existingMaterial.CourseId = request.CourseId; // Correctly updating CourseId
                existingMaterial.MaterialName = request.MaterialName;
                existingMaterial.FileMaterial = request.FileMaterial;

                // Save changes
                await context.SaveChangesAsync();

                return new CourseMaterialDTO
                {
                    Id = existingMaterial.Id,
                    CourseId = existingMaterial.CourseId, // Correctly returning CourseId
                    MaterialName = existingMaterial.MaterialName,
                    FileMaterial = existingMaterial.FileMaterial
                };
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("There was an error updating the CourseMaterial in the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the CourseMaterial: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteCourseMaterial(int id)
        {
            try
            {
                var existingMaterial = await context.CourseMaterials.FindAsync(id);
                if (existingMaterial == null)
                {
                    throw new Exception($"No course material found with ID {id}.");
                }

                context.CourseMaterials.Remove(existingMaterial);
                await context.SaveChangesAsync();

                return true; // Material deleted successfully
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("There was an error deleting the CourseMaterial from the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the CourseMaterial: {ex.Message}", ex);
            }
        }

        public async Task<CourseMaterialDTO> GetCourseMaterialById(int id)
        {
            try
            {
                var material = await context.CourseMaterials.FindAsync(id);
                if (material == null)
                {
                    throw new Exception($"No course material found with ID {id}.");
                }

                return new CourseMaterialDTO
                {
                    Id = material.Id,
                    CourseId = material.CourseId, // Correctly returning CourseId
                    MaterialName = material.MaterialName,
                    FileMaterial = material.FileMaterial
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the CourseMaterial: {ex.Message}", ex);
            }
        }

        public async Task<List<CourseMaterialDTO>> GetMaterialsByCourseId(int courseId)
        {
            try
            {
                var materials = await context.CourseMaterials
                    .Where(m => m.CourseId == courseId) // Correctly filtering by CourseId
                    .ToListAsync();

                return materials.Select(m => new CourseMaterialDTO
                {
                    Id = m.Id,
                    CourseId = m.CourseId, // Correctly returning CourseId
                    MaterialName = m.MaterialName,
                    FileMaterial = m.FileMaterial
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving materials for course ID {courseId}: {ex.Message}", ex);
            }
        }
    }
}
    

