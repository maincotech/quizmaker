using AntDesign;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Pages.Setting.Components
{
    public partial class DataSettings
    {
        [CascadingParameter] 
        private List<UserSettingItem> Data { get; set; }

    }
}