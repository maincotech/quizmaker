using ReactiveUI;
using System.ComponentModel.DataAnnotations;

namespace Maincotech.Quizmaker.Pages.Account
{
    public class AppUserViewModel : ReactiveObject
    {
        private string _Email;

        [Required]
        public string Email
        {
            get => _Email;
            set => this.RaiseAndSetIfChanged(ref _Email, value);
        }

        private string _Password;

        [Required]
        public string Password
        {
            get => _Password;
            set => this.RaiseAndSetIfChanged(ref _Password, value);
        }
    }
}