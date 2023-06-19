using LiveMedyCase.Business.Services.Base;
using LiveMedyCase.Core.Models.Results;
using LiveMedyCase.Data.Model.Dtos;
using LiveMedyCase.Data.Repositories.Base;

namespace LiveMedyCase.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IDataResultModel<List<UserInfoResponse>>> GetListAsync()
        {
            var userList = await _userRepository.GetListAsync();
            var responses = userList.Select(x => new UserInfoResponse(x.Username,x.ReferralCode, x.Role)).ToList();
            return new DataResultModel<List<UserInfoResponse>>(responses);
        }

        public async Task<IDataResultModel<List<UserSummaryInfoResponse>?>> GetUserSummariesAsync()
        {
            var summaries = await _userRepository.GetUserSummariesAsync();
            if(summaries==null)
				return new DataResultModel<List<UserSummaryInfoResponse>?>(false,string.Empty);

            var list = summaries
                        .Select(x => new UserSummaryInfoResponse(x.RoleType, x.Count))
                        .ToList();
			return new DataResultModel<List<UserSummaryInfoResponse>?>(list);
		}

	}
}
