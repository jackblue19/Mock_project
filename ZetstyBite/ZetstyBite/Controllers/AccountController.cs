using Microsoft.AspNetCore.Mvc;
using ZetstyBite.Services;
using ZetstyBite.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using ZetstyBite.Services.Implementations;
using Microsoft.AspNetCore.Http; // For StatusCodes
using ZetstyBite.Models.Entities;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using ZetstyBite.Models.DTOs;

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
        public async Task<ActionResult<Account>> SignUpAction([FromBody] AccountDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdAccount = await _service.SignUpAsync(dto);
                return CreatedAtAction(nameof(SignUpAction), new { id = createdAccount.AccountId }, createdAccount);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpPost("signup2")]
        public async Task<IResult> SignUpSecond([FromBody] AccountDTO acc)
        {
            try
            {
                var created = await _service.SignUpAsync(acc);
                return TypedResults.Created($"/api/account/{created.AccountId}", created);
            }
            catch (InvalidOperationException ex)
            {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }
        [HttpGet("api/account/{id}")]
        public async Task<IResult> ViewProfile(int id)
        {
            try
            {
                var existed = await _service.GetAccountById(id);
                if(existed == null){
                    return TypedResults.NotFound();
                }
                return TypedResults.Ok(existed);
            }
            catch (Exception e)
            {
                return TypedResults.BadRequest(new { Message = e.Message });
                // return TypedResults.BadRequest();
            }
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
