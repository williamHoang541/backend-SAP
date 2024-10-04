using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;

namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicAreaController : ControllerBase
    {
        private readonly ITopicArea topic_area;

        public TopicAreaController(ITopicArea topic_area)
        {
            this.topic_area = topic_area;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.topic_area.GetAllTopicArea();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }
    }
}
