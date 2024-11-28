using ZestyBiteWebAppSolution.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<IEnumerable<FeedbackDTO>> GetFeedbacksByPageAsync(int pageNumber, int pageSize);
        Task<IEnumerable<FeedbackDTO>> GetFeedbacksByItemIdAsync(int itemId);
        Task<IEnumerable<FeedbackDTO?>> GetAllFeedbacksAsync();
        Task<FeedbackDTO> SubmitFeedbackAsync(FeedbackDTO feedbackDto, string usn);
        Task<FeedbackDTO> UpdateFeedbackAsync(FeedbackDTO feedbackDto);
        Task<bool> DeleteFeedbackAsync(int feedbackId);
        Task<IEnumerable<ItemDTO?>> GetAllItemsAsync();

        // CRUD for replies
        Task<FeedbackDTO> SubmitReplyAsync(int parentFbFlag, ReplyDTO replyDto);
        Task<FeedbackDTO> UpdateReplyAsync(ReplyDTO replyDto);
        Task<bool> DeleteReplyAsync(int replyId);
        Task<IEnumerable<ReplyDTO>> GetRepliesByFeedbackAsync(int parentFbFlag);


    }
}
