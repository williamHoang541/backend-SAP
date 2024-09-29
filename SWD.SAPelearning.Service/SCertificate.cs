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
    }
}
