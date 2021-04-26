using AntDesign;
using Maincotech.ExamAssitant.Dtos;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Exam
{
    public partial class Index
    {
        [Inject]
        public IndexViewModel IndexViewModel
        {
            get => ViewModel;
            set => ViewModel = value;
        }

        [Inject]
        private IconService IconServic { get; set; }


        private readonly ListGridType _listGridType = new ListGridType
        {
            Gutter = 24,
            Column = 4
        };

        protected override async Task OnInitializedAsync()
        {
            await IconServic.CreateFromIconfontCN("//at.alicdn.com/t/font_2504866_x7llrglw92.js");

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
