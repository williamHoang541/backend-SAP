using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.CertifficateTestAttempt;
using SWD.SAPelearning.Repository.DTO.SampleTest;
using SWD.SAPelearning.Repository.Models;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SCertificateSampletest : ICertificateSampletest
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningAPIContext context;

        public SCertificateSampletest(SAPelearningAPIContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }


        public async Task<List<CertificateSampleTest>> GetAllCertificateSampletest()
        {
            try
            {
                var a = await this.context.CertificateSampleTests.ToListAsync();
                return a;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<CertificateSampleTest> CreateSampleTest(CreateSampleTestDTO request)
        {
            try
            {
                // Check if the certificate exists
                var certificateExists = await this.context.Certificates
                    .AnyAsync(c => c.Id == request.CertificateId);

                if (!certificateExists)
                {
                    throw new Exception("Certificate not found.");
                }

                // Create a new sample test
                var sampleTest = new CertificateSampleTest
                {
                    CertificateId = request.CertificateId,
                    SampleTestName = request.SampleTestName,
                    Status = request.Status ?? true // Default status to true if not provided
                };

                // Save the new sample test to the database
                await this.context.CertificateSampleTests.AddAsync(sampleTest);
                await this.context.SaveChangesAsync();

                // Return the created sample test
                return sampleTest;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<bool> DeleteAttempt(RemoveSDTO sampleId)
        {
            try
            {
                if (sampleId != null)
                {
                    // Find the attempt by Attempt ID (use attemptId.AttemptId)
                    var sample = await this.context.CertificateTestAttempts
                        .Where(x => x.Id.Equals(sampleId.SampleID)) // Access AttemptId property
                        .FirstOrDefaultAsync();

                    if (sample != null) // Check if the attempt exists
                    {
                        this.context.CertificateTestAttempts.Remove(sample); // Remove the attempt
                        await this.context.SaveChangesAsync(); // Save changes
                        return true; // Return true if deletion is successful
                    }
                    return false; // Return false if attempt is not found
                }
                return false; // Return false if id is null
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting attempt: {ex.Message}");
            }
        }
    }
}
