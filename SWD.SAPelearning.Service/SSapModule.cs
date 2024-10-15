using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.SapModuleDTO;
using SWD.SAPelearning.Repository.Models;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SSapModule : ISapModule
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningAPIContext context;

        public SSapModule(SAPelearningAPIContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }


        public async Task<List<SapModuleDTO>> GetAllSapModulesAsync(GetAllDTO getAllDTO)
        {
            IQueryable<SapModule> query = context.SapModules.AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(getAllDTO.FilterOn) && !string.IsNullOrWhiteSpace(getAllDTO.FilterQuery))
            {
                switch (getAllDTO.FilterOn.ToLower())
                {
                    case "modulename":
                        query = query.Where(m => m.ModuleName.Contains(getAllDTO.FilterQuery));
                        break;
                    case "moduledescription":
                        query = query.Where(m => m.ModuleDescription.Contains(getAllDTO.FilterQuery));
                        break;
                    case "status":
                        if (bool.TryParse(getAllDTO.FilterQuery, out bool status))
                        {
                            query = query.Where(m => m.Status == status);
                        }
                        break;
                    default:
                        break;
                }
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(getAllDTO.SortBy))
            {
                // Handle null IsAscending by defaulting to true if it's not specified
                bool isAscending = getAllDTO.IsAscending ?? true;

                switch (getAllDTO.SortBy.ToLower())
                {
                    case "id":
                        query = isAscending
                            ? query.OrderBy(m => m.Id) // true for ascending
                            : query.OrderByDescending(m => m.Id); // false for descending
                        break;
                    case "modulename":
                        query = isAscending
                            ? query.OrderBy(m => m.ModuleName) // true for ascending
                            : query.OrderByDescending(m => m.ModuleName); // false for descending
                        break;
                    case "moduledescription":
                        query = isAscending
                            ? query.OrderBy(m => m.ModuleDescription) // true for ascending
                            : query.OrderByDescending(m => m.ModuleDescription); // false for descending
                        break;
                    case "status":
                        query = isAscending
                            ? query.OrderBy(m => m.Status) // true for ascending
                            : query.OrderByDescending(m => m.Status); // false for descending
                        break;
                    default:
                        // Default to sorting by ModuleName if the SortBy property is invalid
                        query = isAscending
                            ? query.OrderBy(m => m.ModuleName) // true for ascending
                            : query.OrderByDescending(m => m.ModuleName); // false for descending
                        break;
                }
            }

            // Execute the query and map the results to DTOs
            var sapModules = await query.ToListAsync();
            return sapModules.Select(m => new SapModuleDTO
            {
                Id = m.Id,
                ModuleName = m.ModuleName,
                ModuleDescription = m.ModuleDescription,
                Status = m.Status
            }).ToList();
        }

        public async Task<SapModuleDTO> CreateSapModule(SapModuleCreateDTO request)
        {
            try
            {
                // Validate input
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "SapModuleCreateDTO cannot be null.");
                }

                // Check if a module with the same name already exists (case-insensitive)
                var existingModule = await context.SapModules
                    .FirstOrDefaultAsync(m => m.ModuleName.ToLower() == request.ModuleName.ToLower());

                if (existingModule != null)
                {
                    throw new Exception($"A module with the name '{request.ModuleName}' already exists.");
                }

                // Map DTO to Entity
                var sapModule = new SapModule
                {
                    ModuleName = request.ModuleName,
                    ModuleDescription = request.ModuleDescription,
                    Status = request.Status ?? true // Default to active if not specified
                };

                // Add the new module to the context
                context.SapModules.Add(sapModule);
                await context.SaveChangesAsync();

                // Return the created module as a DTO
                return new SapModuleDTO
                {
                    Id = sapModule.Id,
                    ModuleName = sapModule.ModuleName,
                    ModuleDescription = sapModule.ModuleDescription,
                    Status = sapModule.Status
                };
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database update errors
                throw new Exception("There was an error saving the SapModule to the database.", dbEx);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                var errorMessage = $"{ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}";
                throw new Exception($"An error occurred while creating the SapModule: {errorMessage}");
            }
        }


        public async Task<SapModuleDTO> UpdateSapModule(int id, SapModuleCreateDTO request)
        {
            try
            {
                // Validate input
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "SapModuleCreateDTO cannot be null.");
                }

                // Find the existing module by ID
                var existingModule = await context.SapModules.FindAsync(id);
                if (existingModule == null)
                {
                    throw new Exception($"No module found with ID {id}.");
                }

                // Check if the new module name is already taken (excluding the current module)
                var duplicateModule = await context.SapModules
                    .Where(m => m.ModuleName.ToLower() == request.ModuleName.ToLower() && m.Id != id)
                    .FirstOrDefaultAsync();

                if (duplicateModule != null)
                {
                    throw new Exception($"A module with the name '{request.ModuleName}' already exists.");
                }

                // Update properties of the existing module
                existingModule.ModuleName = request.ModuleName;
                existingModule.ModuleDescription = request.ModuleDescription;
                existingModule.Status = request.Status;

                // Save changes to the database
                await context.SaveChangesAsync();

                // Return the updated module as a DTO
                return new SapModuleDTO
                {
                    Id = existingModule.Id,
                    ModuleName = existingModule.ModuleName,
                    ModuleDescription = existingModule.ModuleDescription,
                    Status = existingModule.Status
                };
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("There was an error updating the SapModule in the database.", dbEx);
            }
            catch (Exception ex)
            {
                var errorMessage = $"{ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}";
                throw new Exception($"An error occurred while updating the SapModule: {errorMessage}");
            }
        }


        public async Task<bool> DeleteSapModule(int id)
        {
            try
            {
                var existingModule = await context.SapModules.FindAsync(id);
                if (existingModule == null)
                {
                    throw new Exception($"No module found with ID {id}.");
                }

                context.SapModules.Remove(existingModule);
                await context.SaveChangesAsync();

                return true; // Module deleted successfully
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("There was an error deleting the SapModule from the database.", dbEx);
            }
            catch (Exception ex)
            {
                var errorMessage = $"{ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}";
                throw new Exception($"An error occurred while deleting the SapModule: {errorMessage}");
            }
        }
        public async Task<SapModuleDTO> GetSapModuleById(int id)
        {
            try
            {
                // Find the SapModule by ID
                var module = await context.SapModules.FindAsync(id);

                if (module == null)
                {
                    throw new Exception($"No module found with ID {id}.");
                }

                // Map the entity to the DTO
                return new SapModuleDTO
                {
                    Id = module.Id,
                    ModuleName = module.ModuleName,
                    ModuleDescription = module.ModuleDescription,
                    Status = (bool)module.Status
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the SapModule: {ex.Message}", ex);
            }
        }
    }
}
