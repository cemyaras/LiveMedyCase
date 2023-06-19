using System.Linq.Expressions;
using LiveMedyCase.Data.Contexts;
using LiveMedyCase.Data.Model.Dtos;
using LiveMedyCase.Data.Model.Entities;
using LiveMedyCase.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace LiveMedyCase.Data.Repositories
{
	public class UserRepository : IUserRepository
	{
		public AppDbContext Context { get; set; }
		protected DbSet<User> Entities { get; set; }

		public UserRepository(AppDbContext context)
		{
			Context = context;
			Entities = Context.Set<User>();
		}

		public async Task<List<UserSummaryInfoDto>> GetUserSummariesAsync()
		{
			return await Entities
							.GroupBy(x => x.Role)
							.Select(g => new UserSummaryInfoDto(g.Key, g.Count()))
			.ToListAsync();
		}

		public async Task<User?> GetAsync(Expression<Func<User, bool>> filter)
		{
			return await Entities.FirstOrDefaultAsync(filter);
		}

		public async Task<IList<User>> GetListAsync(Expression<Func<User, bool>>? filter = null)
		{
			var list = filter != null
				? Entities.Where(filter)
			: Entities;

			return await list.ToListAsync();
		}

		public async Task<bool> CreateAsync(User entity, User? referrerUser)
		{
			if (referrerUser == null)
			{
				await Entities.AddAsync(entity);
				return await SaveAsync();
			}

			using var transaction = await Context.Database.BeginTransactionAsync();
			try
			{
				await Entities.AddAsync(entity);
				await SaveAsync();

				await Context.Set<Referral>().AddAsync(new Referral(entity.Id, referrerUser.Id));
				await SaveAsync();

				await transaction.CommitAsync();
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		private async Task<bool> SaveAsync()
		{
			return await Context.SaveChangesAsync() > 0;
		}
	}
}
