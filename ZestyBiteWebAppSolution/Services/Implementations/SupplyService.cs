using AutoMapper;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Implementations;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Services.Implementations
{
    public class SupplyService : ISupplyService
    {
        private readonly ISupplyRepository _repository;
        private readonly ITableRepository _tableRepository;
        private readonly IMapper _mapper;

        public SupplyService(ISupplyRepository repository, ITableRepository tableRepository, IMapper mapper)
        {
            _repository = repository;
            _tableRepository = tableRepository;
            _mapper = mapper;
        }

        // Method to create a new supply
        // Method to retrieve all supplies
        public async Task<IEnumerable<SupplyDTO>> GetAllSuppliesAsync()
        {
            var supplies = await _repository.GetAllAsync();
            return supplies.Select(MapToDTO).ToList();
        }

        // Method to retrieve a supply by ID
        public async Task<SupplyDTO?> GetSupplyByIdAsync(int supplyId)
        {
            var supply = await _repository.GetSupplyByIdAsync(supplyId);
            if (supply == null)
                throw new KeyNotFoundException($"Supply with ID {supplyId} not found.");

            return MapToDTO(supply);
        }

        public async Task<SupplyDTO?> GetSupplyByTableIdAsync(int tableId)
        {
            // Call the repository method to get the Supply by TableId
            var supply = await _repository.GetSupplyByTableIdAsync(tableId);

            // If no supply is found, throw an exception
            if (supply == null)
                throw new KeyNotFoundException($"Supply with TableId {tableId} not found.");

            // Map the supply entity to a DTO and return
            return MapToDTO(supply);
        }



        // Utility methods for mapping
        private SupplyDTO MapToDTO(Supply supply)
        {
            return new SupplyDTO
            {
                SupplyId = supply.SupplyId,
                ProductName = supply.ProductName,
                SupplyQuantity = supply.SupplyQuantity,
                SupplyPrice = supply.SupplyPrice,
                SupplyStatus = supply.SupplyStatus,
                DateImport = supply.DateImport,
                DateExpiration = supply.DateExpiration,
                TableId = supply.TableId,
                VendorName = supply.VendorName,
                VendorPhone = supply.VendorPhone,
                VendorAddress = supply.VendorAddress,
                SupplyCategory = supply.SupplyCategory
            };
        }

        private Supply MapToEntity(SupplyDTO dto)
        {
            return new Supply
            {
                SupplyId = dto.SupplyId,
                ProductName = dto.ProductName,
                SupplyQuantity = dto.SupplyQuantity,
                SupplyPrice = dto.SupplyPrice,
                SupplyStatus = dto.SupplyStatus,
                DateImport = dto.DateImport,
                DateExpiration = dto.DateExpiration,
                TableId = dto.TableId,
                VendorName = dto.VendorName,
                VendorPhone = dto.VendorPhone,
                VendorAddress = dto.VendorAddress,
                SupplyCategory = dto.SupplyCategory
            };
        }
        public async Task<SupplyDTO?> CreateSupplyAsync(SupplyDTO supplyDto)
        {
            // Kiểm tra giá trị hợp lệ
            if (supplyDto.SupplyQuantity < 1)
            {
                throw new ArgumentException("Supply quantity must be greater than 1.");
            }

            if (supplyDto.SupplyPrice < 1)
            {
                throw new ArgumentException("Supply price must be greater than 1.");
            }

            if (supplyDto.SupplyStatus != 0 && supplyDto.SupplyStatus != 1)
            {
                throw new ArgumentException("Supply status must be either 0 (inactive) or 1 (active).");
            }

            // Kiểm tra ngày tháng
            if (supplyDto.DateImport >= supplyDto.DateExpiration)
            {
                throw new ArgumentException("Date of import must be earlier than date of expiration.");
            }

            // Kiểm tra Vendor_Phone duy nhất
            //var existingSupply = await _repository.GetByVendorPhoneAsync(supplyDto.VendorPhone);
            //if (existingSupply != null)
            //{
            //    throw new ArgumentException("Vendor phone must be unique.");
            //}

            // Nếu tất cả điều kiện đều hợp lệ, tiếp tục tạo mới
            var supply = _mapper.Map<Supply>(supplyDto);
            var createSupply = await _repository.CreateSupplyAsync(supply);
            return _mapper.Map<SupplyDTO>(createSupply);
        }
        public async Task<SupplyDTO> UpdateSupplyAsync(SupplyDTO dto, int id)
        {
            if (dto.SupplyQuantity < 1)
            {
                throw new ArgumentException("Supply quantity must be greater than 1.");
            }

            if (dto.SupplyPrice < 1)
            {
                throw new ArgumentException("Supply price must be greater than 0.");
            }

            if (dto.SupplyStatus != 0 && dto.SupplyStatus != 1)
            {
                throw new ArgumentException("Supply status must be either 0 (inactive) or 1 (active).");
            }

            if (dto.DateImport >= dto.DateExpiration)
            {
                throw new ArgumentException("Date of import must be earlier than date of expiration.");
            }
            // Check if the supply exists
            var existingSupply = await _repository.GetSupplyByIdAsync(id);
            if (existingSupply == null)
                throw new KeyNotFoundException($"Supply with ID {id} not found.");

            // Update all properties of the existing supply
            existingSupply.ProductName = dto.ProductName;
            existingSupply.SupplyQuantity = dto.SupplyQuantity;
            existingSupply.SupplyPrice = dto.SupplyPrice;
            existingSupply.SupplyStatus = dto.SupplyStatus;
            existingSupply.DateImport = dto.DateImport;
            existingSupply.DateExpiration = dto.DateExpiration;
            existingSupply.TableId = dto.TableId;
            existingSupply.VendorName = dto.VendorName;
            existingSupply.VendorPhone = dto.VendorPhone;
            existingSupply.VendorAddress = dto.VendorAddress;
            existingSupply.SupplyCategory = dto.SupplyCategory;

            // If you have other fields in the entity, update them similarly.

            // Save the changes to the database
            await _repository.UpdateSupplyAsync(existingSupply);

            // Map the updated entity to DTO (if you have a mapping function)
            var updatedDto = MapToDTO(existingSupply);

            // Return the updated DTO
            return updatedDto;
        }
        public async Task<bool> DeleteSupplyAsync(int supplyId)
        {
            var supply = await _repository.GetSupplyByIdAsync(supplyId);
            if (supply != null)
            {
                await _repository.DeleteAsync(supply);
                return true;
            }
            return false;
        }

        //public async Task<SupplyDTO?> UpdateSupplyAsync(SupplyDTO supplyDto)
        //{
        //    // Kiểm tra xem có Supply tương ứng với SupplyId không
        //    var existingSupply = await _repository.GetSupplyByIdAsync(supplyDto.SupplyId);
        //    if (existingSupply == null)
        //    {
        //        throw new InvalidOperationException("Supply not found");
        //    }
        //    // Ánh xạ các giá trị mới từ DTO vào entity hiện tại
        //    _mapper.Map(supplyDto, existingSupply);

        //    // Cập nhật Supply vào cơ sở dữ liệu
        //    var updatedSupply = await _repository.UpdateSupplyAsync(existingSupply);

        //    // Trả về SupplyDTO đã được cập nhật
        //    return _mapper.Map<SupplyDTO?>(updatedSupply);
        //}




        //public async Task<SupplyDTO> CreateSupplyAsync(SupplyDTO supplyDto)
        //{
        //    if (supplyDto == null)
        //    {
        //        throw new ArgumentNullException(nameof(supplyDto));
        //    }

        //    // Kiểm tra thông tin Table từ Table_ID
        //    var table = await _tableRepository.GetByIdAsync(supplyDto.TableId);
        //    if (table == null)
        //    {
        //        throw new InvalidOperationException("Invalid Table.");
        //    }

        //    // Kiểm tra trùng Vendor_Phone
        //    var existingVendor = await _repository.GetByVendorPhoneAsync(supplyDto.VendorPhone);
        //    if (existingVendor != null)
        //    {
        //        throw new InvalidOperationException("Vendor phone number already exists.");
        //    }

        //    // Kiểm tra SupplyCategory hợp lệ (Enum)
        //    if (!Enum.IsDefined(typeof(SupplyCategory), supplyDto.SupplyCategory))
        //    {
        //        throw new InvalidOperationException("Invalid Supply Category.");
        //    }

        //    // Chuyển đổi SupplyDTO thành Supply Entity
        //    var supply = new Supply
        //    {
        //        ProductName = supplyDto.ProductName,
        //        SupplyQuantity = supplyDto.SupplyQuantity,
        //        SupplyPrice = supplyDto.SupplyPrice,
        //        SupplyStatus = supplyDto.SupplyStatus,
        //        DateImport = supplyDto.DateImport,
        //        DateExpiration = supplyDto.DateExpiration,
        //        TableId = supplyDto.TableId,
        //        VendorName = supplyDto.VendorName,
        //        VendorPhone = supplyDto.VendorPhone,
        //        VendorAddress = supplyDto.VendorAddress,
        //        SupplyCategory = supplyDto.SupplyCategory
        //    };

        // Lưu vào cơ sở dữ liệu
        //var createdSupply = await _repository.CreateSupplyAsync(supply);

        //    // Trả về SupplyDTO của Supply vừa tạo
        //    return MapToDTO(createdSupply);
        //}


        // Method to delete a supply by ID

        //// Utility methods for mapping
        //private SupplyDTO MapToDTO(Supply supply)
        //{
        //    return new SupplyDTO
        //    {
        //        SupplyId = supply.SupplyId,
        //        ProductName = supply.ProductName,
        //        SupplyQuantity = supply.SupplyQuantity,
        //        SupplyPrice = supply.SupplyPrice,
        //        SupplyStatus = supply.SupplyStatus,
        //        DateImport = supply.DateImport,
        //        DateExpiration = supply.DateExpiration,
        //        TableId = supply.TableId,
        //        ItemId = supply.ItemId,
        //        VendorName = supply.VendorName,
        //        VendorPhone = supply.VendorPhone,
        //        VendorAddress = supply.VendorAddress,
        //        SupplyCategory = supply.SupplyCategory,
        //    };
        //}

        //private Supply MapToEntity(SupplyDTO dto)
        //{
        //    return new Supply
        //    {
        //        SupplyId = dto.SupplyId,
        //        ProductName = dto.ProductName,
        //        SupplyQuantity = dto.SupplyQuantity,
        //        SupplyPrice = dto.SupplyPrice,
        //        SupplyStatus = dto.SupplyStatus,
        //        DateImport = dto.DateImport,
        //        DateExpiration = dto.DateExpiration,
        //        TableId = dto.TableId,
        //        ItemId = dto.ItemId,
        //        VendorName = dto.VendorName,
        //        VendorPhone = dto.VendorPhone,
        //        VendorAddress = dto.VendorAddress,
        //        SupplyCategory = dto.SupplyCategory
        //    };
        //}
    }
}
