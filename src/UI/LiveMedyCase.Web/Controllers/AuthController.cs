using LiveMedyCase.Business.Services.Base;
using LiveMedyCase.Web.Utils.ReCaptcha;
using LiveMedyCase.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LiveMedyCase.Web.Controllers
{
	public class AuthController : Controller
	{
		private readonly IAuthService _authService;
		private readonly IRecaptchaValidator _recaptchaValidator;

		public AuthController(IAuthService authService, IRecaptchaValidator recaptchaValidator)
		{
			_authService = authService;
			_recaptchaValidator = recaptchaValidator;
		}

		public IActionResult SignIn()
		{
			return View(new UserLoginViewModel());
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> SignIn(UserLoginViewModel model)
		{
			if (!_recaptchaValidator.IsRecaptchaValid(model.Token))
			{
				TempData["Message"] = "Recaptcha Error";
				return View(model);
			}

			if (ModelState.IsValid)
			{
				var result = await _authService.SignInAsync(new(Username: model.Username,
																Password: model.Password));
				if (!result.Success)
				{
					TempData["Message"] = result.Message;
					return View(model);
				}

				return RedirectToAction("Index", "Home");
			}

			TempData["Message"] = "Login Error";
			return View(model);
		}

		[EnableRateLimiting("ReferralRate")]
		public IActionResult SignUp(string referralCode)
		{
			return View(new UserRegisterViewModel { Referrer = referralCode });
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> SignUp(UserRegisterViewModel model)
		{
			if (!_recaptchaValidator.IsRecaptchaValid(model.Token))
			{
				TempData["Message"] = "Recaptcha Error";
				return View(model);
			}

			if (ModelState.IsValid)
			{
				var result = await _authService.SignUpAsync(new(Username: model.Username,
																Password: model.Password,
																Referrer: model.Referrer));
				if (result.Success)
				{
					TempData["Message"] = result.Message;
					return RedirectToAction("SignIn", "Auth");
				}
				else
				{
					TempData["Message"] = result.Message;
					return View(model);
				}
			}

			TempData["Message"] = "Registration error";
			return View(model);
		}

		public new async Task<IActionResult> SignOut()
		{
			await _authService.SignOutAsync();
			TempData["Message"] = "Has Been Sign Out";
			return RedirectToAction("SignIn", "Auth");
		}
	}
}
