using System.Data;
using LiveMedyCase.Business.Services.Base;
using LiveMedyCase.Core.Constants;
using LiveMedyCase.Data.Model.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiveMedyCase.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly IAuthService _authService;
		private readonly IUserService _userService;

		public HomeController(IAuthService authService, IUserService userService)
		{
			_authService = authService;
			_userService = userService;
		}

		public async Task<IActionResult> Index()
		{
			var result = await _authService.GetSignInUserAsync();
			if (result.Success)
			{
				if (result.Data?.Role == RoleTypes.Admin)
					return RedirectToAction("AdminDashboard", "Home");
				else
					return RedirectToAction("Dashboard", "Home");
			}

			return RedirectToAction("SignIn", "Auth");
		}

		[Authorize(Roles = RoleConsts.AdminRole)]
		public async Task<IActionResult> AdminDashboard()
		{
			var result = await _userService.GetUserSummariesAsync();
			if (result.Success)
				return View(result.Data);
			else
				return View();
		}

		[Authorize]
		public async Task<IActionResult> Dashboard()
		{
			return View();
		}

	}
}
