using Microsoft.AspNetCore.Identity;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZestyBiteWebAppSolution.Services.Interfaces;
using ZestyBiteWebAppSolution.Repositories.Interfaces;

namespace ZestyBiteWebAppSolution.Services.Implementations
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _tableRepository;

        public TableService(ITableRepository tableRepository)
        {
            _tableRepository = tableRepository;
        }

        // Create
        public async Task<Table> CreateTableAsync(Table table)
        {
            return await _tableRepository.CreateAsync(table);
        }

        // Read
        public async Task<Table?> GetTableByIdAsync(int tableId)
        {
            return await _tableRepository.GetByIdAsync(tableId);
        }

        public async Task<IEnumerable<Table>> GetAllTablesAsync()
        {
            return await _tableRepository.GetAllAsync();
        }

        // Update
        public async Task<Table> UpdateTableAsync(Table table)
        {
            return await _tableRepository.UpdateAsync(table);
        }

        // Delete
        public async Task<bool> DeleteTableAsync(int tableId)
        {
            return await _tableRepository.DeleteAsync(tableId);
        }
    }
}
