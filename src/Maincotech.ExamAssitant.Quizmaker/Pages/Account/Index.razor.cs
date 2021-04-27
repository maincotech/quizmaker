﻿using System;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Account
{
    public partial class Index
    {
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
        }
    }
}