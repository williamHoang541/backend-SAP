using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.CourseMaterialDTO;
using SWD.SAPelearning.Repository.Models;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SCourseMaterial : ICourseMaterial
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningdeployContext context;

        public SCourseMaterial(SAPelearningdeployContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }


        public async Task<List<CourseMaterialDTO>> GetAllCourseMaterialsAsync(GetAllDTO getAllDTO)
        {
            IQueryable<CourseMaterial> query = context.CourseMaterials
                .Include(cm => cm.Course) // Assuming there's a relationship with Course entity
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(getAllDTO.FilterOn) && !string.IsNullOrWhiteSpace(getAllDTO.FilterQuery))
            {
                switch (getAllDTO.FilterOn.ToLower())
                {
                    case "courseid":
                        if (int.TryParse(getAllDTO.FilterQuery, out int courseId))
                        {
                            query = query.Where(cm => cm.CourseId == courseId);
                        }
                        break;
                    case "coursename":
                        query = query.Where(cm => cm.Course.CourseName.Contains(getAllDTO.FilterQuery));
                        break;
                    case "materialname":
                        query = query.Where(cm => cm.MaterialName.Contains(getAllDTO.FilterQuery));
                        break;
                    case "filematerial":
                        query = query.Where(cm => cm.FileMaterial.Contains(getAllDTO.FilterQuery));
                        break;
                    default:
                        break;
                }
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(getAllDTO.SortBy))
            {
                bool isAscending = getAllDTO.IsAscending ?? true;

                switch (getAllDTO.SortBy.ToLower())
                {
                    case "courseid":
                        query = isAscending
                            ? query.OrderBy(cm => cm.CourseId)
                            : query.OrderByDescending(cm => cm.CourseId);
                        break;
                    case "coursename":
                        query = isAscending
                            ? query.OrderBy(cm => cm.Course.CourseName)
                            : query.OrderByDescending(cm => cm.Course.CourseName);
                        break;
                    case "materialname":
                        query = isAscending
                            ? query.OrderBy(cm => cm.MaterialName)
                            : query.OrderByDescending(cm => cm.MaterialName);
                        break;
                    case "filematerial":
                        query = isAscending
                            ? query.OrderBy(cm => cm.FileMaterial)
                            : query.OrderByDescending(cm => cm.FileMaterial);
                        break;
                    default:
                        // Default to sorting by MaterialName if no valid SortBy is provided
                        query = isAscending
                            ? query.OrderBy(cm => cm.MaterialName)
                            : query.OrderByDescending(cm => cm.MaterialName);
                        break;
                }
            }

            // Pagination
            int pageNumber = getAllDTO.PageNumber ?? 1;
            int pageSize = getAllDTO.PageSize ?? 10;
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Fetch and return course materials
            var courseMaterials = await query
                .Select(cm => new CourseMaterialDTO
                {
                    Id = cm.Id,
                    CourseId = cm.CourseId,
                    
                    MaterialName = cm.MaterialName,
                    FileMaterial = cm.FileMaterial
                })
                .ToListAsync();

            return courseMaterials;
        }


        public async Task<CourseMaterialDTO> CreateCourseMaterial(CourseMateriaCreateDTO request)
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
                Id =courseMaterial.Id,
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
                Id = material.Id,
                CourseId = material.CourseId,
                MaterialName = material.MaterialName,
                FileMaterial = material.FileMaterial
            };
        }

        // Update existing course material
        public async Task<CourseMaterialDTO?> UpdateCourseMaterial(int id, CourseMateriaCreateDTO request)
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
            {Id = existingMaterial.Id,
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

    

