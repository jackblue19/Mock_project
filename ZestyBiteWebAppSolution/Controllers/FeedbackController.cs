using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace ZestyBiteWebAppSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // GET: api/feedback/item/{itemId}
        [HttpGet("item/{itemId}")]
        public async Task<IActionResult> GetFeedbacksByItemId(int itemId)
        {
            try
            {
                var feedbacks = await _feedbackService.GetFeedbacksByItemIdAsync(itemId);
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while retrieving feedbacks for item {itemId}: {ex.Message}");
            }
        }

        // POST: api/feedback
        [HttpPost]
        public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackDTO feedbackDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var submittedFeedback = await _feedbackService.SubmitFeedbackAsync(feedbackDto);
                return CreatedAtAction(nameof(GetFeedbacksByItemId), new { itemId = submittedFeedback.ItemId }, submittedFeedback);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while submitting feedback: {ex.Message}");
            }
        }

        // GET: api/feedback/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            try
            {
                var feedbacks = await _feedbackService.GetAllFeedbacksAsync();
                return Ok(feedbacks);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while retrieving all feedbacks: {ex.Message}");
            }
        }

        // GET: api/feedback
        [HttpGet("allpage")]
        public async Task<IActionResult> GetFeedbacks(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var feedbacks = await _feedbackService.GetAllFeedbacksAsync(pageNumber, pageSize);
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while retrieving feedbacks: {ex.Message}");
            }
        }

        // DELETE: api/feedback/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            try
            {
                var result = await _feedbackService.DeleteFeedbackAsync(id);
                if (result)
                {
                    return NoContent();
                }
                return NotFound(new { Message = "Feedback not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while deleting feedback: {ex.Message}");
            }
        }
    }
}
