using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Services.Implementations
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _tableRepository;
        private readonly IMapper _mapper;

        public TableService(ITableRepository tableRepository, IMapper mapper)
        {
            _tableRepository = tableRepository;
            _mapper = mapper;
        }

        public async Task<TableDTO?> CreateTableAsync(TableDTO tableDto)
        {
            var table = _mapper.Map<Table>(tableDto); // Map DTO to entity
            var createdTable = await _tableRepository.CreateAsync(table); // Create entity in repository
            return _mapper.Map<TableDTO>(createdTable); // Map created entity back to DTO
        }

        public async Task<bool> DeleteTableAsync(int tableId)
        {
            var table = await _tableRepository.GetByIdAsync(tableId);
            if (table != null)
            {
                await _tableRepository.DeleteAsync(table);
                return true; // Return true if the table was found and deleted
            }
            return false; // Return false if the table was not found
        }

        public async Task<IEnumerable<TableDTO?>> GetAllTablesAsync()
        {
            var tables = await _tableRepository.GetAllAsync(); // Get all tables from repository
            return _mapper.Map<IEnumerable<TableDTO?>>(tables); // Map entities to DTOs
        }

        public async Task<TableDTO?> GetTableByIdAsync(int tableId)
        {
            var table = await _tableRepository.GetByIdAsync(tableId); // Get table by ID
            return _mapper.Map<TableDTO?>(table); // Map entity to DTO
        }

        public async Task<TableDTO?> UpdateTableAsync(TableDTO tableDto)
        {
            var existingTable = await _tableRepository.GetByIdAsync(tableDto.TableId);
            if (existingTable == null)
            {
                throw new InvalidOperationException("Table not found");
            }

            // Map the updated values from DTO to the existing entity
            _mapper.Map(tableDto, existingTable);
            var updatedTable = await _tableRepository.UpdateAsync(existingTable); // Update entity in repository
            return _mapper.Map<TableDTO?>(updatedTable); // Map updated entity back to DTO
        }
    }
}