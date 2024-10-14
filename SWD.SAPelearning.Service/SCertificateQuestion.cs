using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.CertificateQuestion;
using SWD.SAPelearning.Repository.Models;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SCertificateQuestion : ICertificateQuestion
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningdeployContext context;

        public SCertificateQuestion(SAPelearningdeployContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }


        public async Task<List<CertificateQuestion>> GetAllCertificateQuestion()
        {
            try
            {
                var a = await this.context.CertificateQuestions.ToListAsync();
                return a;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }

        }

        // Create a new CertificateQuestion
        public async Task<CertificateQuestion> CreateQuestion(CreateCertificateQuestionDTO request)
        {
            var question = new CertificateQuestion
            {
                TopicId = request.TopicId,
                QuestionText = request.QuestionText,
                Answer = request.Answer,
                IsCorrect = request.IsCorrect ?? false, // Default to false if not provided
                Status = request.Status ?? true         // Default to true if not provided
            };

            await this.context.CertificateQuestions.AddAsync(question);
            await this.context.SaveChangesAsync();

            return question;
        }

        // Update an existing CertificateQuestion
        public async Task<CertificateQuestion> UpdateQuestion(UpdateCertificateQuestionDTO request)
        {
            var question = await this.context.CertificateQuestions
                .FirstOrDefaultAsync(q => q.Id == request.Id);

            if (question == null)
            {
                throw new Exception("Question not found.");
            }

            question.TopicId = request.TopicId ?? question.TopicId;
            question.QuestionText = request.QuestionText ?? question.QuestionText;
            question.Answer = request.Answer ?? question.Answer;
            question.IsCorrect = request.IsCorrect ?? question.IsCorrect;
            question.Status = request.Status ?? question.Status;

            await this.context.SaveChangesAsync();

            return question;
        }

        // Delete an existing CertificateQuestion
        public async Task<bool> DeleteQuestion(int id)
        {
            var question = await this.context.CertificateQuestions
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
            {
                return false;
            }

            this.context.CertificateQuestions.Remove(question);
            await this.context.SaveChangesAsync();

            return true;
        }
    }
}

