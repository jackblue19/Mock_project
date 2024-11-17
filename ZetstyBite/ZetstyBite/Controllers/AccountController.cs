using Microsoft.AspNetCore.Mvc;
using ZetstyBite.Services;
using ZetstyBite.Models.DTOs;
using ZetstyBite.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using ZetstyBite.Services.Implementations;
using Microsoft.AspNetCore.Http; // For StatusCodes


namespace ZetstyBite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService accountService)
        {
            _service = accountService;
        }

        // no typedResult
        [HttpPost("signup")]
        public async Task<ActionResult<AccountDTO>> SignUpAction([FromBody] AccountDTO dto)
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdAccount = await _service.SignUpAsync(dto);
                return CreatedAtAction(nameof(SignUpAction) , new { id = createdAccount.Id } , createdAccount);
            }
            catch ( InvalidOperationException ex )
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        
        //  with TypedResult sol 1:
        [HttpPost("signup1")]
        public async Task<Results<Created<AccountDTO> , BadRequest>> SignUpFirst([FromBody] AccountDTO dto)
        {
            try
            {
                var created = await _service.SignUpAsync(dto);
                return TypedResults.Created($"/api/account/{created.Id}" , created);
            }
            catch ( InvalidOperationException ex )
            {
                return TypedResults.BadRequest();
            }
        }


        //  with TypedResult sol 1:
        [HttpPost("signup2")]
        public async Task<IResult> SignUpSecond([FromBody] AccountDTO accountDto)
        {
            try
            {
                var created = await _service.SignUpAsync(accountDto);
                return TypedResults.Created($"/api/account/{created.Id}" , created);
            }
            catch ( InvalidOperationException ex )
            {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }

    
        public IActionResult Index()
        {
            return View();
        }
    }
}
