using Maincotech.Quizmaker.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Services
{
    public interface IUserService
    {
        Task<CurrentUser> GetCurrentUserAsync();
    }

    public class UserService : IUserService
    {
        //  private readonly HttpClient _httpClient;
        private AuthenticationStateProvider _authenticationStateProvider;

        private CurrentUser _currentUser;

        public UserService(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _authenticationStateProvider.AuthenticationStateChanged += (task) => OnAuthenticationStateChanged(task).GetAwaiter().GetResult();
        }

        public async Task<CurrentUser> GetCurrentUserAsync()
        {
            if (_currentUser == null)
            {
                var task = _authenticationStateProvider.GetAuthenticationStateAsync();
                await OnAuthenticationStateChanged(task);
            }
            return _currentUser;
        }

        private async Task OnAuthenticationStateChanged(Task<AuthenticationState> task)
        {
            var state = await task;
            if (state.User.Identity.IsAuthenticated)
            {
                var groups = state.User.FindAll("groups");

                _currentUser = new CurrentUser
                {
                    Name = state.User.FindFirst("name").Value,
                    // Groups = string.Join(";", groups.Select(x => x.Value)),
                    Userid = state.User.FindFirst(ClaimTypes.NameIdentifier).Value
                };
            }
        }
    }
}