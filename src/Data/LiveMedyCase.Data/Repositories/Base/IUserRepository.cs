using System.Linq.Expressions;
using LiveMedyCase.Data.Model.Dtos;
using LiveMedyCase.Data.Model.Entities;

namespace LiveMedyCase.Data.Repositories.Base
{
	public interface IUserRepository
	{
		Task<bool> CreateAsync(User entity, User? referrerUser);
		Task<User?> GetAsync(Expression<Func<User, bool>> filter);
		Task<IList<User>> GetListAsync(Expression<Func<User, bool>>? filter = null);
		Task<List<UserSummaryInfoDto>> GetUserSummariesAsync();
	}
}
