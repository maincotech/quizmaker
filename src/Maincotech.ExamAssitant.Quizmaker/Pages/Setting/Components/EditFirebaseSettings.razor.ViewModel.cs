using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Setting.Components
{
    public class FirebaseSettingsViewModel : ReactiveObject
    {
        private string _ProjectId;

        [Required]
        public string ProjectId
        {
            get => _ProjectId;
            set => this.RaiseAndSetIfChanged(ref _ProjectId, value);
        }

        private string _JsonCredentials;

        [Required]
        public string JsonCredentials
        {
            get => _JsonCredentials;
            set => this.RaiseAndSetIfChanged(ref _JsonCredentials, value);
        }


    }
}
