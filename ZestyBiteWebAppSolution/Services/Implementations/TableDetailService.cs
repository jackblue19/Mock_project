using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Services.Implementations
{
    public class TableDetailService : ITableDetailService
    {
        private readonly ITableDetailRepository _tableDetailRepository;
        private readonly IMapper _mapper;

        public TableDetailService(ITableDetailRepository tableDetailRepository, IMapper mapper)
        {
            _tableDetailRepository = tableDetailRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TableDetailDTO?>> GetAllTableDetailsAsync()
        {
            var tableDetails = await _tableDetailRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TableDetailDTO?>>(tableDetails);
        }

        public async Task<IEnumerable<TableDetailDTO>> GetTableItemsByTableIdAsync(int tableId)
        {
            var tableDetails = await _tableDetailRepository.GetByTableIdAsync(tableId);
            return _mapper.Map<IEnumerable<TableDetailDTO>>(tableDetails);
        }

        public async Task<TableDetailDTO?> GetTableDetailByIdAsync(int tableDetailId)
        {
            var tableDetail = await _tableDetailRepository.GetByIdAsync(tableDetailId);
            return _mapper.Map<TableDetailDTO?>(tableDetail);
        }

        public async Task<TableDetailDTO?> CreateTableDetailAsync(TableDetailDTO tableDetailDto)
        {
            var tableDetail = _mapper.Map<TableDetail>(tableDetailDto);
            var createdTableDetail = await _tableDetailRepository.CreateAsync(tableDetail);
            return _mapper.Map<TableDetailDTO>(createdTableDetail);
        }

        public async Task<bool> DeleteTableDetailAsync(int tableDetailId)
        {
            var tableDetail = await _tableDetailRepository.GetByIdAsync(tableDetailId);
            if (tableDetail != null)
            {
                await _tableDetailRepository.DeleteAsync(tableDetail);
                return true;
            }
            return false;
        }
        public async Task<TableDetailDTO?> UpdateTableDetailAsync(TableDetailDTO tableDetailDto)
        {
            var existingTableDetail = await _tableDetailRepository.GetByIdAsync(tableDetailDto.TableId);
            if (existingTableDetail == null)
            {
                throw new InvalidOperationException("Table detail not found");
            }

            // Map the updated values from DTO to the existing entity
            _mapper.Map(tableDetailDto, existingTableDetail);
            var updatedTableDetail = await _tableDetailRepository.UpdateAsync(existingTableDetail);
            return _mapper.Map<TableDetailDTO?>(updatedTableDetail);
        }
    }
}
