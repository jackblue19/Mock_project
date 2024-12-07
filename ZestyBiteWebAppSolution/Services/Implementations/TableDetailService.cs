using AutoMapper;
using ZestyBiteWebAppSolution.Data;
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
        private readonly ZestyBiteContext _context;
        private readonly ITableRepository _tableRepository;
        private readonly IBillRepository _billRepository;

        public TableDetailService(ITableDetailRepository tableDetailRepository, IMapper mapper, ZestyBiteContext context, ITableRepository tableRepository, IBillRepository billRepository) {
            _tableDetailRepository = tableDetailRepository;
            _mapper = mapper;
            _context = context;
            _tableRepository = tableRepository;
            _billRepository = billRepository;
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
            return false; // Return false if the table detail was not found
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
        public async Task<int> ToPayment(Dictionary<int?, int?> itemQuantityMap, Account acc, string CartSessionKey, HttpContext httpContext) {
            if (!itemQuantityMap.Any() || acc == null) {
                return 0;
            }

            try
            {
                // Create the cart from the provided itemQuantityMap
                var cart = new CheckoutDTO
                {
                    Items = itemQuantityMap.Select(item => new CheckoutItemDTO
                    {
                        ItemId = item.Key,
                        Quantity = item.Value,
                        Name = _context.Items.FirstOrDefault(i => i.ItemId == item.Key)?.ItemName,
                        Price = _context.Items.FirstOrDefault(i => i.ItemId == item.Key)?.SuggestedPrice
                    }).ToList(),
                    TotalAmount = itemQuantityMap.Sum(item => item.Value * (_context.Items.FirstOrDefault(i => i.ItemId == item.Key)?.SuggestedPrice ?? 0))
                };
                int accId = acc.AccountId;
                // Save cart to session
                httpContext.Session.SetObjectAsJson(CartSessionKey, cart);

                // Create table and table details for the user
                var userTable = new Table()
                {
                    TableCapacity = 0,
                    TableMaintenance = 1,
                    TableType = 1,
                    TableStatus = "Deposit",
                    TableNote = "nothing to comment",
                    AccountId = accId
                };

                var tableUser = await _tableRepository.CreateAsync(userTable);
                int tbid = tableUser.TableId;

                var tableDetail = itemQuantityMap.Select(item => new TableDetail
                {
                    TableId = tableUser.TableId,
                    ItemId = item.Key,
                    Quantity = item.Value,

                }).ToList();

                await _tableDetailRepository.CreateRangeAsync(tableDetail);
                await _context.SaveChangesAsync();

                httpContext.Session.SetObjectAsJson("UserTable", tableUser);
                httpContext.Session.SetObjectAsJson("TableDetails", tableDetail);
                var billll = await _billRepository.CreateAsync(tbid);
                int idid = billll.BillId;
                return idid;
            } catch (InvalidOperationException) {
                // return TypedResults.BadRequest("del on roi");
                return 0;
            }
        }
    }
}
