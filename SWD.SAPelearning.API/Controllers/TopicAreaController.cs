using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.TopicAreaDTO;

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
        public async Task<IActionResult> GetAll([FromQuery] GetAllDTO getAllDTO)
        {
            try
            {
                var topicAreas = await this.topic_area.GetAllTopicAreasAsync(getAllDTO);
                return Ok(topicAreas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
        public async Task<IActionResult> CreateTopicArea([FromBody] TopicAreaCreateDTO request)
        {
            if (request == null)
            {
                return BadRequest("TopicAreaCreateDTO is null.");
            }

            try
            {
                var createdTopicArea = await this.topic_area.CreateTopicArea(request);
                return CreatedAtAction(nameof(GetById), new { id = createdTopicArea.Id }, createdTopicArea);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateTopicArea(int id, [FromBody] TopicAreaCreateDTO request)
        {
            if (request == null)
            {
                return BadRequest("TopicAreaCreateDTO is null.");
            }

            try
            {
                var updatedTopicArea = await this.topic_area.UpdateTopicArea(id, request);

                if (updatedTopicArea == null)
                {
                    return NotFound($"TopicArea with ID {id} not found.");
                }

                return Ok(updatedTopicArea);
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
