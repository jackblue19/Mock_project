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
        public async Task<IActionResult> GetFeedbacksItemId(int itemId)
        {
            try
            {
                var feedbacks = await _feedbackService.GetFeedbacksByItemAsync(itemId);
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
                return CreatedAtAction(nameof(GetFeedbacksItemId), new { itemId = submittedFeedback.ItemId }, submittedFeedback);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while submitting feedback: {ex.Message}");
            }
        }

        // GET: api/feedback/all
        [HttpGet("all")]
        public async Task<IResult> GetAllFeedbacks()
        {
            try
            {
                var feedbacks = await _feedbackService.GetAllFeedbacksAsync();
                return TypedResults.Ok(feedbacks);
            }
            catch (InvalidOperationException ex)
            {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }

        // GET: api/feedback
        [HttpGet]
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
    }
}
