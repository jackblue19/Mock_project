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

        public async Task<TableDetailDTO?> CreateTableDetailAsync(TableDetailDTO tableDetailDto)
        {
            var tableDetail = _mapper.Map<TableDetail>(tableDetailDto); // Map DTO to entity
            var createdTableDetail = await _tableDetailRepository.CreateAsync(tableDetail); // Create entity in repository
            return _mapper.Map<TableDetailDTO>(createdTableDetail); // Map created entity back to DTO
        }

        public async Task<bool> DeleteTableDetailAsync(int tableDetailId)
        {
            var tableDetail = await _tableDetailRepository.GetByIdAsync(tableDetailId);
            if (tableDetail != null)
            {
                await _tableDetailRepository.DeleteAsync(tableDetail);
                return true; // Return true if the table detail was found and deleted
            }
            return false; // Return false if the table detail was not found
        }

        public async Task<IEnumerable<TableDetailDTO?>> GetAllTableDetailsAsync()
        {
            var tableDetails = await _tableDetailRepository.GetAllAsync(); // Get all table details from repository
            return _mapper.Map<IEnumerable<TableDetailDTO?>>(tableDetails); // Map entities to DTOs
        }

        public async Task<TableDetailDTO?> GetTableDetailByIdAsync(int tableDetailId)
        {
            var tableDetail = await _tableDetailRepository.GetByIdAsync(tableDetailId); // Get table detail by ID
            return _mapper.Map<TableDetailDTO?>(tableDetail); // Map entity to DTO
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
            var updatedTableDetail = await _tableDetailRepository.UpdateAsync(existingTableDetail); // Update entity in repository
            return _mapper.Map<TableDetailDTO?>(updatedTableDetail); // Map updated entity back to DTO
        }
    }
}