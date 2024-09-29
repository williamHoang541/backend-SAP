using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.Models;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SCertificateTestAttempt : ICertificateTestAttempt
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningContext context;

        public SCertificateTestAttempt(SAPelearningContext Context, IConfiguration configuration)
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
    }
}
