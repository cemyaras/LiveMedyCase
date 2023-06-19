using LiveMedyCase.Core.Utils;
using LiveMedyCase.Data.Model.Entities;
using LiveMedyCase.Data.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace LiveMedyCase.Data.Seeds
{
	public static class ModelBuilderSeedExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var referralCode = ReferralUtil.GenerateReferralCode();
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("admin");
			modelBuilder.Entity<User>().HasData(new User(username: "admin",
                                                         passwordHash: passwordHash,
                                                         referralCode: referralCode,
														 role: RoleTypes.Admin,
                                                         id: 1));
        }
    }
}
