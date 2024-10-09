using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO;

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
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                // Call the service to get the TopicArea by ID
                var topicArea = await this.topic_area.GetTopicAreaById(id);

                // If no TopicArea is found, return a 404 Not Found
                if (topicArea == null)
                {
                    return NotFound($"TopicArea with ID {id} not found.");
                }

                // Return the TopicArea with a 200 OK status
                return Ok(topicArea);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateTopicArea([FromBody] TopicAreaDTO request)
        {
            if (request == null)
            {
                return BadRequest("TopicAreaDTO is null.");
            }

            try
            {
                var createdTopicArea = await this.topic_area.CreateTopicArea(request);
                return CreatedAtAction(nameof(CreateTopicArea), new { id = createdTopicArea.CertificateId }, createdTopicArea);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateTopicArea(int id, [FromBody] TopicAreaDTO request)
        {
            if (request == null)
            {
                return BadRequest("TopicAreaDTO is null.");
            }

            try
            {
                var updatedTopicArea = await this.topic_area.UpdateTopicArea(id, request);

                if (updatedTopicArea == null)
                {
                    return NotFound($"TopicArea with ID {id} not found.");
                }

                return Ok(updatedTopicArea); // Return the updated topic area
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/TopicArea/{id}
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTopicArea(int id)
        {
            try
            {
                var result = await this.topic_area.DeleteTopicArea(id);

                if (!result)
                {
                    return NotFound($"TopicArea with ID {id} not found or could not be deleted.");
                }

                return NoContent(); // Return 204 No Content when successfully deleted
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
