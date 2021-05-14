using Maincotech.Utilities;
using ReactiveUI;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.ViewModels
{
    public abstract class SettingBasedViewModel : ReactiveObject
    {
        public string UserId { get; }
        protected readonly ExamAssistantClient client;

        public SettingBasedViewModel(string userId)
        {
            UserId = userId;
            client = AppRuntimeContext.Current.Resolve<ExamAssistantClient>();
            client.UserId = userId;
        }

        public async Task InitAsync()
        {
           var firebaseSetting = await client.GetFirebaseSettingAsync(UserId);
            ParameterChecker.Against<NotConfiguredException>(firebaseSetting == null,"The firebase setting has not been configured.");
        }
    }
}