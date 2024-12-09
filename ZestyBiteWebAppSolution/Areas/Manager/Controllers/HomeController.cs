using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Areas.Manager.Controllers {
    
    [Area("Manager")]
    public class HomeController : Controller {
        private readonly IAccountService _accountService;
        private readonly IBillService _billService;
        public HomeController(IAccountService accountService, IBillService billService) {
            _accountService = accountService;
            _billService = billService;
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
                var accounts = await _accountService.GetALlAccAsync();
                if (!accounts.Any()) return TypedResults.NotFound();
                return TypedResults.Ok(accounts);

            } catch (InvalidOperationException ex) {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        [Route("api/account/addstaff")]
        public async Task<IResult> CreateNewStaffAccount([FromBody] StaffDTO dto) {
                var staffAccount = await _accountService.MapFromDTO(dto);
                await _accountService.CreateStaffAsync(staffAccount);
                return TypedResults.Ok(staffAccount);
             
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete]
        [Route("api/account/deleteacc")]
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
        [Route("api/account/status")]
        public async Task<IResult> ManageStatus([FromBody] StatusDTO dto) {
            try {
                if (await _accountService.ChangeAccStatus(dto.Username)) 
                    return TypedResults.Ok("Changed the status");
                else return TypedResults.Ok("Fail to change status");
            } catch (InvalidOperationException ex) {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/bill/getall")]
        public async Task<IResult> GetAllBill() {
            try {
                var bbb = await _billService.GetALlAccAsync();
                return TypedResults.Ok(bbb);

            } catch (InvalidOperationException ex) {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/bill/GetBillByTableId/{id}")]
        public async Task<IResult> GetBillByTableId(int id) {
            var usn = User.Identity.Name;
            var ussn = await _accountService.GetByUsn(usn);
            var res = await _billService.GetBillByTableId(id, usn);
            if (res == null) {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(res);
        }
    }
}