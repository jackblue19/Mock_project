using Microsoft.AspNetCore.Identity;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Services.Implementations
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _repository;
        private readonly IItemRepository _itemRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IAccountRepository _accountRepository;

        public TableService(
            ITableRepository tableRepository,
            IItemRepository itemRepository,
            IReservationRepository reservationRepository,
            IAccountRepository accountRepository)
        {
            _repository = tableRepository;
            _itemRepository = itemRepository;
            _reservationRepository = reservationRepository;
            _accountRepository = accountRepository;
        }

        public async Task<Table?> GetTableByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Table?>> GetAllTablesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Table> CreateTableAsync(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table), "Table cannot be null");
            }

            var created = await _repository.CreateAsync(table);
            return created;
        }

        public async Task<Table> UpdateTableAsync(int id, Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table), "Table cannot be null");
            }

            var existingTable = await _repository.GetByIdAsync(id);
            if (existingTable == null)
            {
                throw new InvalidOperationException($"Table with ID {id} not found.");
            }

            existingTable.TableCapacity = table.TableCapacity;
            existingTable.TableMaintenance = table.TableMaintenance;
            existingTable.TableStatus = table.TableStatus;
            existingTable.TableNote = table.TableNote;
            existingTable.ReservationId = table.ReservationId;
            existingTable.ItemId = table.ItemId;
            existingTable.TableType = table.TableType;
            existingTable.AccountId = table.AccountId;

            await _repository.UpdateAsync(existingTable);
            return existingTable;
        }

        public async Task<bool> DeleteTableAsync(int id)
        {
            var table = await _repository.GetByIdAsync(id);
            if (table == null)
            {
                throw new InvalidOperationException($"Table with ID {id} not found.");
            }

            await _repository.DeleteAsync(table);
            return true;
        }
    }
}
