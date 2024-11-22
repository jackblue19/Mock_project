using ZestyBiteWebAppSolution.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces
{
    public interface IFeedbackService
    {
        //Task<IEnumerable<FeedbackDTO>> GetFeedbacksForItemAsync(int itemId);
        //Task<IEnumerable<FeedbackDTO>> GetAllFeedbacksAsync(int pageNumber, int pageSize);
        //Task<IEnumerable<FeedbackDTO>> GetFeedbacksByItemIdAsync(int itemId);
        Task<IEnumerable<Feedback?>> GetAllFeedbacksAsync();
        //Task<IEnumerable<Feedback>> GetFeedbacksByItemIdAsync(int itemId);
        //Task<FeedbackDTO> SubmitFeedbackAsync(FeedbackDTO feedbackDto);
        //Task<FeedbackDTO> UpdateFeedbackAsync(FeedbackDTO feedbackDto);
        //Task<Feedback> SubmitFeedbackAsync(FeedbackDTO feedbackDto);
        //Task<Feedback> UpdateFeedbackAsync(FeedbackDTO feedbackDto);
        //Task<bool> DeleteFeedbackAsync(int feedbackId);

    //    // CRUD for replies
    //    Task<FeedbackDTO> SubmitReplyAsync(int parentFbFlag, ReplyDTO replyDto);
    //    Task<FeedbackDTO> UpdateReplyAsync(ReplyDTO replyDto);
    //    Task<bool> DeleteReplyAsync(int replyId);
    //    Task<IEnumerable<ReplyDTO>> GetRepliesForFeedbackAsync(int parentFbFlag);
    //
    }
}
