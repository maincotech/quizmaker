using AntDesign;
using Maincotech.Quizmaker.Models;
using Maincotech.Quizmaker.Pages.Setting.Components;
using Maincotech.Quizmaker.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Setting
{
    public partial class Index
    {
        [Inject] private DrawerService DrawerService { get; set; }
        [Inject] protected IUserService UserService { get; set; }

        private CurrentUser _currentUser = new CurrentUser();

        private readonly Dictionary<string, string> _menuMap = new Dictionary<string, string>
        {
            {"base", "Basic Settings"},
            {"security", "Security Settings"},
            {"binding", "Account Binding"},
            {"notification", "New Message Notification"},
        };

        private string _selectKey = "base";

        private void SelectKey(MenuItem item)
        {
            _selectKey = item.Key;
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

        private void UpdateFirebaseSettings(FirebaseSettingsViewModel viewModel)
        {
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _currentUser = await UserService.GetCurrentUserAsync();
            ViewModel = new IndexViewModel(_currentUser.Userid);
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

        public async Task OpenFirebaseSettings()
        {
            var options = new DrawerOptions()
            {
                Title = "Edit Section",
                Width = 450,
            };
            var firebaseSettingsViewModel = new FirebaseSettingsViewModel();
            if (ViewModel.FirebaseSetting.IsNotNullOrEmpty())
            {
                var vm = ViewModel.FirebaseSetting.To<FirebaseSettingsViewModel>();
                firebaseSettingsViewModel.MergeDataFrom(vm);
            }
            var result = await DrawerService.CreateDialogAsync<Components.EditFirebaseSettings, FirebaseSettingsViewModel, bool>(options, firebaseSettingsViewModel);
            if (result)
            {
                IsLoading = true;
                ViewModel.SaveFirebaseSetting.Execute(firebaseSettingsViewModel).Subscribe(
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