using AntDesign;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Pages.Exam
{
    public partial class Edit
    {
        [Parameter]
        public string Id { get; set; }

        [Inject] private DrawerService DrawerService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            IsLoading = true;

            ViewModel = new EditViewModel(currentUser.Userid)
            {
                Id = Id
            };

            try
            {
                await ViewModel.InitAsync();
            }
            catch (NotConfiguredException)
            {
                NavigationManager.NavigateTo("/setting", true);
            }
            ViewModel.Load.Execute().Subscribe(
                (unit) => { },
                (ex) =>
                {
                    Console.WriteLine(ex);
                    IsLoading = false;
                },
                () =>
                {
                    IsLoading = false;
                    Search();
                });
        }

        private bool _IsLoading;

        public bool IsLoading
        {
            get => _IsLoading;
            set
            {
                if (_IsLoading != value)
                {
                    _IsLoading = value;
                    InvokeAsync(StateHasChanged);
                }
            }
        }

        private void Search()
        {
            IsLoading = true;
            ViewModel.Search.Execute().Subscribe(items => { },
                ex =>
                {
                    Console.WriteLine(ex);
                    IsLoading = false;
                },
                () =>
                {
                    IsLoading = false;
                });
        }

        private void OnLoadMore()
        {
            IsLoading = true;
            ViewModel.LoadMore.Execute().Subscribe(items => { },
                ex =>
                {
                    Console.WriteLine(ex);
                    IsLoading = false;
                },
                () =>
                {
                    IsLoading = false;
                });
        }

        private async Task OnEditQuestion(QuestionViewModel viewModel)
        {
            var options = new DrawerOptions()
            {
                Title = "Edit Question",
                Width = 680,
            };
            var vm = new QuestionViewModel();
            vm.MergeDataFrom(viewModel);
            //merege options
            foreach (var item in viewModel.Options)
            {
                var option = new QuestionOptionViewModel();
                option.MergeDataFrom(item);
                vm.Options.Add(option);
            }

            var result = await DrawerService.CreateDialogAsync<Components.EditQuestion, QuestionViewModel, bool>(options, vm);

            if (result)
            {
                viewModel.MergeDataFrom(vm);
                viewModel.Options.Clear();
                foreach (var item in vm.Options)
                {
                    var option = new QuestionOptionViewModel();
                    option.MergeDataFrom(item);
                    viewModel.Options.Add(option);
                }

                IsLoading = true;
                ViewModel.UpdateQuestion.Execute(viewModel).Subscribe(
                (unit) => { },
                (ex) =>
                {
                    Console.WriteLine(ex);
                    IsLoading = false;
                },
                () =>
                {
                    IsLoading = false;
                });
            }
        }

        private async Task OnRemoveQuestion(QuestionViewModel viewModel)
        {
            IsLoading = true;
            ViewModel.DeleteQuestion.Execute(viewModel).Subscribe(
            (unit) => { },
            (ex) =>
            {
                Console.WriteLine(ex);
                IsLoading = false;
            },
            () =>
            {
                IsLoading = false;
            });
        }
    }
}