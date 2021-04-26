using AntDesign;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Setting.Components
{
    public partial class DataSettings
    {
        [Inject] private IconService IconServic { get; set; }
        [CascadingParameter] private Index Index { get; set; }
        private List<UserSettingItem> _data = new List<UserSettingItem>();

        protected override async Task OnInitializedAsync()
        {
            await IconServic.CreateFromIconfontCN("//at.alicdn.com/t/font_2504866_x7llrglw92.js");
            _data.Add(new UserSettingItem
            {
                Avater = "iconfirebase",
                Title = "Firebase",
                Description = Index.ViewModel.FirebaseSetting.IsNotNullOrEmpty()
                ? $"Store your data in project: {Index.ViewModel.FirebaseSetting.ProjectId}"
                : "Use firebase to store your data",
                Actions = new RenderFragment[]
               {
                builder => {
                    builder.OpenElement(0,"a");
                    builder.AddAttribute(1, "onclick", EventCallback.Factory.Create(this,Index.OpenFirebaseSettings));
                    builder.AddContent(1,"Modify");
                    builder.CloseElement();
                }
               }
            });
        }
    }
}