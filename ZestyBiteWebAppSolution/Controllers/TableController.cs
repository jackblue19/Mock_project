using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : ControllerBase {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService) {
            _tableService = tableService;
        }

        // fix
        [HttpGet("{tableId}/items")] 
        public async Task<ActionResult<IEnumerable<TableDetailDTO>>> GetTableItemsByTableId(int tableId) 
        { 
            var tableItems = await _tableService.GetTableItemsByTableIdAsync(tableId); 
            return Ok(tableItems); 
        }

        // GET: api/table
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TableDTO>>> GetAllTables() {
            try {
                var tables = await _tableService.GetAllTablesAsync();
                return Ok(tables);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/table/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TableDTO>> GetTableById(int id) {
            try {
                var table = await _tableService.GetTableByIdAsync(id);
                if (table == null) {
                    return NotFound(new { Message = "Table not found" });
                }
                return Ok(table);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // fix
        [HttpPost]
        public async Task<ActionResult<TableDTO>> CreateTable([FromBody] TableDTO tableDto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            try {
                var createdTable = await _tableService.CreateTableAsync(tableDto);
                return CreatedAtAction(nameof(GetTableById), new { id = createdTable.TableId }, createdTable);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // fix => table id status must be "empty"
        [HttpPut]
        public async Task<ActionResult<TableDTO>> UpdateTable([FromBody] TableDTO tableDto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            try {
                var updatedTable = await _tableService.UpdateTableAsync(tableDto);
                return Ok(updatedTable);
            } catch (InvalidOperationException ex) {
                return NotFound(new { Message = ex.Message });
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // fix
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTable(int id) {
            try {
                var result = await _tableService.DeleteTableAsync(id);
                if (result) {
                    return NoContent();
                }
                return NotFound(new { Message = "Table not found" });
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Existing methods...

        // GET: api/table/reality
        [HttpGet("reality")]
        public async Task<ActionResult<IEnumerable<TableDTO>>> GetAllRealTables() {
            try {
                var tables = await _tableService.GetAllRealTablesAsync();
                return Ok(tables);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Addition

        // GET: api/table/virtual
        [HttpGet("virtual")]
        public async Task<ActionResult<IEnumerable<TableDTO>>> GetAllVirtualTables() {
            try {
                var tables = await _tableService.GetAllVirtualTablesAsync();
                return Ok(tables);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}