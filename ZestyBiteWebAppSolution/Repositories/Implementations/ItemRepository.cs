using Microsoft.EntityFrameworkCore;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;

namespace ZestyBiteWebAppSolution.Repositories {
    public class ItemRepository : IItemRepository, IRepository<Item> {
        private readonly ZestybiteContext _context;

        public ItemRepository(ZestybiteContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Item?>> GetAllAsync() {
            return await _context.Items.ToListAsync();
        }

        public async Task<Item?> GetByIdAsync(int id) {
            return await _context.Items.FindAsync(id);
        }

        public async Task<Item> CreateAsync(Item item) {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Item> UpdateAsync(Item item) {
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        // Ensure DeleteAsync returns Task<Item>
        public async Task<Item> DeleteAsync(Item item) {
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return item; // Return the deleted entity
        }
    }
}