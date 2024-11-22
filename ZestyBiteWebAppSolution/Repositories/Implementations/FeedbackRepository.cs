using Microsoft.EntityFrameworkCore;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;

namespace ZestyBiteWebAppSolution.Repositories.Implementations
{
    public class FeedbackRepository : IRepository<Feedback>, IFeedbackRepository
    {
        private readonly ZestyBiteContext _context;
        public FeedbackRepository(ZestyBiteContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Feedback>> GetFeedbacksByItemIdAsync(int itemId)
        {
            return await _context.Feedbacks
                .Where(f => f.ItemId == itemId && f.ParentFbFlag == null) // Exclude replies
                                                                          //.Include(f => f.Account)
                                                                          //.Include(f => f.Item)
                .OrderByDescending(f => f.FbDatetime)
                .ToListAsync();
        }

        // Rely on the generic IRepository<T> methods for basic CRUD:

        public Task<IEnumerable<Feedback>> GetAllFeedbacksAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Feedback>> GetFeedbackRepliesAsync(int ParentFb)
        {
            return await _context.Feedbacks
                .Where(f => f.ParentFbFlag == ParentFb)
                //.Include(f => f.Account)
                .OrderBy(f => f.FbDatetime) // Oldest first
                .ToListAsync();
        }

        //public async Task<Feedback> SubmitFeedbackAsync(Feedback feedback)
        //{
        //    _context.Feedbacks.Add(feedback);
        //    await _context.SaveChangesAsync();
        //    return feedback;
        //}

        public async Task<Feedback> CreateAsync(Feedback item)
        {
            _context.Feedbacks.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }
        public async Task<Feedback> UpdateAsync(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        //public async Task<Feedback> DeleteAsync(int feedbackId)
        //{
        //    var feedback = await _context.Feedbacks.FindAsync(feedbackId);

        //    _context.Feedbacks.Remove(feedback);
        //    await _context.SaveChangesAsync();
        //    return feedback; // Return the deleted feedback or null if not found
        //}

        //CRUD for reply

        public async Task<IEnumerable<Feedback>> GetRepliesByParentIdAsync(int ParentFb)
        {
            return await _context.Feedbacks
                .Where(f => f.ParentFbFlag == ParentFb)
                //.Include(f => f.Account) // Include Account for Username and ProfileImage
                .OrderBy(f => f.FbDatetime) // Oldest first
                .ToListAsync();
        }
        //public async Task<Feedback> SubmitReplyAsync(Feedback reply)
        //{
        //    _context.Feedbacks.Add(reply);
        //    await _context.SaveChangesAsync();
        //    return reply;
        //}

        //public async Task<Feedback> UpdateReplyAsync(Feedback reply)
        //{
        //    _context.Feedbacks.Update(reply);
        //    await _context.SaveChangesAsync();
        //    return reply;
        //}

        //public async Task<Feedback> DeleteReplyAsync(int replyId)
        //{
        //    var reply = await _context.Feedbacks.FindAsync(replyId);
        //    _context.Feedbacks.Remove(reply);
        //    await _context.SaveChangesAsync();
        //    return reply; // Return the deleted reply or null if not found
        //}

        public async Task<IEnumerable<Feedback?>> GetAllAsync()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        public async Task<Feedback?> GetByIdAsync(int id) => await _context.Feedbacks.FindAsync(id);

        public async Task<Feedback> DeleteAsync(Feedback feedback)
        {
            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }
    }
}
