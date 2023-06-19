using System.Reflection.Metadata;
using LiveMedyCase.Core.Entities;

namespace LiveMedyCase.Data.Model.Entities
{
    public class Referral : BaseIdEntity<int>
    {
        public int UserId { get; set; }
		public User User { get; set; }

		public int ReffererUserId { get; set; }
		public User ReffererUser { get; set; }


		public Referral()
        {}
        public Referral(int userId, int reffererUserId, int id=default)
        {
			UserId = userId;
			ReffererUserId = reffererUserId;
			Id = id;
        }
    }
}
