using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using ZestyBiteWebAppSolution.Helpers;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Implementations;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : Controller
    {
        private readonly IItemService _service;
        private readonly ILogger<ItemController> _logger;

        public ItemController(IItemService service, ILogger<ItemController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // [Authorize(Roles = "Manager")]
        [AllowAnonymous]
        [HttpGet("viewmenu")]
        public async Task<IResult> ViewMenu()
        {
            try
            
            {
                var dishes = _service.ViewAllItem();
                if (dishes == null) return TypedResults.NotFound("No dishes here");
                return TypedResults.Ok(dishes);
            }
            catch
            {
                return TypedResults.BadRequest("Can not get the dishes");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("viewdish")]
        public async Task<IResult> ViewDish([FromBody] IdDTO dto)
        {
            try
            {
                var dish = _service.ViewOneDish(dto.Id);
                if (dish == null) return TypedResults.NotFound("No dishes here");
                return TypedResults.Ok(dish);
            }
            catch
            {
                return TypedResults.BadRequest("Can not get the dish");
            }
        }

        // [Authorize(Roles = "Manager")]
        [HttpPost]
        [Route("newdish")]
        public async Task<IResult> AddToMenu([FromBody] EItemDTO dto)
        {
            try
            {
                var dish = _service.CreateNewDish(dto);
                if (dish == null) return TypedResults.NotFound("No dishes here");
                return TypedResults.Ok(dish);
            }
            catch
            {
                return TypedResults.BadRequest("Can not add the dish to menyu");
            }
        }

        // [Authorize(Roles = "Manager")]
        [HttpPut]
        [Route("editdish")]
        public async Task<IResult> EditDish([FromBody] EItemDTO dto)
        {
            try
            {
                var dish = _service.ModifyDish(dto);
                if (dish == null) return TypedResults.NotFound("critical damage");
                return TypedResults.Ok(dish);
            }
            catch
            {
                return TypedResults.BadRequest("Dish in menu cant be modified");
            }
        }

        [HttpDelete, Route("delete")]
        public async Task<IResult> DeleteDish([FromBody] IdDTO dto)
        {
            try
            {
                bool res = _service.DeleteItemAsync(dto.Id).Result;
                if (res) return TypedResults.Ok("delete");
                else return TypedResults.BadRequest("un-deleted");
            }
            catch
            {
                return TypedResults.BadRequest("Can not del the dish in menu");
            }

        }
    }
}