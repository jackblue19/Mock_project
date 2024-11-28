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
        private readonly IItemRepository _itemRepository;
        private readonly ITableRepository _tableRepository;

        public TableDetailService(
            ITableDetailRepository tableDetailRepository,
            IItemRepository itemRepository,
            ITableRepository tableRepository)
        {
            _repository = tableDetailRepository;
            _itemRepository = itemRepository;
            _tableRepository = tableRepository;
        }

        public async Task<TableDetail?> GetTableDetailByIdAsync(int tableId, int itemId)
        {
            return await _repository.GetByIdsAsync(tableId, itemId);
        }

        public async Task<IEnumerable<TableDetail?>> GetAllTableDetailsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<TableDetail> CreateTableDetailAsync(TableDetail tableDetail)
        {
            if (tableDetail == null)
            {
                throw new ArgumentNullException(nameof(tableDetail), "TableDetail cannot be null");
            }

            var existingTable = await _tableRepository.GetByIdAsync(tableDetail.TableId);
            if (existingTable == null)
            {
                throw new InvalidOperationException($"Table with ID {tableDetail.TableId} not found.");
            }

            var existingItem = await _itemRepository.GetByIdAsync(tableDetail.ItemId);
            if (existingItem == null)
            {
                throw new InvalidOperationException($"Item with ID {tableDetail.ItemId} not found.");
            }

            var created = await _repository.CreateAsync(tableDetail);
            return created;
        }

        public async Task<TableDetail> UpdateTableDetailAsync(int tableId, int itemId, TableDetail tableDetail)
        {
            if (tableDetail == null)
            {
                throw new ArgumentNullException(nameof(tableDetail), "TableDetail cannot be null");
            }

            var existingTableDetail = await _repository.GetByIdsAsync(tableId, itemId);
            if (existingTableDetail == null)
            {
                throw new InvalidOperationException($"TableDetail with Table ID {tableId} and Item ID {itemId} not found.");
            }

            existingTableDetail.Quantity = tableDetail.Quantity;

            await _repository.UpdateAsync(existingTableDetail);
            return existingTableDetail;
        }

        public async Task<bool> DeleteTableDetailAsync(int tableId, int itemId)
        {
            var tableDetail = await _repository.GetByIdsAsync(tableId, itemId);
            if (tableDetail == null)
            {
                throw new InvalidOperationException($"TableDetail with Table ID {tableId} and Item ID {itemId} not found.");
            }

            await _repository.DeleteAsync(tableDetail);
            return true;
        }
    }
}
