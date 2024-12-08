using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Services.Implementations
{
    public class SupplyItemService : ISupplyItemService
    {
        private readonly ISupplyItemRepository _supplyItemRepository;
        private readonly IMapper _mapper;

        public SupplyItemService(ISupplyItemRepository supplyItemRepository, IMapper mapper)
        {
            _supplyItemRepository = supplyItemRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SupplyItemDTO?>> GetAllSupplyItemsAsync()
        {
            var supplyItems = await _supplyItemRepository.GetAllAsync(); // Lấy tất cả các supply items từ repository
            if (supplyItems == null || !supplyItems.Any())
            {
                return Enumerable.Empty<SupplyItemDTO?>(); // Trả về danh sách rỗng nếu không có dữ liệu
            }
            return _mapper.Map<IEnumerable<SupplyItemDTO?>>(supplyItems); // Map các entity sang DTO
        }


        //public async Task<SupplyItemDTO?> CreateSupplyItemAsync(SupplyItemDTO supplyItemDto)
        //{
        //    var supplyItem = _mapper.Map<SupplyItem>(supplyItemDto); // Map DTO to entity
        //    var createdSupplyItem = await _supplyItemRepository.CreateAsync(supplyItem); // Create entity in repository
        //    return _mapper.Map<SupplyItemDTO>(createdSupplyItem); // Map created entity back to DTO
        //}

        //public async Task<bool> DeleteSupplyItemAsync(int supplyId, int itemId)
        //{
        //    var supplyItem = await _supplyItemRepository.GetByIdAsync(supplyId, itemId);
        //    if (supplyItem != null)
        //    {
        //        await _supplyItemRepository.DeleteAsync(supplyItem);
        //        return true; // Return true if the supply item was found and deleted
        //    }
        //    return false; // Return false if the supply item was not found
        //}

        //public async Task<SupplyItemDTO?> GetSupplyItemByIdAsync(int supplyId, int itemId)
        //{
        //    var supplyItem = await _supplyItemRepository.GetByIdAsync(supplyId, itemId); // Get supply item by supplyId and itemId
        //    return _mapper.Map<SupplyItemDTO?>(supplyItem); // Map entity to DTO
        //}

        //public async Task<SupplyItemDTO?> UpdateSupplyItemAsync(SupplyItemDTO supplyItemDto)
        //{
        //    var existingSupplyItem = await _supplyItemRepository.GetByIdAsync(supplyItemDto.SupplyId, supplyItemDto.ItemId);
        //    if (existingSupplyItem == null)
        //    {
        //        throw new InvalidOperationException("Supply item not found");
        //    }

        //    // Map the updated values from DTO to the existing entity
        //    _mapper.Map(supplyItemDto, existingSupplyItem);
        //    var updatedSupplyItem = await _supplyItemRepository.UpdateAsync(existingSupplyItem); // Update entity in repository
        //    return _mapper.Map<SupplyItemDTO?>(updatedSupplyItem); // Map updated entity back to DTO
        //}
    }
}
