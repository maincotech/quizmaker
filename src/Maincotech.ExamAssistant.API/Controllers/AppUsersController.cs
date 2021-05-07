using Maincotech.ExamAssistant.Dtos;
using Maincotech.ExamAssistant.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppUsersController : ControllerBase
    {
        private IAppUserService _service;

        public AppUsersController(IAppUserService appUserService)
        {
            _service = appUserService;
        }

        /// <summary>
        /// List users
        /// </summary>
        /// <param name="pageToken"></param>
        /// <returns></returns>
        [HttpGet(Name = "ListUsers")]
        public async Task<ListDataResult<AppUserDto>> ListUsers(string pageToken)
        {
            return await _service.ListUsersAsync(pageToken);
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost(Name = "CreateUser")]
        public async Task<AppUserDto> CreateUser(AppUserDto dto)
        {
            return await _service.CreateUserAsync(dto);
        }

        /// <summary>
        /// Disable user
        /// </summary>
        /// <param name="uid">The user id</param>
        /// <returns></returns>
        [HttpPatch("{uid}/disable", Name = "DisableUser")]
        public async Task DisableUser(string uid)
        {
            await _service.DisableUser(uid);
        }


        /// <summary>
        /// Reset user's password
        /// </summary>
        /// <param name="email">The email address</param>
        /// <returns></returns>
        [HttpPost("{email}/reset", Name = "ResetPassword")]
        public async Task ResetPassword(string email)
        {
            await _service.ResetPassword(email);
        }
    }
}