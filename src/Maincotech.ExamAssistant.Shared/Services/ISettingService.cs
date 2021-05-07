using Maincotech.ExamAssistant.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Services
{
    public interface ISettingService
    {
        Task<FirebaseSettingDto> CreateOrFirebaseSetting(FirebaseSettingDto dto);
        Task<FirebaseSettingDto> GetFirebaseSetting(string userID);
    }
}
