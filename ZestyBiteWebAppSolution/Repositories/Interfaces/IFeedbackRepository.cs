﻿using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Repositories.Interfaces {
    public interface IFeedbackRepository : IRepository<Feedback> {
        // Fetch feedbacks related to a specific item, including Account and Item relationships
        Task<IEnumerable<Feedback>> GetFeedbacksByItemIdAsync(int itemId);
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Feedback>> GetFeedbackRepliesAsync(int ParentFb);
        // CRUD for replies
        Task<Feedback> CreateReplyAsync(Feedback reply);
        Task<Feedback> UpdateReplyAsync(Feedback reply);
        Task<bool> DeleteReplyAsync(Feedback replyId);
    }
}