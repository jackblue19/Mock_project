using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZestyBiteWebAppSolution.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ZestyBiteWebAppSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        // Create
        [HttpPost]
        public async Task<ActionResult<Table>> CreateTableAsync([FromBody] Table table)
        {
            if (table == null)
            {
                return BadRequest("Table data is required.");
            }

            var createdTable = await _tableService.CreateTableAsync(table);
            return CreatedAtAction(nameof(GetTableByIdAsync), new { tableId = createdTable.TableId }, createdTable);
        }

        // Read
        [HttpGet("{tableId}")]
        public async Task<ActionResult<Table>> GetTableByIdAsync(int tableId)
        {
            var table = await _tableService.GetTableByIdAsync(tableId);
            if (table == null)
            {
                return NotFound($"Table with ID {tableId} not found.");
            }

            return Ok(table);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Table>>> GetAllTablesAsync()
        {
            var tables = await _tableService.GetAllTablesAsync();
            return Ok(tables);
        }

        // Update
        [HttpPut("{tableId}")]
        public async Task<ActionResult<Table>> UpdateTableAsync(int tableId, [FromBody] Table table)
        {
            if (tableId != table.TableId)
            {
                return BadRequest("Table ID mismatch.");
            }

            var updatedTable = await _tableService.UpdateTableAsync(table);
            return Ok(updatedTable);
        }

        // Delete
        [HttpDelete("{tableId}")]
        public async Task<ActionResult> DeleteTableAsync(int tableId)
        {
            var isDeleted = await _tableService.DeleteTableAsync(tableId);
            if (!isDeleted)
            {
                return NotFound($"Table with ID {tableId} not found.");
            }

            return NoContent(); // Successfully deleted
        }
    }
}
