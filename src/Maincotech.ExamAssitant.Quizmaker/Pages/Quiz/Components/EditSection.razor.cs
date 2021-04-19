using AntDesign;
using Maincotech.Web.Components.Vditor;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Quiz.Components
{
    public partial class EditSection
    {
        private Dictionary<string, object> EditorOptions => new Dictionary<string, object>
    {
        {"upload", new UploadOptions{ Url = "/api/files/vditorUpload" } }
    };

        private async ValueTask OnFinish(EditContext editContext)
        {
            await ((DrawerRef<bool>)base.FeedbackRef)?.CloseAsync(true);
           // await this.OkCancelRefWithResult.OkAsync(true);
          //  await this.CloseFeedbackAsync();
           // await this.CloseAsync(true);
        }

        private void OnFinishFailed(EditContext editContext)
        {
            //   Console.WriteLine($"Failed:{JsonSerializer.Serialize(model)}");
        }


      
    }
}