using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ZestyBiteWebAppSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService) {
            _feedbackService = feedbackService;
        }

        // GET: api/feedback/items
        [AllowAnonymous]
        [HttpGet("items")]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetItems() {
            var items = await _feedbackService.GetAllItemsAsync();
            return Ok(items);
        }

        // GET: api/feedback/allfeedbacks
        [AllowAnonymous]
        [HttpGet("allfeedbacks")]
        public async Task<ActionResult<IEnumerable<FeedbackDTO>>> GetAllFeedbacks() {
            try {
                var feedbacks = await _feedbackService.GetAllFeedbacksAsync();
                return Ok(feedbacks);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/feedback/item/{itemId}
        [AllowAnonymous]
        [HttpGet("item/{itemId}")]
        public async Task<ActionResult<IEnumerable<FeedbackDTO>>> GetFeedbacksByItemId(int itemId) {
            try {
                var feedbacks = await _feedbackService.GetFeedbacksByItemIdAsync(itemId);
                return Ok(feedbacks);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/feedback/submitfeedback
        [Authorize]
        [HttpPost("submitfeedback")]
        public async Task<ActionResult<FeedbackDTO>> SubmitFeedback([FromBody] FeedbackDTO feedbackDto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            try
            {
                string username = User.Identity.Name;
                var submittedFeedback = await _feedbackService.SubmitFeedbackAsync(feedbackDto, username);
                return CreatedAtAction(nameof(GetFeedbacksByItemId), new { itemId = submittedFeedback.ItemId }, submittedFeedback);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/feedback
        [HttpPut]
        public async Task<ActionResult<FeedbackDTO>> UpdateFeedback([FromBody] FeedbackDTO feedbackDto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            try {
                var updatedFeedback = await _feedbackService.UpdateFeedbackAsync(feedbackDto);
                return Ok(updatedFeedback);
            } catch (InvalidOperationException ex) {
                return NotFound(new { Message = ex.Message });
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/feedback/feedbackpagination
        [AllowAnonymous]
        [HttpGet("feedbackpagination")]
        public async Task<IActionResult> GetFeedbacksByPage(int pageNumber, int pageSize)
        {
            var feedbacks = await _feedbackService.GetFeedbacksByPageAsync(pageNumber, pageSize);
            return Ok(feedbacks);
        }

        // DELETE: api/feedback/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFeedback(int id) {
            try {
                var result = await _feedbackService.DeleteFeedbackAsync(id);
                if (result) {
                    return NoContent();
                }
                return NotFound(new { Message = "Feedback not found" });
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // CRUD Reply
        // GET: api/feedback/replies/{parentFb}
        [HttpGet("replies/{parentFb}")]
        public async Task<ActionResult<IEnumerable<ReplyDTO>>> GetRepliesForFeedback(int parentFb) {
            try {
                var replies = await _feedbackService.GetRepliesByFeedbackAsync(parentFb);
                return Ok(replies);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/feedback/reply
        [Authorize]
        [HttpPost("reply")]
        public async Task<ActionResult<ReplyDTO>> SubmitReply([FromBody] ReplyDTO replyDto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            try
            {
                string username = User.Identity.Name; // Get the username from the authenticated user
                var submittedReply = await _feedbackService.SubmitReplyAsync(replyDto.ParentFb, replyDto, username);
                return CreatedAtAction(nameof(GetRepliesForFeedback), new { parentFb = submittedReply.ParentFb }, submittedReply);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/feedback/reply
        [HttpPut("reply")]
        public async Task<ActionResult<ReplyDTO>> UpdateReply([FromBody] ReplyDTO replyDto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            try {
                var updatedReply = await _feedbackService.UpdateReplyAsync(replyDto);
                return Ok(updatedReply);
            } catch (InvalidOperationException ex) {
                return NotFound(new { Message = ex.Message });
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/feedback/reply/{id}
        [HttpDelete("reply/{id}")]
        public async Task<ActionResult> DeleteReply(int id) {
            try {
                var result = await _feedbackService.DeleteReplyAsync(id);
                if (result) {
                    return NoContent();
                }
                return NotFound(new { Message = "Reply not found" });
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}