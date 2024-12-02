using Microsoft.AspNetCore.Identity;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZestyBiteWebAppSolution.Services
{
    public class TableService
    {
        private readonly TableRepository _tableRepository;

        public TableService(TableRepository tableRepository)
        {
            _tableRepository = tableRepository;
        }

        // Create
        public async Task<Table> CreateTableAsync(Table table)
        {
            return await _tableRepository.CreateTableAsync(table);
        }

        // Read
        public async Task<Table> GetTableByIdAsync(int tableId)
        {
            return await _tableRepository.GetTableByIdAsync(tableId);
        }

        public async Task<List<Table>> GetAllTablesAsync()
        {
            return await _tableRepository.GetAllTablesAsync();
        }

        // Update
        public async Task<Table> UpdateTableAsync(Table table)
        {
            return await _tableRepository.UpdateTableAsync(table);
        }

        // Delete
        public async Task<bool> DeleteTableAsync(int tableId)
        {
            return await _tableRepository.DeleteTableAsync(tableId);
        }
    }
}
