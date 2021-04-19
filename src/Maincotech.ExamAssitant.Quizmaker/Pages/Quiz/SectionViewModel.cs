using ReactiveUI;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Maincotech.Quizmaker.Pages.Quiz
{
    public class SectionViewModel : ReactiveObject
    {
        private string _Name;

        [Required]
        public string Name
        {
            get => _Name;
            set => this.RaiseAndSetIfChanged(ref _Name, value);
        }

        private string _MarkdownContent;

        public string MarkdownContent
        {
            get => _MarkdownContent;
            set => this.RaiseAndSetIfChanged(ref _MarkdownContent, value);
        }

        private string _HtmlContent;

        public string HtmlContent
        {
            get => _HtmlContent;
            set => this.RaiseAndSetIfChanged(ref _HtmlContent, value);
        }

        public ObservableCollection<QuestionViewModel> Questions { get; set; } = new ObservableCollection<QuestionViewModel>();
    }
}