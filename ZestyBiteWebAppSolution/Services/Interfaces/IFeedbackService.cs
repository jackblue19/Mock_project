using ZestyBiteWebAppSolution.Models.DTOs;

namespace ZestyBiteWebAppSolution.Services.Interfaces {
    public interface IFeedbackService {
        Task<IEnumerable<FeedbackDTO>> GetAllFeedbacksAsync(int pageNumber, int pageSize);
        Task<IEnumerable<FeedbackDTO>> GetFeedbacksByItemIdAsync(int itemId);

        //Task<IEnumerable<FeedbackDTO?>> GetAllFeedbacksAsync();
        Task<IEnumerable<FeedbackDTO?>> GetAllFeedbacksAsync();
        Task<FeedbackDTO> SubmitFeedbackAsync(FeedbackDTO feedbackDto);
        Task<FeedbackDTO> UpdateFeedbackAsync(FeedbackDTO feedbackDto);
        Task<bool> DeleteFeedbackAsync(int feedbackId);

        // CRUD for replies
        Task<FeedbackDTO> SubmitReplyAsync(int parentFbFlag, ReplyDTO replyDto);
        Task<FeedbackDTO> UpdateReplyAsync(ReplyDTO replyDto);
        Task<bool> DeleteReplyAsync(int replyId);
        Task<IEnumerable<ReplyDTO>> GetRepliesForFeedbackAsync(int parentFbFlag);

    }
}
