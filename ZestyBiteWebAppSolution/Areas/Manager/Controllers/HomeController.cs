using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Areas.Manager.Controllers {
    [Area("Manager")]
    public class HomeController : Controller {
        private readonly IAccountService _accountService;
        public HomeController(IAccountService accountService) {
            _accountService = accountService;
        }
        public IActionResult Index() {
            return View();
        }
        public IActionResult MenuManager() {
            return View();
        }
        public IActionResult payHistory() {
            return View();
        }
        public IActionResult AccountManagement() {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("api/account/getallacc")]
        public async Task<IResult> GetAllAccount() {
            try {
                var accounts = await _accountService.GetALlAccountAsync();
                if (!accounts.Any()) return TypedResults.NotFound();
                return TypedResults.Ok(accounts);

            } catch (InvalidOperationException ex) {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }
        [Authorize(Roles = "Manager")]
        [HttpPost]
        [Route("addstaff")]
        public async Task<IResult> CreateNewStaffAccount([FromBody] StaffDTO dto) {
            try {
                var staffAccount = await _accountService.MapFromDTO(dto);
                await _accountService.CreateStaffAsync(staffAccount);
                return TypedResults.Ok(staffAccount);
            } catch (InvalidOperationException ex) {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }
        [Authorize(Roles = "Manager")]
        [HttpDelete]
        [Route("deleteacc")]
        public async Task<IResult> DeleteAnAccount([FromBody] StatusDTO dto) {
            try {
                if (await _accountService.DeleteAcc(dto.Username))
                    return TypedResults.Ok("delete done");
                return TypedResults.Ok("fail to delete");
            } catch (InvalidOperationException ex) {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }
        [Authorize(Roles = "Manager")]
        [HttpGet]
        [Route("api/account/{username}")]
        public async Task<IResult> ViewProfileDetail(string username) {
            try {
                var dto = await _accountService.GetAccountByUsnAsync(username);
                if (dto == null)
                    return TypedResults.NotFound("Account not found");
                return TypedResults.Ok(dto);
            } catch {
                return TypedResults.BadRequest("Account not found");
            }
        }
        [Authorize(Roles = "Manager")]
        [HttpPut]
        [Route("status")]
        public async Task<IResult> ManageStatus([FromBody] StatusDTO dto) {
            try {
                if (await _accountService.ChangeAccStatus(dto.Username)) return TypedResults.Ok("Changed the status");
                else return TypedResults.Ok("Fail to change status");
            } catch (InvalidOperationException ex) {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }
    }
}