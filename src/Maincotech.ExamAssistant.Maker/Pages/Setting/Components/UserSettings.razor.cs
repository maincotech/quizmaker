using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Pages.Setting.Components
{
    public partial class UserSettings
    {
        [CascadingParameter]
        private List<UserSettingItem> Data { get; set; }
    }
}
