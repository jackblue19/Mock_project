using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // Render the Feedback view
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var feedbacks = await _feedbackService.GetAllFeedbacksAsync();
            return View(feedbacks);
        }

        // Render the Blog view
        [HttpGet]
        [Route("Blog")]
        public IActionResult Blog()
        {
            return View();
        }

        // CRUD Feedback

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
        [HttpPost("SubmitFeedback")]
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

        // PUT: api/feedback
        [HttpPut("UpdateFeedback")]
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
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while updating feedback: {ex.Message}");
            }
        }

        // GET: api/feedback
        [HttpGet("GetPagination")]
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

        // CRUD Reply
        // GET: api/feedback/replies/{parentFb}
        [HttpGet("replies/{parentFb}")]
        public async Task<IActionResult> GetRepliesForFeedback(int parentFb)
        {
            try
            {
                var replies = await _feedbackService.GetRepliesForFeedbackAsync(parentFb);
                return Ok(replies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while retrieving replies: {ex.Message}");
            }
        }

        // POST: api/feedback/reply
        [HttpPost("reply")]
        public async Task<IActionResult> SubmitReply([FromBody] ReplyDTO replyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var submittedReply = await _feedbackService.SubmitReplyAsync(replyDto.ParentFb, replyDto);
                return CreatedAtAction(nameof(GetRepliesForFeedback), new { parentFb = submittedReply.ParentFb }, submittedReply);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while submitting reply: {ex.Message}");
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
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while updating reply: {ex.Message}");
            }
        }

        // DELETE: api/feedback/reply/{id}
        [HttpDelete("reply/{id}")]
        public async Task<IActionResult> DeleteReply(int id)
        {
            try
            {
                var result = await _feedbackService.DeleteReplyAsync(id);
                if (result)
                {
                    return NoContent();
                }
                return NotFound(new { Message = "Reply not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while deleting reply: {ex.Message}");
            }
        }
    }
}
