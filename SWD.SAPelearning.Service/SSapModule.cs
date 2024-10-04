using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.SapModuleDTO;
using SWD.SAPelearning.Repository.Models;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SSapModule : ISapModule
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningContext context;

        public SSapModule(SAPelearningContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }


        public async Task<List<SapModule>> GetAllSapModule()
        {
            try
            {
                var a = await this.context.SapModules.ToListAsync();
                return a;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }

        }

        public async Task<SapModule> CreateSapModule(SapModuleDTO request)
        {
            try
            {
                // Validate input
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "SapModuleDTO cannot be null.");
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
                    Status = request.Status
                };

                // Add new module to the context
                await context.SapModules.AddAsync(sapModule);
                await context.SaveChangesAsync();

                // Return the newly created module
                return sapModule;
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


        public async Task<SapModule> UpdateSapModule(int id, SapModuleDTO request)
        {
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "SapModuleDTO cannot be null.");
                }

                var existingModule = await context.SapModules.FindAsync(id);
                if (existingModule == null)
                {
                    throw new Exception($"No module found with ID {id}.");
                }

                // Check if the module name is already taken (excluding the current module)
                var duplicateModule = await context.SapModules
                    .Where(m => m.ModuleName.ToLower() == request.ModuleName.ToLower() && m.Id != id)
                    .FirstOrDefaultAsync();

                if (duplicateModule != null)
                {
                    throw new Exception($"A module with the name '{request.ModuleName}' already exists.");
                }

                // Update the properties
                existingModule.ModuleName = request.ModuleName;
                existingModule.ModuleDescription = request.ModuleDescription;
                existingModule.Status = request.Status;

                // Save changes
                await context.SaveChangesAsync();

                return existingModule;
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
        public async Task<SapModule> GetSapModuleById(int id)
        {
            try
            {
                var module = await context.SapModules.FindAsync(id);
                if (module == null)
                {
                    throw new Exception($"No module found with ID {id}.");
                }
                return module;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the SapModule: {ex.Message}", ex);
            }
        }


    }
}
