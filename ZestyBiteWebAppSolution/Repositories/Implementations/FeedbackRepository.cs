using Microsoft.EntityFrameworkCore;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;

namespace ZestyBiteWebAppSolution.Repositories.Implementations
{
    public class FeedbackRepository : IRepository<Feedback>, IFeedbackRepository {
        private readonly ZestyBiteContext _context;

        public FeedbackRepository(ZestyBiteContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByItemIdAsync(int itemId) {
            return await _context.Feedbacks
                .Where(f => f.ItemId == itemId) // Exclude replies
                .Include(f => f.UsernameNavigation)
                .Include(f => f.Item)
                .OrderByDescending(f => f.FbDatetime)
                .ToListAsync();
        }

        public async Task<Feedback?> GetByIdAsync(int id) => await _context.Feedbacks.FindAsync(id);

        // Rely on the generic IRepository<T> methods for basic CRUD:
        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync(int pageNumber, int pageSize)
        {
            var offset = (pageNumber - 1) * pageSize;
            return await _context.Feedbacks
                .Include(f => f.UsernameNavigation) // Include related entities
                .Include(f => f.Item)
                .OrderByDescending(f => f.FbDatetime)
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<IEnumerable<Feedback?>> GetAllAsync() {
            return await _context.Feedbacks
                .Include(f => f.UsernameNavigation)
                .Include(f => f.Item)
                .Include(f => f.ParentFbFlagNavigation)
                .ToListAsync();
        }

        public async Task<Feedback> CreateAsync(Feedback feedback) {
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }
        public async Task<Feedback> UpdateAsync(Feedback feedback) {
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task<Feedback> DeleteAsync(Feedback feedback) {
            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        // CRUD for reply
        public async Task<IEnumerable<Feedback>> GetFeedbackRepliesAsync(int ParentFb) {
            return await _context.Feedbacks
                .Where(f => f.ParentFbFlag == ParentFb)
                .Include(f => f.UsernameNavigation)
                .OrderBy(f => f.FbDatetime) // Oldest first
                .ToListAsync();
        }
        public async Task<Feedback> CreateReplyAsync(Feedback reply) {
            _context.Feedbacks.Add(reply);
            await _context.SaveChangesAsync();
            return reply;
        }
        public async Task<Feedback> UpdateReplyAsync(Feedback reply) {
            _context.Feedbacks.Update(reply);
            await _context.SaveChangesAsync();
            return reply;
        }
        public async Task<bool> DeleteReplyAsync(Feedback reply) {
            _context.Feedbacks.Remove(reply);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}