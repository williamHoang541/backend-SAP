using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.TestQuestion;
using SWD.SAPelearning.Repository.Models;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SCertificateTestQuestion : ICertificateTestQuestion
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningAPIContext context;

        public SCertificateTestQuestion(SAPelearningAPIContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }


        public async Task<List<CertificateTestQuestion>> GetAllCertificateTestQuestion()
        {
            try
            {
                var a = await this.context.CertificateTestQuestions.ToListAsync();
                return a;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }

        }

        public async Task<CertificateTestQuestion> CreateTestQuestion(CreateTestQuestionDTO request)
        {
            var testQuestion = new CertificateTestQuestion
            {
                SampleTestId = request.SampleTestId,
                QuestionId = request.QuestionId,
                DisplayInTest = request.DisplayInTest,
                Status = request.Status ?? true // Default to true if not provided
            };

            await this.context.CertificateTestQuestions.AddAsync(testQuestion);
            await this.context.SaveChangesAsync();

            return testQuestion;
        }

        // Update an existing CertificateTestQuestion
        public async Task<CertificateTestQuestion> UpdateTestQuestion(UpdateTestQuestionDTO request)
        {
            var testQuestion = await this.context.CertificateTestQuestions
                .FirstOrDefaultAsync(tq => tq.Id == request.Id);

            if (testQuestion == null)
            {
                throw new Exception("Test question not found.");
            }

            testQuestion.SampleTestId = request.SampleTestId ?? testQuestion.SampleTestId;
            testQuestion.QuestionId = request.QuestionId ?? testQuestion.QuestionId;
            testQuestion.DisplayInTest = request.DisplayInTest ?? testQuestion.DisplayInTest;
            testQuestion.Status = request.Status ?? testQuestion.Status;

            await this.context.SaveChangesAsync();

            return testQuestion;
        }

        // Delete an existing CertificateTestQuestion
        public async Task<bool> DeleteTestQuestion(int id)
        {
            var testQuestion = await this.context.CertificateTestQuestions
                .FirstOrDefaultAsync(tq => tq.Id == id);

            if (testQuestion == null)
            {
                return false;
            }

            this.context.CertificateTestQuestions.Remove(testQuestion);
            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
