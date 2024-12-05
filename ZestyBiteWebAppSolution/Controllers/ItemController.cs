// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using System.Collections.Concurrent;
// using ZestyBiteWebAppSolution.Helpers;
// using ZestyBiteWebAppSolution.Models.DTOs;
// using ZestyBiteWebAppSolution.Services.Implementations;
// using ZestyBiteWebAppSolution.Services.Interfaces;

// namespace ZestyBiteWebAppSolution.Controllers
// {
//     [AllowAnonymous]
//     [ApiController]
//     [Route("api/[controller]")]
//     public class ItemController : Controller
//     {
//         private readonly IAccountService _service;
//         private readonly ILogger<AccountController> _logger;
//         private readonly IVerifyService _mailService;

//         private static readonly
//         ConcurrentDictionary<string, TaskCompletionSource<string>> VerificationTasks
//                 = new ConcurrentDictionary<string, TaskCompletionSource<string>>();
//         private static readonly
//         ConcurrentDictionary<string, int> VerificationAttempts
//                 = new ConcurrentDictionary<string, int>();

//         public AccountController(IVerifyService verifyService, ILogger<AccountController> logger, IAccountService accountService)
//         {
//             _logger = logger;
//             _service = accountService;
//             _mailService = verifyService;
//         }

//     }
// }