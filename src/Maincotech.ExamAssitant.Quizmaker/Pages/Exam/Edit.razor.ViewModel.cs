using Maincotech.Data;
using Maincotech.ExamAssitant.Dtos;
using Maincotech.ExamAssitant.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Exam
{
    public class EditViewModel : ReactiveObject
    {
        private static Maincotech.Logging.ILogger _Logger = AppRuntimeContext.Current.GetLogger<EditViewModel>();

        private readonly IExamService _examService;

        private string _Name;

        [Required]
        public string Name
        {
            get => _Name;
            set => this.RaiseAndSetIfChanged(ref _Name, value);
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

        public string SearchText { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int Total { get; set; }

        private readonly ObservableAsPropertyHelper<IEnumerable<QuestionViewModel>> _items;
        public IEnumerable<QuestionViewModel> Items => _items.Value;

        private readonly ObservableAsPropertyHelper<bool> _isLoading;
        public bool IsLoading => _isLoading.Value;

        public EditViewModel()
        {
            _examService = AppRuntimeContext.Current.Resolve<IExamService>();
            Search = ReactiveCommand.CreateFromTask(SearchAsync);
            Load = ReactiveCommand.CreateFromTask(LoadAsync);
            _items = Search.ToProperty(this, x => x.Items, scheduler: RxApp.MainThreadScheduler);
            Search.ThrownExceptions.Subscribe(exception =>
            {
                _Logger.Error("Unexpected error occurred.", exception);
            });

            this.WhenAnyObservable(x => x.Search.IsExecuting).ToProperty(this, x => x.IsLoading, out _isLoading);

            UpdateSection = ReactiveCommand.CreateFromTask<SectionViewModel>(CreateOrUpdateSectionAsync);
            DeleteSection = ReactiveCommand.CreateFromTask<SectionViewModel>(DeleteSectionAsync);
            UpdateQuestion = ReactiveCommand.CreateFromTask<QuestionViewModel>(CreateOrUpdateQuestionAsync);
            DeleteQuestion = ReactiveCommand.CreateFromTask<QuestionViewModel>(DeleteQuestionAsync);
            LoadSection = ReactiveCommand.CreateFromTask<SectionViewModel>(LoadSectioinAsync);
        }

        private async Task<IEnumerable<QuestionViewModel>> SearchAsync()
        {
            var result = new List<QuestionViewModel>();

            var pagination = new Pagination
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
            };
            var entities = await _examService.GetQuestions(pagination, Id, SearchText);
            Total = entities.TotalRecords;
            if (entities.Count > 0)
            {
                var viewModels = AppRuntimeContext.Current.Adapt<List<QuestionViewModel>>(entities);
                result.AddRange(viewModels);
            }
            //  await Task.Delay(1000 * 3);
            return result;
        }

        // public ReactiveCommand<Unit, Unit> Load { get; }
        public ReactiveCommand<Unit, IEnumerable<QuestionViewModel>> Search { get; }

        public ReactiveCommand<Unit, Unit> Load { get; }

        public ReactiveCommand<SectionViewModel, Unit> UpdateSection { get; }
        public ReactiveCommand<SectionViewModel, Unit> DeleteSection { get; }
        public ReactiveCommand<QuestionViewModel, Unit> UpdateQuestion { get; }
        public ReactiveCommand<QuestionViewModel, Unit> DeleteQuestion { get; }
        public ReactiveCommand<SectionViewModel, Unit> LoadSection { get; }

        public ObservableCollection<SectionViewModel> Sections { get; set; } = new ObservableCollection<SectionViewModel>();

        private async Task LoadSectioinAsync(SectionViewModel vm)
        {
            var dtos = await _examService.GetQuestions(Id, vm.Id);
            foreach (var dto in dtos)
            {
                var questionVM = dto.To<QuestionViewModel>();
                vm.Questions.Add(questionVM);
            }
            vm.IsLoaded = true;
            //  var section = Sections.First(x => x.Id == vm.SectionId);
            // section.Questions.Remove(vm);
        }

        private async Task DeleteQuestionAsync(QuestionViewModel vm)
        {
            await _examService.DeleteQuestion(vm.ExamId, vm.SectionId, vm.Id);
            var section = Sections.First(x => x.Id == vm.SectionId);
            section.Questions.Remove(vm);
        }

        private async Task DeleteSectionAsync(SectionViewModel vm)
        {
            await _examService.DeleteSection(Id, vm.Id);
            Sections.Remove(vm);
        }

        private async Task CreateOrUpdateSectionAsync(SectionViewModel vm)
        {
            var dto = new SectionDto
            {
                ExamId = Id,
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.MarkdownContent
            };
            dto = await _examService.CreateOrUpdateSection(dto);
            vm.Id = dto.Id;
            if (Sections.Contains(vm) == false)
            {
                Sections.Add(vm);
            }
        }

        private async Task CreateOrUpdateQuestionAsync(QuestionViewModel vm)
        {
            var dto = vm.To<QuestionDto>();
            dto = await _examService.CreateOrUpdateQuestion(dto);
            vm.Id = dto.Id;
            var sectionVM = Sections.First(x => x.Id == vm.SectionId);
            if (sectionVM.Questions.Contains(vm) == false)
            {
                sectionVM.Questions.Add(vm);
            }
        }

        private async Task LoadAsync()
        {
            ExamDto examDto;
            if (Id.IsNotNullOrEmpty())
            {
                examDto = await _examService.GetExam(Id);
                await LoadSections();
            }
            else
            {
                examDto = new ExamDto
                {
                    Name = "New exam",
                    Duration = 0,
                };
                examDto = await _examService.CreateOrUpdateExam(examDto);
            }
            Id = examDto.Id;
            Name = examDto.Name;
            Provider = examDto.Provider;
            Duration = examDto.Duration;

            //Sections = new ObservableCollection<SectionViewModel>();
            //Questions = new ObservableCollection<QuestionViewModel>();
        }

        public async Task LoadSections()
        {
            var sections = await _examService.GetSections(Id);
            foreach (var section in sections)
            {
                Sections.Add(new SectionViewModel
                {
                    Id = section.Id,
                    Name = section.Name,
                    MarkdownContent = section.Description,
                    NumberOfQuestions = section.NumberOfQuestions
                });
            }
        }

        public int GetIndex(QuestionViewModel viewModel)
        {
            var index = 1;
            SectionViewModel sectionViewModel = null;
            foreach (var section in Sections)
            {
                if (section.Id == viewModel.SectionId)
                {
                    sectionViewModel = section;
                    break;
                }
                index += section.NumberOfQuestions;
            }
            foreach (var question in sectionViewModel.Questions)
            {
                if (question.Id == viewModel.Id)
                {
                    break;
                }
                index++;
            }
            return index;
        }
    }
}