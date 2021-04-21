using Maincotech.Quizmaker.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Quiz
{
    public class QuestionViewModel : ReactiveObject
    {
        public string ExamId { get; set; }

        public string SectionId { get; set; }

        public string Id { get; set; }

        private string _Title;

        [Required]
        public string Title
        {
            get => _Title;
            set => this.RaiseAndSetIfChanged(ref _Title, value);
        }

        private string _Analysis;
        public string Analysis
        {
            get => _Analysis;
            set => this.RaiseAndSetIfChanged(ref _Analysis, value);
        }

        private QuestionTypes _QuestionTypes;
        public QuestionTypes QuestionType
        {
            get => _QuestionTypes;
            set => this.RaiseAndSetIfChanged(ref _QuestionTypes, value);
        }

        public ObservableCollection<QuestionOptionViewModel> Options { get; } = new ObservableCollection<QuestionOptionViewModel>();
    }

    public class QuestionOptionViewModel : ReactiveObject
    {
        public string Name { get; set; }
        public bool IsCorrect { get; set; }
        public string AnswerText { get; set; }
    }
}
