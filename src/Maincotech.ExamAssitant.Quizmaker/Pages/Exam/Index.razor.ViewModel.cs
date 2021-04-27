using Maincotech.ExamAssistant.Dtos;
using Maincotech.ExamAssistant.Services;
using Maincotech.Utilities;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Exam
{
    public class IndexViewModel : ReactiveObject
    {
        private static Maincotech.Logging.ILogger _Logger = AppRuntimeContext.Current.GetLogger<IndexViewModel>();
        public ObservableCollection<ExamDto> Items { get; set; } = new ObservableCollection<ExamDto>();
        public string UserId { get; }

        private IExamService _examService;
        private readonly ISettingService _settingService;

        public IndexViewModel(string userId)
        {
            UserId = userId;
            _settingService = AppRuntimeContext.Current.Resolve<ISettingService>();
            Load = ReactiveCommand.CreateFromTask(LoadAsync);
            DeleteExam = ReactiveCommand.CreateFromTask<ExamDto>(DeleteExamAsync);
        }

        public ReactiveCommand<Unit, Unit> Load { get; }

        public ReactiveCommand<ExamDto, Unit> DeleteExam { get; }

        private async Task LoadAsync()
        {
            //load setting
            var firebaseSetting = await _settingService.GetFirebaseSetting(UserId);
            ParameterChecker.Against<NotConfiguredException>(firebaseSetting == null, "The firebase settings have not been configured.");
            _examService = new ExamService(firebaseSetting.ProjectId, firebaseSetting.JsonCredentials);
            Items.Clear();
            Items.Add(new ExamDto());
            var exams = await _examService.GetExams();
            foreach (var exam in exams)
            {
                Items.Add(exam);
            }
        }

        private async Task DeleteExamAsync(ExamDto exam)
        {
            await _examService.DeleteExam(exam.Id);
            Items.Remove(exam);
        }
    }
}