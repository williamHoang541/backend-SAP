using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
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
    }
}
