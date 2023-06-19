using LiveMedyCase.Data.Model.Enums;

namespace LiveMedyCase.Data.Model.Dtos
{
    public record UserLoginRequest(string Username, string Password);

    public record UserRegisterRequest(string Username, string Password, string? Referrer);

    public record UserInfoResponse(string Username, string ReferralCode, RoleTypes Role);

    public record UserSummaryInfoDto(RoleTypes RoleType, int Count);
	public record UserSummaryInfoResponse(RoleTypes RoleType, int Count)
	{
		public string Role => RoleType.ToString();
	}
}
