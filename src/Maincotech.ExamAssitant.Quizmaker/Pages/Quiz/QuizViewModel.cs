using Maincotech.ExamAssitant.Services;
using Maincotech.Quizmaker.Services;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reactive;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Quiz
{
    public class QuizViewModel : ReactiveObject
    {
        private IQuizService _service;
        private IExamService _examService;
        

        private string _Title;

        [Required]
        public string Title
        {
            get => _Title;
            set => this.RaiseAndSetIfChanged(ref _Title, value);
        }

        private string _Provider;

        public string Provider
        {
            get => _Provider;
            set => this.RaiseAndSetIfChanged(ref _Provider, value);
        }

        private string _Id;

        public string Id
        {
            get => _Id;
            set => this.RaiseAndSetIfChanged(ref _Id, value);
        }

        private int _Duration;

        public int Duration
        {
            get => _Duration;
            set => this.RaiseAndSetIfChanged(ref _Duration, value);
        }

        public QuizViewModel()
        {
            _service = AppRuntimeContext.Current.Resolve<IQuizService>();
            _examService = AppRuntimeContext.Current.Resolve<IExamService>();
            // _dataAdminService = AppRuntimeContext.Current.Resolve<Maincotech.Cms.Services.IAdminService>();
            Load = ReactiveCommand.CreateFromTask(LoadAll);
        }

        public ReactiveCommand<Unit, Unit> Load { get; }

        public ObservableCollection<SectionViewModel> Sections { get; set; }
        public ObservableCollection<QuestionViewModel> Questions { get; set; }

        public async Task LoadAll()
        {
            if(!string.IsNullOrEmpty(Id))
            {
                var exam = _examService.GetExam("");
            }
            Sections = new ObservableCollection<SectionViewModel>();
            Questions = new ObservableCollection<QuestionViewModel>();
        }

        public int GetIndex(QuestionViewModel viewModel)
        {
            var index = 0;
            for (; index < Questions.Count; index++)
            {
                if (Questions[index].Id.Equals(viewModel.Id))
                {
                    index++;
                    break;
                }
            }
            return index;
        }
    }
}