using LiveMedyCase.Core.Entities;
using LiveMedyCase.Data.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace LiveMedyCase.Data.Model.Entities
{
	[Index(nameof(ReferralCode), IsUnique = true)]
	public class User : BaseIdEntity<int>
	{
		public string Username { get; set; }
		public string PasswordHash { get; set; }
		public string ReferralCode { get; set; }
		public RoleTypes Role { get; set; }

		public ICollection<Referral> Referral { get; set; }
		public ICollection<Referral> Referrer { get; set; }

		public User()
		{}
		public User(string username,
					string passwordHash,
					RoleTypes role,
					string referralCode,
					int id = default)
		{
			Username = username;
			PasswordHash = passwordHash;
			Role = role;
			ReferralCode = referralCode;
			Id = id;
		}
	}
}
