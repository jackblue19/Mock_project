using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplyController : ControllerBase
    {
        private readonly ISupplyService _supplyService;

        public SupplyController(ISupplyService supplyService)
        {
            _supplyService = supplyService;
        }

        // GET: api/Supply
        [HttpGet]
        public async Task<IActionResult> GetAllSupplies()
        {
            var supplies = await _supplyService.GetAllSuppliesAsync();
            return Ok(supplies);
        }

        // GET: api/Supply/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSupplyById(int id)
        {
            try
            {
                var supply = await _supplyService.GetSupplyByIdAsync(id);
                return Ok(supply);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        // GET: api/Supply/table/{tableId}
        [HttpGet("table/{tableId:int}")]
        public async Task<IActionResult> GetSupplyByTableId(int tableId)
        {
            try
            {
                // Call the service method to get the supply based on TableId
                var supply = await _supplyService.GetSupplyByTableIdAsync(tableId);

                // If no supply is found, return 404 Not Found
                if (supply == null)
                {
                    return NotFound($"Supply with TableId {tableId} not found.");
                }

                // Return the supply data if found
                return Ok(supply);
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



        // POST: api/Supply
        [HttpPost("create")]
        public async Task<IActionResult> CreateSupply([FromBody] SupplyDTO supplyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdSupply = await _supplyService.CreateSupplyAsync(supplyDto);
                return CreatedAtAction(nameof(GetSupplyById), new { id = createdSupply.SupplyId }, createdSupply);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }




        // PUT: api/Supply/{id}
        [HttpPut("update/{supplyId:int}")]
        public async Task<IActionResult> UpdateSupply(int supplyId, [FromBody] SupplyDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid supply data.");
            }

            try
            {
                // Update the supply using the service method
                var updatedSupply = await _supplyService.UpdateSupplyAsync(dto, supplyId);

                // Return the updated supply DTO
                return Ok(updatedSupply);
            }
            catch (KeyNotFoundException ex)
            {
                // If supply is not found, return a 404 Not Found
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Catch other potential errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Supply/{id}
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteSupply(int id)
        {
            try
            {
                var result = await _supplyService.DeleteSupplyAsync(id);
                if (result)
                {
                    return NoContent();
                }
                return NotFound(new { Message = "Supply not found" });
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
