using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Implementations;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        // GET: api/table/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTableByIdAsync(int id)
        {
            var table = await _tableService.GetTableByIdAsync(id);
            if (table == null)
            {
                return NotFound($"Table with ID {id} not found.");
            }

            // Convert the Table entity to TableDTO
            var tableDTO = new TableDTO
            {
                TableId = table.TableId,
                TableCapacity = table.TableCapacity,
                TableMaintenance = table.TableMaintenance,
                TableStatus = table.TableStatus,
                TableNote = table.TableNote,
                ReservationId = table.ReservationId,
                ItemId = table.ItemId,
                TableType = table.TableType,
                AccountId = table.AccountId
            };

            return Ok(tableDTO);
        }

        // GET: api/table
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAllTablesAsync()
        {
            var tables = await _tableService.GetAllTablesAsync();

            if (tables == null || !tables.Any())
            {
                return NotFound("No tables found.");
            }

            // Convert each Table entity to TableDTO
            //var tableDTOs = tables.Select(t => new TableDTO
            //{
            //    TableId = t.TableId,
            //    TableCapacity = t.TableCapacity,
            //    TableMaintenance = t.TableMaintenance,
            //    TableStatus = t.TableStatus,
            //    TableNote = t.TableNote,
            //    ReservationId = t.ReservationId,
            //    ItemId = t.ItemId,
            //    TableType = t.TableType,
            //    AccountId = t.AccountId
            //}).ToList();

            return Ok(tables);
        }

        // POST: api/table
        [HttpPost]
        public async Task<IActionResult> CreateTableAsync([FromBody] TableDTO tableDTO)
        {
            if (tableDTO == null)
            {
                return BadRequest("Table data is null.");
            }

            // Convert TableDTO to Table entity
            var table = new Table
            {
                TableCapacity = tableDTO.TableCapacity,
                TableMaintenance = tableDTO.TableMaintenance,
                TableStatus = tableDTO.TableStatus,
                TableNote = tableDTO.TableNote,
                ReservationId = tableDTO.ReservationId,
                ItemId = tableDTO.ItemId,
                TableType = tableDTO.TableType,
                AccountId = tableDTO.AccountId
            };

            var createdTable = await _tableService.CreateTableAsync(table);

            // Convert created Table entity to TableDTO
            var createdTableDTO = new TableDTO
            {
                TableId = createdTable.TableId,
                TableCapacity = createdTable.TableCapacity,
                TableMaintenance = createdTable.TableMaintenance,
                TableStatus = createdTable.TableStatus,
                TableNote = createdTable.TableNote,
                ReservationId = createdTable.ReservationId,
                ItemId = createdTable.ItemId,
                TableType = createdTable.TableType,
                AccountId = createdTable.AccountId
            };

            return CreatedAtAction(nameof(GetTableByIdAsync), new { id = createdTable.TableId }, createdTableDTO);
        }

        // PUT: api/table/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTableAsync(int id, [FromBody] TableDTO tableDTO)
        {
            if (tableDTO == null)
            {
                return BadRequest("Table data is null.");
            }

            try
            {
                var table = new Table
                {
                    TableId = id, // Ensure the ID is passed as part of the update
                    TableCapacity = tableDTO.TableCapacity,
                    TableMaintenance = tableDTO.TableMaintenance,
                    TableStatus = tableDTO.TableStatus,
                    TableNote = tableDTO.TableNote,
                    ReservationId = tableDTO.ReservationId,
                    ItemId = tableDTO.ItemId,
                    TableType = tableDTO.TableType,
                    AccountId = tableDTO.AccountId
                };

                var updatedTable = await _tableService.UpdateTableAsync(id, table);

                var updatedTableDTO = new TableDTO
                {
                    TableId = updatedTable.TableId,
                    TableCapacity = updatedTable.TableCapacity,
                    TableMaintenance = updatedTable.TableMaintenance,
                    TableStatus = updatedTable.TableStatus,
                    TableNote = updatedTable.TableNote,
                    ReservationId = updatedTable.ReservationId,
                    ItemId = updatedTable.ItemId,
                    TableType = updatedTable.TableType,
                    AccountId = updatedTable.AccountId
                };

                return Ok(updatedTableDTO);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/table/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTableAsync(int id)
        {
            try
            {
                bool isDeleted = await _tableService.DeleteTableAsync(id);
                if (isDeleted)
                {
                    return NoContent(); // Successful deletion
                }
                return NotFound($"Table with ID {id} not found.");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
