using Maincotech.ExamAssistant.ViewModels;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Pages.Exam
{
    public class IndexViewModel : SettingBasedViewModel
    {
        private static Maincotech.Logging.ILogger _Logger = AppRuntimeContext.Current.GetLogger<IndexViewModel>();
        public ObservableCollection<ExamDto> Items { get; set; } = new ObservableCollection<ExamDto>();

        public IndexViewModel(string userId) : base(userId)
        {
            Load = ReactiveCommand.CreateFromTask(LoadAsync);
            DeleteExam = ReactiveCommand.CreateFromTask<ExamDto>(DeleteExamAsync);
        }

        public ReactiveCommand<Unit, Unit> Load { get; }

        public ReactiveCommand<ExamDto, Unit> DeleteExam { get; }

        private async Task LoadAsync()
        {
            Items.Clear();
            Items.Add(new ExamDto());
            var exams = await client.GetExamsAsync();
            foreach (var exam in exams)
            {
                Items.Add(exam);
            }
        }

        private async Task DeleteExamAsync(ExamDto exam)
        {
            await client.DeleteExamAsync(exam.Id);
            Items.Remove(exam);
        }
    }
}