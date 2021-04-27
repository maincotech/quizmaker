using AntDesign;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Exam
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
    }
}