using Maincotech.ExamAssitant.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maincotech.ExamAssitant.Services
{
    public interface ISettingService
    {
        Task<FirebaseSettingDto> CreateOrFirebaseSetting(FirebaseSettingDto dto);
        Task<FirebaseSettingDto> GetFirebaseSetting(string userID);
    }
}
