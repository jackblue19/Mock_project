using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Interfaces;
using System.Threading.Tasks;
using ZestyBiteWebAppSolution.Models.Entities;
using Azure.Messaging;

namespace ZestyBiteWebAppSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;
       
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        /*
        // GET: api/feedback/item/{itemId}
        [HttpGet("item/{itemId}")]
        public async Task<IActionResult> GetFeedbacksForItem(int itemId)
        {
            try
            {
                var feedbacks = await _feedbackService.GetFeedbacksForItemAsync(itemId);
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                // Log exception (if logging is implemented)
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
                await _feedbackService.SubmitFeedbackAsync(feedbackDto);
                return CreatedAtAction(nameof(GetFeedbacksForItem), new { itemId = feedbackDto.ItemId }, feedbackDto);
            }
            catch (Exception ex)
            {
                // Log exception (if logging is implemented)
                return StatusCode(500, $"Internal server error while submitting feedback: {ex.Message}");
            }
        }

        // GET: api/feedback/all?pageNumber={pageNumber}&pageSize={pageSize}
        [HttpGet("all")]
        public async Task<IActionResult> GetAllFeedbacks(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var feedbacks = await _feedbackService.GetAllFeedbacksAsync(pageNumber, pageSize);
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                // Log exception (if logging is implemented)
                return StatusCode(500, $"Internal server error while retrieving all feedbacks: {ex.Message}");
            }
        }

        // PUT: api/feedback
        [HttpPut]
        public async Task<IActionResult> UpdateFeedback([FromBody] FeedbackDTO feedbackDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedFeedback = await _feedbackService.UpdateFeedbackAsync(feedbackDto);
                return Ok(updatedFeedback);
            }
            catch (Exception ex)
            {
                // Log exception (if logging is implemented)
                return StatusCode(500, $"Internal server error while updating feedback: {ex.Message}");
            }
        }

        // DELETE: api/feedback/{feedbackId}
        [HttpDelete("{feedbackId}")]
        public async Task<IActionResult> DeleteFeedback(int feedbackId)
        {
            try
            {
                var result = await _feedbackService.DeleteFeedbackAsync(feedbackId);
                if (result)
                {
                    return NoContent(); // 204 No Content
                }
                return NotFound(); // 404 Not Found
            }
            catch (Exception ex)
            {
                // Log exception (if logging is implemented)
                return StatusCode(500, $"Internal server error while deleting feedback {feedbackId}: {ex.Message}");
            }
        }

        // GET: api/feedback/replies/{parentFbFlag}
        [HttpGet("replies/{parentFbFlag}")]
        public async Task<IActionResult> GetRepliesForFeedback(int parentFbFlag)
        {
            try
            {
                var replies = await _feedbackService.GetRepliesForFeedbackAsync(parentFbFlag);
                return Ok(replies);
            }
            catch (Exception ex)
            {
                // Log exception (if logging is implemented)
                return StatusCode(500, $"Internal server error while retrieving replies for feedback {parentFbFlag}: {ex.Message}");
            }
        }

        // POST: api/feedback/reply/{parentFbFlag}
        [HttpPost("reply/{parentFbFlag}")]
        public async Task<IActionResult> SubmitReply(int parentFbFlag, [FromBody] ReplyDTO replyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var submittedReply = await _feedbackService.SubmitReplyAsync(parentFbFlag, replyDto);
                return CreatedAtAction(nameof(GetRepliesForFeedback), new { parentFbFlag }, submittedReply);
            }
            catch (Exception ex)
            {
                // Log exception (if logging is implemented)
                return StatusCode(500, $"Internal server error while submitting reply for feedback {parentFbFlag}: {ex.Message}");
            }
        }

        // PUT: api/feedback/reply
        [HttpPut("reply")]
        public async Task<IActionResult> UpdateReply([FromBody] ReplyDTO replyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedReply = await _feedbackService.UpdateReplyAsync(replyDto);
                return Ok(updatedReply);
            }
            catch (Exception ex)
            {
                // Log exception (if logging is implemented)
                return StatusCode(500, $"Internal server error while updating reply: {ex.Message}");
            }
        }

        // DELETE: api/feedback/reply/{replyId}
        [HttpDelete("reply/{replyId}")]
        public async Task<IActionResult> DeleteReply(int replyId)
        {
            try
            {
                var result = await _feedbackService.DeleteReplyAsync(replyId);
                if (result)
                {
                    return NoContent(); // 204 No Content
                }
                return NotFound(); // 404 Not Found
            }
            catch (Exception ex)
            {
                // Log exception (if logging is implemented)
                return StatusCode(500, $"Internal server error while deleting reply {replyId}: {ex.Message}");
            }
        }
    */
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
    }
}
