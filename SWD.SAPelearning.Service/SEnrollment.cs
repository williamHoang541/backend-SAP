using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.Models;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SEnrollment : IEnrollment
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningContext context;

        public SEnrollment(SAPelearningContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }


        public async Task<List<Enrollment>> GetAllEnrollment()
        {
            try
            {
                var a = await this.context.Enrollments.ToListAsync();
                return a;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }

        }
    }
}
