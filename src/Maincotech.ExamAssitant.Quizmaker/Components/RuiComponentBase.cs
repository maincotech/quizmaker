using Maincotech.Quizmaker.Models;
using Maincotech.Quizmaker.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ReactiveUI;
using ReactiveUI.Blazor;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Components
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