using Microsoft.AspNetCore.Identity;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;
namespace ZestyBiteWebAppSolution.Services.Implementations
{
    public class TableDetailService : ITableDetailService
    {
        private readonly ITableDetailRepository _repository;

        public TableDetailService(ITableDetailRepository tableDetailRepository)
        {
            _repository = tableDetailRepository;
        }

        public async Task<TableDetail?> GetTableDetailByIdAsync(int tableId, int itemId)
        {
            return await _repository.GetTableDetailByIdAsync(tableId, itemId);
        }

        public async Task<IEnumerable<TableDetail>> GetTableDetailsByTableIdAsync(int tableId)
        {
            return await _repository.GetTableDetailsByTableIdAsync(tableId);
        }

        public async Task<IEnumerable<TableDetail>> GetTableDetailsByItemIdAsync(int itemId)
        {
            return await _repository.GetTableDetailsByItemIdAsync(itemId);
        }

        public async Task<TableDetail> CreateTableDetailAsync(TableDetail tableDetail)
        {
            if (tableDetail == null)
            {
                throw new ArgumentNullException(nameof(tableDetail), "TableDetail cannot be null");
            }

            var createdTableDetail = await _repository.CreateAsync(tableDetail);
            return createdTableDetail;
        }

        public async Task<TableDetail> UpdateTableDetailAsync(int tableId, int itemId, TableDetail tableDetail)
        {
            if (tableDetail == null)
            {
                throw new ArgumentNullException(nameof(tableDetail), "TableDetail cannot be null");
            }

            var existingTableDetail = await _repository.GetTableDetailByIdAsync(tableId, itemId);
            if (existingTableDetail == null)
            {
                throw new InvalidOperationException($"TableDetail with TableId {tableId} and ItemId {itemId} not found.");
            }

            // Update the details as needed
            existingTableDetail.Quantity = tableDetail.Quantity;

            await _repository.UpdateAsync(existingTableDetail);
            return existingTableDetail;
        }

        public async Task<bool> DeleteTableDetailAsync(int tableId, int itemId)
        {
            var tableDetail = await _repository.GetTableDetailByIdAsync(tableId, itemId);
            if (tableDetail == null)
            {
                throw new InvalidOperationException($"TableDetail with TableId {tableId} and ItemId {itemId} not found.");
            }

            await _repository.DeleteAsync(tableDetail);
            return true;
        }

        public async Task<IEnumerable<TableDetail>> GetAllTableDetailsAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}
