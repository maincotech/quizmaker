using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Pages.Setting
{
    public class UserSettingItem
    {
        public string Avater { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public RenderFragment[] Actions { get; set; }
    }
}
