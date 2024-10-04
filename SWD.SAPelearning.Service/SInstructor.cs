using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Service
{
    public class SInstructor : IInstructor
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningContext context;

        public SInstructor(SAPelearningContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }

        public async Task<List<Instructor>> GetAllInstructor()
        {
            try
            {
                var a = await this.context.Instructors.ToListAsync();
                return a;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }

        }
    }
}
