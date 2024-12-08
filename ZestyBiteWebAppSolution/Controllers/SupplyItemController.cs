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
    public class SupplyItemController : ControllerBase
    {
        private readonly ISupplyItemService _supplyItemService;

        public SupplyItemController(ISupplyItemService supplyItemService)
        {
            _supplyItemService = supplyItemService;
        }

        // GET: api/supplyitem
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplyItemDTO>>> GetAllSupplyItems()
        {
            try
            {
                var supplyItems = await _supplyItemService.GetAllSupplyItemsAsync();
                return Ok(supplyItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //    // GET: api/supplyitem/{id}
        //    [AllowAnonymous]
        //    [HttpGet("{id}")]
        //    public async Task<ActionResult<SupplyItemDTO>> GetSupplyItemById(int id)
        //    {
        //        try
        //        {
        //            var supplyItem = await _supplyItemService.GetSupplyItemByIdAsync(id);
        //            if (supplyItem == null)
        //            {
        //                return NotFound(new { Message = "Supply item not found" });
        //            }
        //            return Ok(supplyItem);
        //        }
        //        catch (Exception ex)
        //        {
        //            return StatusCode(500, $"Internal server error: {ex.Message}");
        //        }
        //    }

        //    // POST: api/supplyitem
        //    [Authorize]
        //    [HttpPost]
        //    public async Task<ActionResult<SupplyItemDTO>> CreateSupplyItem([FromBody] SupplyItemDTO supplyItemDto)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        try
        //        {
        //            var createdSupplyItem = await _supplyItemService.CreateSupplyItemAsync(supplyItemDto);
        //            return CreatedAtAction(nameof(GetSupplyItemById), new { id = createdSupplyItem.SupplyId }, createdSupplyItem);
        //        }
        //        catch (Exception ex)
        //        {
        //            return StatusCode(500, $"Internal server error: {ex.Message}");
        //        }
        //    }

        //    // PUT: api/supplyitem
        //    [HttpPut]
        //    public async Task<ActionResult<SupplyItemDTO>> UpdateSupplyItem([FromBody] SupplyItemDTO supplyItemDto)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        try
        //        {
        //            var updatedSupplyItem = await _supplyItemService.UpdateSupplyItemAsync(supplyItemDto);
        //            return Ok(updatedSupplyItem);
        //        }
        //        catch (InvalidOperationException ex)
        //        {
        //            return NotFound(new { Message = ex.Message });
        //        }
        //        catch (Exception ex)
        //        {
        //            return StatusCode(500, $"Internal server error: {ex.Message}");
        //        }
        //    }

        //    // DELETE: api/supplyitem/{id}
        //    [HttpDelete("{id}")]
        //    public async Task<ActionResult> DeleteSupplyItem(int id)
        //    {
        //        try
        //        {
        //            var result = await _supplyItemService.DeleteSupplyItemAsync(id);
        //            if (result)
        //            {
        //                return NoContent();
        //            }
        //            return NotFound(new { Message = "Supply item not found" });
        //        }
        //        catch (Exception ex)
        //        {
        //            return StatusCode(500, $"Internal server error: {ex.Message}");
        //        }
        //    }
        //}
    }
}
