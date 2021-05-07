using Maincotech.ExamAssistant.Dtos;
using Maincotech.ExamAssistant.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private ISettingService _service;

        public SettingsController(ISettingService settingService)
        {
            _service = settingService;
        }

        /// <summary>
        /// Create firebase settings
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("firebasesettings", Name = "CreateOrFirebaseSetting")]
        public async Task<FirebaseSettingDto> CreateOrFirebaseSetting(FirebaseSettingDto dto)
        {
            return await _service.CreateOrFirebaseSetting(dto);
        }

        /// <summary>
        /// Get user's firebase setting
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("firebasesettings/{userId}", Name = "GetFirebaseSetting")]
        public async Task<FirebaseSettingDto> GetFirebaseSetting(string userId)
        {
            return await _service.GetFirebaseSetting(userId);
        }
        [HttpPost("firebasesettings/validate", Name = "ValidateFirebaseSetting")]
        public bool ValidateFirebaseSetting(FirebaseSettingDto dto)
        {
            try
            {
                new ExamService(dto.ProjectId, dto.JsonCredentials);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}