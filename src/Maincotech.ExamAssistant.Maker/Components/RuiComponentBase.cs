using Maincotech.ExamAssistant.Maker.Models;
using Maincotech.ExamAssistant.Maker.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ReactiveUI;
using ReactiveUI.Blazor;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Components
{
    public partial class RuiComponentBase<TViewModel> : ReactiveComponentBase<TViewModel> where TViewModel : ReactiveObject
    {
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] protected IUserService UserService { get; set; }
        protected CurrentUser currentUser;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            currentUser = await UserService.GetCurrentUserAsync();
        }
    }
}