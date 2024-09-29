using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.Models;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SCourseSession : ICourseSession
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningContext context;

        public SCourseSession(SAPelearningContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }


        public async Task<List<CourseSession>> GetAllCourseSession()
        {
            try
            {
                var a = await this.context.CourseSessions.ToListAsync();
                return a;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }

        }
    }
}
