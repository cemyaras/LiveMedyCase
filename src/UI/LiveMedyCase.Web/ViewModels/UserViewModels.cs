using System.ComponentModel.DataAnnotations;
using LiveMedyCase.Data.Model.Enums;

namespace LiveMedyCase.Web.ViewModels
{
    public class UserLoginViewModel
    {
        [Required]
        public string Username { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		public string Token { get; set; }
    }

    public class UserRegisterViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
		[Compare("Password")]
		public string ConfirmPassword { get; set; }
        public string? Referrer { get; set; }
        [Required]
        public string Token { get; set; }
    }

    public class UserInfoViewModel
    {
		public string ReferralCode { get; set; }
		public string Username { get; set; }
		public RoleTypes RoleType { get; set; }
		public string Role => RoleType.ToString();
	}
}
