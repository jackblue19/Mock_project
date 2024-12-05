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
        private readonly ITableDetailRepository _tableDetailRepository;
        private readonly IMapper _mapper;

        public TableService(ITableRepository tableRepository, ITableDetailRepository tableDetailRepository, IMapper mapper)
        {
            _tableRepository = tableRepository;
            _tableDetailRepository = tableDetailRepository;
            _mapper = mapper;
        }

        // Default CRUD by IRepo
        public async Task<IEnumerable<TableDTO?>> GetAllTablesAsync()
        {
            var tables = await _tableRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TableDTO?>>(tables);
        }

        public async Task<TableDTO?> GetTableByIdAsync(int tableId)
        {
            var table = await _tableRepository.GetByIdAsync(tableId);
            return _mapper.Map<TableDTO?>(table);
        }

        public async Task<TableDTO?> CreateTableAsync(TableDTO tableDto)
        {
            var table = _mapper.Map<Table>(tableDto);
            var createdTable = await _tableRepository.CreateAsync(table);
            return _mapper.Map<TableDTO>(createdTable);
        }

        public async Task<bool> DeleteTableAsync(int tableId)
        {
            var table = await _tableRepository.GetByIdAsync(tableId);
            if (table != null)
            {
                await _tableRepository.DeleteAsync(table);
                return true;
            }
            return false;
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
            var updatedTable = await _tableRepository.UpdateAsync(existingTable);
            return _mapper.Map<TableDTO?>(updatedTable);
        }

        // Additional CRUD
        public async Task<IEnumerable<TableDTO?>> GetAllRealTablesAsync()
        {
            var realTables = await _tableRepository.GetTablesByTypeAsync(0);
            return _mapper.Map<IEnumerable<TableDTO?>>(realTables);
        }

        public async Task<IEnumerable<TableDTO?>> GetAllVirtualTablesAsync()
        {
            var virtualTables = await _tableRepository.GetTablesByTypeAsync(1);
            return _mapper.Map<IEnumerable<TableDTO?>>(virtualTables);
        }
        public async Task<IEnumerable<TableDetailDTO>> GetTableItemsByTableIdAsync(int tableId)
        {
            try
            {
                var tableDetails = await _tableDetailRepository.GetByTableIdAsync(tableId);
                return _mapper.Map<IEnumerable<TableDetailDTO>>(tableDetails);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error occured while fetching table items by table id");
            }
        }
    }
}
