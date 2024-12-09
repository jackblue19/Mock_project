using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Interfaces;
using ZestyBiteWebAppSolution.Services.Implementations;

namespace ZestyBiteSolution.Areas.Food_Runner.Controllers {
    [Area("Food_Runner")]
    public class HomeController : Controller {

        private readonly IAccountService _accountService;
        private readonly ITableDetailService _tableDetailService;
        private readonly ITableService _tableService;
        public HomeController(IAccountService accountService, ITableDetailService tableDetailService, ITableService tableService)
        {
            _accountService = accountService;
            _tableDetailService = tableDetailService;
            _tableService = tableService;
        }
        public IActionResult Index() {
            return View();
        }

        [Authorize(Roles = "FoodRunner")]
        [HttpGet]
        [Route("api/account/{username}")]
        public async Task<IResult> ViewProfileDetail(string username)
        {
            try
            {
                var dto = await _accountService.GetAccountByUsnAsync(username);
                if (dto == null)
                    return TypedResults.NotFound("Account not found");
                return TypedResults.Ok(dto);
            }
            catch
            {
                return TypedResults.BadRequest("Account not found");
            }
        }
        // GET: api/table/reality
        [HttpGet("reality")]
        public async Task<ActionResult<IEnumerable<TableDTO>>> GetAllRealTables()
        {
            try
            {
                var tables = await _tableService.GetAllRealTablesAsync();
                return Ok(tables);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Addition

        // GET: api/table/virtual
        [HttpGet("virtual")]
        public async Task<ActionResult<IEnumerable<TableDTO>>> GetAllVirtualTables()
        {
            try
            {
                var tables = await _tableService.GetAllVirtualTablesAsync();
                return Ok(tables);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}