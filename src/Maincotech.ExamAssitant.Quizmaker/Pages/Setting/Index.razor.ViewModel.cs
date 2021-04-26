using Maincotech.ExamAssitant.Dtos;
using Maincotech.ExamAssitant.Services;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Setting
{
    public class IndexViewModel : ReactiveObject
    {
        private readonly ISettingService _service;
        public string UserId { get; }

        public FirebaseSettingDto FirebaseSetting { get; private set; }

        public IndexViewModel(string userId)
        {
            UserId = userId;
            _service = AppRuntimeContext.Current.Resolve<ISettingService>();
            Load = ReactiveCommand.CreateFromTask(LoadAsync);
            SaveFirebaseSetting = ReactiveCommand.CreateFromTask<Components.FirebaseSettingsViewModel>(SaveFirebaseSettingAsync);
        }

        public ReactiveCommand<Unit, Unit> Load { get; }
        public ReactiveCommand<Components.FirebaseSettingsViewModel, Unit> SaveFirebaseSetting { get; }

        private async Task LoadAsync()
        {
            FirebaseSetting = await _service.GetFirebaseSetting(UserId);
        }

        private async Task SaveFirebaseSettingAsync(Components.FirebaseSettingsViewModel viewModel)
        {
            var dto = viewModel.To<FirebaseSettingDto>();
            dto.UserId = UserId;
            FirebaseSetting = await _service.CreateOrFirebaseSetting(dto);
        }
    }
}