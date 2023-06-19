using LiveMedyCase.Core.Models.Results;
using LiveMedyCase.Data.Model.Dtos;

namespace LiveMedyCase.Business.Services.Base
{
    public interface IUserService
    {
        Task<IDataResultModel<List<UserInfoResponse>>> GetListAsync();
		Task<IDataResultModel<List<UserSummaryInfoResponse>?>> GetUserSummariesAsync();
	}
}
