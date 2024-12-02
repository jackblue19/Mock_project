using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZestyBiteWebAppSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly TableService _tableService;

        public TableController(TableService tableService)
        {
            _tableService = tableService;
        }

        // Create
        [HttpPost]
        public async Task<ActionResult<Table>> CreateTable([FromBody] Table table)
        {
            var createdTable = await _tableService.CreateTableAsync(table);
            return CreatedAtAction(nameof(GetTableById), new { id = createdTable.TableId }, createdTable);
        }

        // Read
        [HttpGet("{id}")]
        public async Task<ActionResult<Table>> GetTableById(int id)
        {
            var table = await _tableService.GetTableByIdAsync(id);
            if (table == null)
            {
                return NotFound();
            }
            return Ok(table);
        }

        [HttpGet]
        public async Task<ActionResult<List<Table>>> GetAllTables()
        {
            var tables = await _tableService.GetAllTablesAsync();
            return Ok(tables);
        }

        // Update
        [HttpPut("{id}")]
        public async Task<ActionResult<Table>> UpdateTable(int id, [FromBody] Table table)
        {
            if (id != table.TableId)
            {
                return BadRequest();
            }

            var updatedTable = await _tableService.UpdateTableAsync(table);
            return Ok(updatedTable);
        }

        // Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTable(int id)
        {
            var success = await _tableService.DeleteTableAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
