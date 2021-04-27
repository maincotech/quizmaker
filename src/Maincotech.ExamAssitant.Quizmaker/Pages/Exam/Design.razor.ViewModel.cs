using Maincotech.ExamAssistant.Dtos;
using Maincotech.Quizmaker.ViewModels;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Exam
{
    public class DesignViewModel : SettingBasedViewModel
    {
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

        public DesignViewModel(string userId) : base(userId)
        {
            // _dataAdminService = AppRuntimeContext.Current.Resolve<Maincotech.Cms.Services.IAdminService>();
            Load = ReactiveCommand.CreateFromTask(LoadAsync);
            Save = ReactiveCommand.CreateFromTask(SaveAsync);
            UpdateSection = ReactiveCommand.CreateFromTask<SectionViewModel>(CreateOrUpdateSectionAsync);
            DeleteSection = ReactiveCommand.CreateFromTask<SectionViewModel>(DeleteSectionAsync);
            UpdateQuestion = ReactiveCommand.CreateFromTask<QuestionViewModel>(CreateOrUpdateQuestionAsync);
            DeleteQuestion = ReactiveCommand.CreateFromTask<QuestionViewModel>(DeleteQuestionAsync);
            LoadSection = ReactiveCommand.CreateFromTask<SectionViewModel>(LoadSectioinAsync);
        }

        public ReactiveCommand<Unit, Unit> Load { get; }
        public ReactiveCommand<SectionViewModel, Unit> UpdateSection { get; }
        public ReactiveCommand<SectionViewModel, Unit> DeleteSection { get; }
        public ReactiveCommand<QuestionViewModel, Unit> UpdateQuestion { get; }
        public ReactiveCommand<QuestionViewModel, Unit> DeleteQuestion { get; }
        public ReactiveCommand<SectionViewModel, Unit> LoadSection { get; }

        public ObservableCollection<SectionViewModel> Sections { get; set; } = new ObservableCollection<SectionViewModel>();

        private async Task LoadSectioinAsync(SectionViewModel vm)
        {
            var dtos = await ExamService.GetQuestions(Id, vm.Id);
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
            await ExamService.DeleteQuestion(vm.ExamId, vm.SectionId, vm.Id);
            var section = Sections.First(x => x.Id == vm.SectionId);
            section.Questions.Remove(vm);
        }

        private async Task DeleteSectionAsync(SectionViewModel vm)
        {
            await ExamService.DeleteSection(Id, vm.Id);
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
            dto = await ExamService.CreateOrUpdateSection(dto);
            vm.Id = dto.Id;
            if (Sections.Contains(vm) == false)
            {
                Sections.Add(vm);
            }
        }

        private async Task CreateOrUpdateQuestionAsync(QuestionViewModel vm)
        {
            var dto = vm.To<QuestionDto>();
            dto = await ExamService.CreateOrUpdateQuestion(dto);
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
                examDto = await ExamService.GetExam(Id);
                await LoadSections();
            }
            else
            {
                examDto = new ExamDto
                {
                    Name = "New exam",
                    Duration = 0,
                };
                examDto = await ExamService.CreateOrUpdateExam(examDto);
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
            var sections = await ExamService.GetSections(Id);
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

        public ReactiveCommand<Unit, Unit> Save { get; }

        public async Task SaveAsync()
        {
            var examDto = new ExamDto
            {
                Id = Id,
                Name = Name,
                Duration = Duration,
                Provider = Provider
            };
            examDto = await ExamService.CreateOrUpdateExam(examDto);
        }
    }
}