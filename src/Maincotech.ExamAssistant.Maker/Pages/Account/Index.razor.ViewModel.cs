using Maincotech.ExamAssistant.ViewModels;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Pages.Account
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
        public ReactiveCommand<AppUserViewModel, Unit> AddAppUser { get; }
        private ExamAssistantClient _client;

        public IndexViewModel(string userId) : base(userId)
        {
            _client = AppRuntimeContext.Current.Resolve<ExamAssistantClient>();
            _client.UserId = userId;
            LoadMore = ReactiveCommand.CreateFromTask(LoadMoreAsync);
            AddAppUser = ReactiveCommand.CreateFromTask<AppUserViewModel>(AddUserAsync);
        }

        private async Task AddUserAsync(AppUserViewModel viewModel)
        {
            var dto = await _client.CreateUserAsync(new AppUserDto
            {
                Email = viewModel.Email,
                Password = viewModel.Password
            });

            Items.Add(dto);
        }

        private async Task LoadMoreAsync()
        {
            var queryResult = await _client.ListUsersAsync(NextPageToken);
            HasMoreData = queryResult.HasMoreData;
            NextPageToken = queryResult.NextPageToken;
            foreach (var dto in queryResult.Items)
            {
                Items.Add(dto);
            }
        }
    }
}