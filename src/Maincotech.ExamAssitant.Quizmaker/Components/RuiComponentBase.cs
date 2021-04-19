using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ReactiveUI;
using ReactiveUI.Blazor;

namespace Maincotech.Quizmaker.Components
{
    public partial class RuiComponentBase<TViewModel> : ReactiveComponentBase<TViewModel> where TViewModel : ReactiveObject
    {
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    }
}