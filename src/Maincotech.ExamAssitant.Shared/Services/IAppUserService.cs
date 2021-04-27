using Maincotech.ExamAssistant.Dtos;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Services
{
    public interface IAppUserService
    {
        Task<ListDataResult<AppUserDto>> ListUsersAsync(string pageToken);

        Task<AppUserDto> CreateUserAsync(AppUserDto dto);

        Task DisableUser(string uid);

        Task ResetPassword(string email);
    }
}