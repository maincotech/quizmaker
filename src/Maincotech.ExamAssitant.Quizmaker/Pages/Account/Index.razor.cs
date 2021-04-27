using AntDesign;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Account
{
    public partial class Index
    {
        [Inject] private DrawerService DrawerService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            ViewModel = new IndexViewModel(currentUser.Userid);
            try
            {
                await ViewModel.InitAsync();
            }
            catch (NotConfiguredException)
            {
                NavigationManager.NavigateTo("/setting", true);
            }

            ViewModel.LoadMore.Execute().Subscribe(
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

        private bool _IsLoading = true;

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

        private async Task OnAddAppUser()
        {
            var options = new DrawerOptions()
            {
                Title = "Edit Section",
                Width = 450,
            };
            var vm = new AppUserViewModel();
            var result = await DrawerService.CreateDialogAsync<Components.AddAppUser, AppUserViewModel, bool>(options, vm);
            if (result)
            {
                IsLoading = true;
                ViewModel.AddAppUser.Execute(vm).Subscribe(
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