using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SAPelearning.Service
{
    public class STopicArea : ITopicArea
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningContext context;

        public STopicArea(SAPelearningContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }

        public async Task<List<TopicArea>> GetAllTopicArea()
        {
            try
            {
                var a = await this.context.TopicAreas.ToListAsync();
                return a;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }

        }
    }
}
