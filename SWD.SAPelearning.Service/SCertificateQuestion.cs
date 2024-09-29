using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.Models;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SCertificateQuestion : ICertificateQuestion
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningContext context;

        public SCertificateQuestion(SAPelearningContext Context, IConfiguration configuration)
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
    }
}
