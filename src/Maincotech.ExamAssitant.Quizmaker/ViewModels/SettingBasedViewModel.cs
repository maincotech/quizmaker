using Maincotech.ExamAssistant.Dtos;
using Maincotech.ExamAssistant.Services;
using Maincotech.Utilities;
using ReactiveUI;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.ViewModels
{
    public abstract class SettingBasedViewModel : ReactiveObject
    {
        public string UserId { get; }
        private readonly ISettingService _settingService;
        protected FirebaseSettingDto firesbaseSettings;

        public SettingBasedViewModel(string userId)
        {
            UserId = userId;
            _settingService = AppRuntimeContext.Current.Resolve<ISettingService>();
        }

        public async Task InitAsync()
        {
            //load setting
            firesbaseSettings = await _settingService.GetFirebaseSetting(UserId);
            ParameterChecker.Against<NotConfiguredException>(firesbaseSettings == null, "The firebase settings have not been configured.");
        }
    }
}