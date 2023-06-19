using LiveMedyCase.Core.Models.Results;
using LiveMedyCase.Data.Model.Dtos;

namespace LiveMedyCase.Business.Services.Base
{
    public interface IAuthService
    {
        Task<IDataResultModel<UserInfoResponse>> GetSignInUserAsync();
        Task<IDataResultModel<UserInfoResponse>> SignInAsync(UserLoginRequest model);
        Task<IDataResultModel<UserInfoResponse>> SignUpAsync(UserRegisterRequest model);
        Task SignOutAsync();
    }
}
