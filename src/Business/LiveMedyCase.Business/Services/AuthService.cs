using System.Security.Claims;
using LiveMedyCase.Business.Services.Base;
using LiveMedyCase.Core.Constants;
using LiveMedyCase.Core.Models.Results;
using LiveMedyCase.Core.Utils;
using LiveMedyCase.Data.Model.Dtos;
using LiveMedyCase.Data.Model.Entities;
using LiveMedyCase.Data.Model.Enums;
using LiveMedyCase.Data.Repositories.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace LiveMedyCase.Business.Services
{
	public class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AuthService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
		{
			_userRepository = userRepository;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<IDataResultModel<UserInfoResponse>> GetSignInUserAsync()
		{
			var claims = _httpContextAccessor.HttpContext.User.Claims;
			if (claims == null || !claims.Any())
				return new DataResultModel<UserInfoResponse>(false, ServiceMessages.UserNotFound);

			if (!claims.Any(x => x.Type == ClaimTypes.Name))
				return new DataResultModel<UserInfoResponse>(false, ServiceMessages.UserNotFound);

			var claimUserName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)!.Value;

			var user = await _userRepository.GetAsync(x => x.Username == claimUserName);
			if (user == null)
				return new DataResultModel<UserInfoResponse>(false, ServiceMessages.UserNotFound);

			return new DataResultModel<UserInfoResponse>(new(user.Username, user.ReferralCode, user.Role));
		}

		public async Task<IDataResultModel<UserInfoResponse>> SignInAsync(UserLoginRequest model)
		{
			var user = await _userRepository.GetAsync(x => x.Username == model.Username);
			if (user == null)
				return new DataResultModel<UserInfoResponse>(false, ServiceMessages.PasswordOrUsernameError);

			if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
				return new DataResultModel<UserInfoResponse>(false, ServiceMessages.PasswordOrUsernameError);

			await SignOutAsync();
			await SetIdentity(user, model);

			return new DataResultModel<UserInfoResponse>(new UserInfoResponse(user.Username, user.ReferralCode, user.Role));

		}

		private async Task SetIdentity(User user, UserLoginRequest userdto)
		{
			var claims = new List<Claim>
			{
				new (ClaimTypes.Name,user.Username),
				new (ClaimTypes.Role,user.Role.ToString()),
				new (ClaimTypes.NameIdentifier,user.Id.ToString())
			};

			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var userPrincipal = new ClaimsPrincipal(claimsIdentity);
			var authProperties = new AuthenticationProperties();

			await _httpContextAccessor.HttpContext.SignInAsync(userPrincipal, authProperties);
			_httpContextAccessor.HttpContext.User.AddIdentity(claimsIdentity);
		}

		public async Task SignOutAsync()
		{
			await _httpContextAccessor.HttpContext.SignOutAsync();
		}

		public async Task<IDataResultModel<UserInfoResponse>> SignUpAsync(UserRegisterRequest model)
		{
			var userCheck = await _userRepository.GetAsync(x => x.Username == model.Username);
			if (userCheck != null)
				return new DataResultModel<UserInfoResponse>(false, ServiceMessages.UsernameCheckError);

			User? referrerUser = null;
			if (!string.IsNullOrWhiteSpace(model.Referrer))
			{
				referrerUser = await _userRepository.GetAsync(x => x.ReferralCode == model.Referrer);
				if (referrerUser == null)
					return new DataResultModel<UserInfoResponse>(false, ServiceMessages.UserReferrerInvalid);
			}

			var user = new User(username: model.Username,
								passwordHash: BCrypt.Net.BCrypt.HashPassword(model.Password),
								role: referrerUser != null ? RoleTypes.Manager : RoleTypes.Customer,
								referralCode: ReferralUtil.GenerateReferralCode());

			var result = await _userRepository.CreateAsync(user, referrerUser);

			return result
				? new DataResultModel<UserInfoResponse>(new UserInfoResponse(model.Username, user.ReferralCode, user.Role),
														true,
														ServiceMessages.UserRegistrationSucceeded)
				: new DataResultModel<UserInfoResponse>(false,
														ServiceMessages.UserRegistrationError);
		}
	}
}
