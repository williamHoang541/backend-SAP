using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.CertifficateTestAttempt;
using SWD.SAPelearning.Repository.Models;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SCertificateTestAttempt : ICertificateTestAttempt
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningAPIContext context;

        public SCertificateTestAttempt(SAPelearningAPIContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }


        public async Task<List<CertificateTestAttempt>> GetAllCertificateTestAttempt()
        {
            try
            {
                var a = await this.context.CertificateTestAttempts.ToListAsync();
                return a;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<CertificateTestAttempt> CreateAttempt(CreateAttemptDTO request)
        {
            try
            {
                // Create a new attempt
                var attempt = new CertificateTestAttempt
                {
                    UserId = request.UserId,
                    SampleTestId = request.SampleTestId,
                    AttemptDate = DateTime.Now,   // Current date and time
                    Score = 0,               // Set the score
                    CorrectAnswers = 0, // Set the number of correct answers
                    TotalAnswers = 80,  // Set the total number of answers
                    Status = true                 // Set the status to true
                };

                // Save the new attempt to the database
                await this.context.CertificateTestAttempts.AddAsync(attempt);
                await this.context.SaveChangesAsync();

                // Return the created attempt
                return attempt;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<CertificateTestAttempt> UpdateAttempt(UpdateAttemptDTO request)
        {
            try
            {
                // Find the existing attempt by its Id
                var attempt = await this.context.CertificateTestAttempts
                    .Where(a => a.Id == request.AttemptId)
                    .FirstOrDefaultAsync();

                if (attempt == null)
                {
                    throw new Exception("Attempt not found");
                }

                // Update the score if provided
                if (request.Score.HasValue)
                {
                    attempt.Score = request.Score.Value;
                }

                // Update the correct answers if provided
                if (request.CorrectAnswers.HasValue)
                {
                    attempt.CorrectAnswers = request.CorrectAnswers.Value;
                }

                // Update the attempt in the database
                this.context.CertificateTestAttempts.Update(attempt);
                await this.context.SaveChangesAsync();

                // Return the updated attempt
                return attempt;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<CertificateTestAttempt> UpdateStatusAttemptByUserId(string userId)
        {
            try
            {
                // Find the latest attempt for the given user ID
                var attempt = await this.context.CertificateTestAttempts
                    .Where(a => a.UserId == userId) 
                    .OrderByDescending(a => a.AttemptDate) 
                    .FirstOrDefaultAsync();

                if (attempt == null)
                {
                    throw new Exception("Certificate test attempt not found for the given user ID");
                }

                // Set the Status to false
                attempt.Status = false;

                // Update the attempt's record
                this.context.CertificateTestAttempts.Update(attempt);
                await this.context.SaveChangesAsync();

                return attempt; // Return the updated attempt
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<CertificateTestAttempt> UpdateStatusAttempt(int attemptId)
        {
            try
            {
                // Find the attempt by attemptId
                var attempt = await this.context.CertificateTestAttempts
                    .Where(a => a.Id == attemptId)
                    .FirstOrDefaultAsync();

                if (attempt == null)
                {
                    throw new Exception("Certificate test attempt not found");
                }

                // Toggle the Status (assuming it's a boolean where true = passed, false = failed)
                attempt.Status = false;

                // Update the attempt's record
                this.context.CertificateTestAttempts.Update(attempt);
                await this.context.SaveChangesAsync();

                return attempt;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating status: {ex.Message}");
            }
        }

        public async Task<int> CountAttemptsByUserId(string userId)
        {
            try
            {
                // Count the number of attempts for the given user ID
                var count = await this.context.CertificateTestAttempts
                    .CountAsync(a => a.UserId == userId);

                return count; // Return the count
            }
            catch (Exception ex)
            {
                throw new Exception($"Error counting attempts: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAttempt(RemoveADTO attemptId)
        {
            try
            {
                if (attemptId != null)
                {
                    // Find the attempt by Attempt ID (use attemptId.AttemptId)
                    var attempt = await this.context.CertificateTestAttempts
                        .Where(x => x.Id.Equals(attemptId.AttemptID)) // Access AttemptId property
                        .FirstOrDefaultAsync();

                    if (attempt != null) // Check if the attempt exists
                    {
                        this.context.CertificateTestAttempts.Remove(attempt); // Remove the attempt
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
