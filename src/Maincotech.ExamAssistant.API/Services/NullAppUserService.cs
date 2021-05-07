using Maincotech.ExamAssistant.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Services
{
    public class NullAppUserService : IAppUserService
    {
        public Task<AppUserDto> CreateUserAsync(AppUserDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DisableUser(string uid)
        {
            throw new NotImplementedException();
        }

        public Task<ListDataResult<AppUserDto>> ListUsersAsync(string pageToken)
        {
            throw new NotImplementedException();
        }

        public Task ResetPassword(string email)
        {
            throw new NotImplementedException();
        }
    }
}
