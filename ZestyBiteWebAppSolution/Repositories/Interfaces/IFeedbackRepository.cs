using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Repositories.Interfaces
{
    public interface IFeedbackRepository : IRepository<Feedback>
    {
        // Fetch feedbacks related to a specific item, including Account and Item relationships
        Task<IEnumerable<Feedback>> GetFeedbacksByItemIdAsync(int itemId);
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Feedback>> GetFeedbackRepliesAsync(int ParentFb);

        //CRUD for reply
        Task<IEnumerable<Feedback>> GetRepliesByParentIdAsync(int ParentFb);
        //Task<Feedback> SubmitReplyAsync(Feedback reply);
    }
}