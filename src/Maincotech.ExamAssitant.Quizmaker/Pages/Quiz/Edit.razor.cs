using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Quiz
{
    public partial class Edit
    {
        [Parameter] public string Id { get; set; }
        [Inject] private DrawerService DrawerService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            IsLoading = true;
            if (Id.IsNotNullOrEmpty())
            {
                ViewModel = new QuizViewModel()
                {
                    Id = Id
                };
            }
            else
            {
                var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                ViewModel = new QuizViewModel()
                {
                };
                //
            }
            IsLoading = true;
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

        private async ValueTask OnFinish(EditContext editContext)
        {
            // Console.WriteLine($"Success:{JsonSerializer.Serialize(model)}");
            //   await ViewModel.Save();
            //   var indexPagePath = Options.AdminAreaName.IsNullOrEmpty() ? "/blogs" : $"/{Options.AdminAreaName}/blogs";
            //   NavigationManager.NavigateTo(indexPagePath, forceLoad: true);
        }

        private void OnFinishFailed(EditContext editContext)
        {
            //   Console.WriteLine($"Failed:{JsonSerializer.Serialize(model)}");
        }

        private async Task OnSave()
        {
            IsLoading = true;
            ViewModel.Save.Execute().Subscribe(
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

        private async Task OnAddSection()
        {
            var options = new DrawerOptions()
            {
                Title = "Edit Section",
                Width = 450,
            };
            var vm = new SectionViewModel();
            var result = await DrawerService.CreateDialogAsync<Components.EditSection, SectionViewModel, bool>(options, vm);
            if (result)
            {
                IsLoading = true;
                ViewModel.UpdateSection.Execute(vm).Subscribe(
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

        private async Task OnDeleteSection(SectionViewModel viewModel)
        {
            IsLoading = true;
            ViewModel.DeleteSection.Execute(viewModel).Subscribe(
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

        private async Task OnEditSection(SectionViewModel viewModel)
        {
            var options = new DrawerOptions()
            {
                Title = "Edit Section",
                Width = 450,
            };
            var vm = new SectionViewModel();
            vm.MergeDataFrom(viewModel);

            var result = await DrawerService.CreateDialogAsync<Components.EditSection, SectionViewModel, bool>(options, vm);
            if (result)
            {
                viewModel.MergeDataFrom(vm);
                IsLoading = true;
                ViewModel.UpdateSection.Execute(viewModel).Subscribe(
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

        private async Task OnAddQuestion(SectionViewModel viewModel)
        {
            var options = new DrawerOptions()
            {
                Title = "Edit Question",
                Width = 680,
            };
            var vm = new QuestionViewModel() { ExamId = ViewModel.Id, SectionId = viewModel.Id };
            var result = await DrawerService.CreateDialogAsync<Components.EditQuestion, QuestionViewModel, bool>(options, vm);

            if (result)
            {
                IsLoading = true;
                ViewModel.UpdateQuestion.Execute(vm).Subscribe(
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

        private void Callback(string[] keys)
        {
            Console.WriteLine(string.Join(',', keys));
        }

        private void OnSectionCollapsed(bool isCollapsed, SectionViewModel vm)
        {
            if (IsLoading)
            {
                return;
            }
            if (isCollapsed)
            {
                if (vm.IsLoaded == false)
                {
                    IsLoading = true;
                    ViewModel.LoadSection.Execute(vm).Subscribe(
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
    }
}