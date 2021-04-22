using Maincotech.ExamAssitant.Dtos;
using Maincotech.ExamAssitant.Services;
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

        private readonly IExamService _examService;

        public IndexViewModel()
        {
            _examService = AppRuntimeContext.Current.Resolve<IExamService>();
            Load = ReactiveCommand.CreateFromTask(LoadAsync);
            DeleteExam = ReactiveCommand.CreateFromTask<ExamDto>(DeleteExamAsync);
        }

        public ReactiveCommand<Unit, Unit> Load { get; }

        public ReactiveCommand<ExamDto, Unit> DeleteExam { get; }

        private async Task LoadAsync()
        {
            Items.Clear();
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