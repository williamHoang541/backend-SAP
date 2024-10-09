using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.Models;


namespace SWD.SAPelearning.Services
{
    public class SCertificate : ICertificate
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningContext context;

        public SCertificate(SAPelearningContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }


        public async Task<List<Certificate>> GetAllCertificate()
        {
            try
            {
                var a = await this.context.Certificates.ToListAsync();
                return a;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }

        }

        public async Task<Certificate> CreateCertificate(CertificateDTO request)
        {
            try
            {
                // Validate input
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "CertificateDTO cannot be null.");
                }

                // Check if a certificate with the same name already exists (case-insensitive)
                var existingCertificate = await context.Certificates
                    .FirstOrDefaultAsync(c => c.CertificateName.ToLower() == request.CertificateName.ToLower());

                if (existingCertificate != null)
                {
                    throw new Exception($"A certificate with the name '{request.CertificateName}' already exists.");
                }

                // Map DTO to Entity
                var certificate = new Certificate
                {
                    CertificateName = request.CertificateName,
                    Description = request.Description,
                    Level = request.Level,
                    Environment = request.Environment,
                    Status = request.Status,
                    Modules = new List<SapModule>() // Initialize the modules list
                };

                // Add modules based on the provided ModuleIds
                if (request.ModuleIds != null && request.ModuleIds.Count > 0)
                {
                    foreach (var moduleId in request.ModuleIds)
                    {
                        var module = await context.SapModules.FindAsync(moduleId);
                        if (module != null)
                        {
                            // If module status is False, set it to True
                            if (module.Status == false)
                            {
                                module.Status = true;
                            }

                            certificate.Modules.Add(module); // Associate the module with the certificate
                        }
                        else
                        {
                            throw new Exception($"Module with ID {moduleId} does not exist.");
                        }
                    }
                }

                // Add new certificate to the context
                await context.Certificates.AddAsync(certificate);
                await context.SaveChangesAsync();

                // Return the newly created certificate
                return certificate;
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database update errors
                throw new Exception("There was an error saving the Certificate to the database.", dbEx);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                var errorMessage = $"{ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}";
                throw new Exception($"An error occurred while creating the Certificate: {errorMessage}");
            }
        }

        public async Task<CertificateDTO> UpdateCertificate(int id, CertificateDTO request)
        {
            try
            {
                var existingCertificate = await context.Certificates.Include(c => c.Modules).FirstOrDefaultAsync(c => c.Id == id);
                if (existingCertificate == null)
                {
                    throw new Exception($"No certificate found with ID {id}.");
                }

                // Update properties
                existingCertificate.CertificateName = request.CertificateName;
                existingCertificate.Description = request.Description;
                existingCertificate.Level = request.Level;
                existingCertificate.Environment = request.Environment;
                existingCertificate.Status = request.Status;

                // Update modules if provided
                if (request.ModuleIds != null && request.ModuleIds.Count > 0)
                {
                    existingCertificate.Modules.Clear(); // Clear existing modules

                    foreach (var moduleId in request.ModuleIds)
                    {
                        var module = await context.SapModules.FindAsync(moduleId);
                        if (module != null)
                        {
                            existingCertificate.Modules.Add(module); // Associate the module with the certificate
                        }
                        else
                        {
                            throw new Exception($"Module with ID {moduleId} does not exist.");
                        }
                    }
                }

                await context.SaveChangesAsync();

                // Map the updated certificate back to DTO
                return new CertificateDTO
                {
                    CertificateName = existingCertificate.CertificateName,
                    Description = existingCertificate.Description,
                    Level = existingCertificate.Level,
                    Environment = existingCertificate.Environment,
                    Status = existingCertificate.Status,
                    ModuleIds = existingCertificate.Modules.Select(m => m.Id).ToList() // Assuming you want to return ModuleIds as well
                };
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("There was an error updating the Certificate in the database.", dbEx);
            }
            catch (Exception ex)
            {
                var errorMessage = $"{ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}";
                throw new Exception($"An error occurred while updating the Certificate: {errorMessage}");
            }
        }








        public async Task<CertificateDTO> GetCertificateById(int id)
        {
            try
            {
                var certificate = await context.Certificates
                    .Include(c => c.Modules) // Include related modules
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (certificate == null)
                {
                    throw new Exception($"No certificate found with ID {id}.");
                }

                // Return the DTO with module IDs
                return new CertificateDTO
                {
                    CertificateName = certificate.CertificateName,
                    Description = certificate.Description,
                    Level = certificate.Level,
                    Environment = certificate.Environment,
                    Status = certificate.Status,
                    ModuleIds = certificate.Modules.Select(m => m.Id).ToList() // Extract module IDs
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the certificate: {ex.Message}", ex);
            }
        }







        public async Task<bool> DeleteCertificate(int id)
        {
            try
            {
                // Retrieve the certificate along with its related entities
                var existingCertificate = await context.Certificates
                    .Include(c => c.Modules)
                    .Include(c => c.Courses)
                    .Include(c => c.CertificateSampleTests)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (existingCertificate == null)
                {
                    throw new Exception($"No certificate found with ID {id}.");
                }

                // Soft delete related courses if they exist
                if (existingCertificate.Courses != null && existingCertificate.Courses.Any())
                {
                    foreach (var course in existingCertificate.Courses)
                    {
                        course.Status = false; // Assuming `Status` is a bool in the Course entity
                    }
                }

                // Soft delete related sample tests if they exist
                if (existingCertificate.CertificateSampleTests != null && existingCertificate.CertificateSampleTests.Any())
                {
                    foreach (var sampleTest in existingCertificate.CertificateSampleTests)
                    {
                        sampleTest.Status = false; // Assuming `Status` is a bool in the SampleTest entity
                    }
                }

                // Soft delete related modules if no other certificates are associated
                if (existingCertificate.Modules != null && existingCertificate.Modules.Any())
                {
                    foreach (var module in existingCertificate.Modules)
                    {
                        var moduleCertificates = await context.Certificates
                            .Where(c => c.Modules.Any(m => m.Id == module.Id) && c.Id != id)
                            .ToListAsync();

                        // Only soft delete the module if no other certificate is using it
                        if (!moduleCertificates.Any())
                        {
                            module.Status = false; // Set module status to false
                        }
                    }
                }

                // Delete the certificate from the database
                context.Certificates.Remove(existingCertificate);
                await context.SaveChangesAsync();

                return true; // Certificate deleted successfully
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("There was an error deleting the Certificate from the database.", dbEx);
            }
            catch (Exception ex)
            {
                var errorMessage = $"{ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}";
                throw new Exception($"An error occurred while deleting the Certificate: {errorMessage}");
            }
        }



    }
}
