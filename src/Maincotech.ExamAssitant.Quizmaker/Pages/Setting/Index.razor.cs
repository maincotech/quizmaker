using AntDesign;
using Maincotech.Quizmaker.Pages.Setting.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Setting
{
    public partial class Index
    {
        [Inject] private DrawerService DrawerService { get; set; }

        private readonly Dictionary<string, string> _menuMap = new Dictionary<string, string>
        {
            {"data", "Data Settings"},
            {"user", "User Settings"}
        };

        private List<UserSettingItem> _dataSettingItems = new();
        private List<UserSettingItem> _userSettingItems = new();

        private string _selectKey = "data";

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

        private async Task OnResetPassword()
        {
            NavigationManager.NavigateTo($"/MicrosoftIdentity/Account/ResetPassword", true);
        }

        private async Task OnEditProfile()
        {
            NavigationManager.NavigateTo($"/MicrosoftIdentity/Account/EditProfile", true);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            ViewModel = new IndexViewModel(currentUser.Userid);
            IsLoading = true;

            ViewModel.Load.Execute().Subscribe(
                (unit) =>
                {
                    _dataSettingItems.Add(new UserSettingItem
                    {
                        Avater = "iconfirebase",
                        Title = "Firebase",
                        Description = ViewModel.FirebaseSetting.IsNotNullOrEmpty()
                 ? $"Store your data in project: {ViewModel.FirebaseSetting.ProjectId}"
                 : "Use firebase to store your data",
                        Actions = new RenderFragment[]
                {
                builder => {
                    builder.OpenElement(0,"a");
                    builder.AddAttribute(1, "onclick", EventCallback.Factory.Create(this,OpenFirebaseSettings));
                    builder.AddContent(1,"Modify");
                    builder.CloseElement();
                }
                }
                    });
                },
                (ex) =>
                {
                    Console.WriteLine(ex);
                    IsLoading = false;
                },
                () =>
                {
                    IsLoading = false;
                });

            _userSettingItems.Add(
            new UserSettingItem
            {
                Title = "Account Password",
                Description = "Reset current user's login password",
                Actions = new RenderFragment[]
                {
                builder => {
                    builder.OpenElement(0,"a");
                    builder.AddAttribute(1, "onclick", EventCallback.Factory.Create(this,OnResetPassword));
                    builder.AddContent(1,"Modify");
                    builder.CloseElement();
                }
                }
            });

            _userSettingItems.Add(
           new UserSettingItem
           {
               Title = "User Profile",
               Description = "Modify current user's profile",
               Actions = new RenderFragment[]
               {
                builder => {
                    builder.OpenElement(0,"a");
                    builder.AddAttribute(1, "onclick", EventCallback.Factory.Create(this,OnEditProfile));
                    builder.AddContent(1,"Modify");
                    builder.CloseElement();
                }
               }
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