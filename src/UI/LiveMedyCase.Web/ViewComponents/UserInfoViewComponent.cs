using LiveMedyCase.Business.Services.Base;
using LiveMedyCase.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LiveMedyCase.Web.ViewComponents
{
	public class UserInfoViewComponent : ViewComponent
	{
		private readonly IAuthService _authService;

		public UserInfoViewComponent(IAuthService authService)
		{
			_authService = authService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var result = await _authService.GetSignInUserAsync();
			if (result.Success)
				return View(new UserInfoViewModel
				{
					Username = result.Data!.Username,
					RoleType = result.Data!.Role,
					ReferralCode = result.Data!.ReferralCode
				});

			return View();
		}
	}
}
