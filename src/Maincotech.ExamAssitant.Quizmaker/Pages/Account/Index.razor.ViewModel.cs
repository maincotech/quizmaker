using Maincotech.ExamAssistant.Dtos;
using Maincotech.ExamAssistant.Services;
using Maincotech.Quizmaker.ViewModels;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Account
{
    public class IndexViewModel : SettingBasedViewModel
    {
        private static Maincotech.Logging.ILogger _Logger = AppRuntimeContext.Current.GetLogger<IndexViewModel>();

        public string SearchKeyword { get; set; }
        public string NextPageToken { get; set; }

        private bool _HasMoreData;

        public bool HasMoreData
        {
            get => _HasMoreData;
            set => this.RaiseAndSetIfChanged(ref _HasMoreData, value);
        }

        public ObservableCollection<AppUserDto> Items { get; set; } = new ObservableCollection<AppUserDto>();
        public ReactiveCommand<Unit, Unit> LoadMore { get; }
        private IAppUserService _appUserService;

        public IndexViewModel(string userId) : base(userId)
        {
            LoadMore = ReactiveCommand.CreateFromTask(LoadMoreAsync);
        }

        private async Task LoadMoreAsync()
        {
            if (_appUserService == null)
            {
                _appUserService = new AppUserService(firesbaseSettings.JsonCredentials);
            }

            var queryResult = await _appUserService.ListUsersAsync(NextPageToken);
            HasMoreData = queryResult.HasMoreData;
            NextPageToken = queryResult.NextPageToken;
            foreach (var dto in queryResult.Items)
            {
                Items.Add(dto.To<AppUserDto>());
            }
        }
    }
}