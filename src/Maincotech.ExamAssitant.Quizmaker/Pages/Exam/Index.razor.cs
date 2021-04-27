using AntDesign;
using Maincotech.ExamAssistant.Dtos;
using Maincotech.ExamAssistant.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Exam
{
    public partial class Index
    {
        private readonly ListGridType _listGridType = new ListGridType
        {
            Gutter = 24,
            Column = 4
        };

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

            ViewModel.Load.Execute().Subscribe(
                (unit) => { },
                (ex) =>
                {
                    Console.WriteLine(ex);
                    IsLoading = false;
                    if (ex.InnerException is NotConfiguredException)
                    {
                        NavigationManager.NavigateTo("/setting", true);
                    }
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

        private void OnAddExam()
        {
            NavigationManager.NavigateTo("exams/design");
        }

        private async Task OnDeleteExam(ExamDto viewModel)
        {
            IsLoading = true;
            ViewModel.DeleteExam.Execute(viewModel).Subscribe(
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