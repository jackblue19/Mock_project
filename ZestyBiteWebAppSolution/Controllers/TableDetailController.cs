using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ZestyBiteWebAppSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableDetailController : ControllerBase
    {
        private readonly ITableDetailService _tableDetailService;

        public TableDetailController(ITableDetailService tableDetailService)
        {
            _tableDetailService = tableDetailService;
        }

 // GET: api/tabledetail
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TableDetailDTO>>> GetAllTableDetails()
        {
            try
            {
                var tableDetails = await _tableDetailService.GetAllTableDetailsAsync();
                return Ok(tableDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/tabledetail/{id}
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<TableDetailDTO>> GetTableDetailById(int id)
        {
            try
            {
                var tableDetail = await _tableDetailService.GetTableDetailByIdAsync(id);
                if (tableDetail == null)
                {
                    return NotFound(new { Message = "Table detail not found" });
                }
                return Ok(tableDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/tabledetail
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TableDetailDTO>> CreateTableDetail([FromBody] TableDetailDTO tableDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdTableDetail = await _tableDetailService.CreateTableDetailAsync(tableDetailDto);
                return CreatedAtAction(nameof(GetTableDetailById), new { id = createdTableDetail.TableId }, createdTableDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/tabledetail
        [HttpPut]
        public async Task<ActionResult<TableDetailDTO>> UpdateTableDetail([FromBody] TableDetailDTO tableDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedTableDetail = await _tableDetailService.UpdateTableDetailAsync(tableDetailDto);
                return Ok(updatedTableDetail);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/tabledetail/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTableDetail(int id)
        {
            try
            {
                var result = await _tableDetailService.DeleteTableDetailAsync(id);
                if (result)
                {
                    return NoContent();
                }
                return NotFound(new { Message = "Table detail not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}