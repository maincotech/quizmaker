using AntDesign;
using Maincotech.Web.Components.Vditor;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Quiz.Components
{
    public partial class EditQuestion
    {
        private Dictionary<string, object> EditorOptions => new Dictionary<string, object>
    {
        {"upload", new UploadOptions{ Url = "/api/files/vditorUpload" } }
    };

        private async ValueTask OnFinish(EditContext editContext)
        {
            //1. Options must have least one
            if(Options.Options.Count == 0)
            {
                var messageStore = new ValidationMessageStore(editContext);
                messageStore.Add(editContext.Field("Options"), "There must be at least one option.");
                editContext.NotifyValidationStateChanged();
                return;
            }

            //TODO: Check
            if(Options.QuestionType == Models.QuestionTypes.DragDrop)
            {
                //need check every answer text
                
            }
            await ((DrawerRef<bool>)base.FeedbackRef)?.CloseAsync(true);
           // await this.OkCancelRefWithResult.OkAsync(true);
           // await this.CloseFeedbackAsync();
           // await this.CloseAsync(true);
        }

        private void OnFinishFailed(EditContext editContext)
        {
            //   Console.WriteLine($"Failed:{JsonSerializer.Serialize(model)}");
        }

        private void OnAddOption()
        {
            var option = new QuestionOptionViewModel();
            Options.Options.Add(option);
        }

        private void OnDeleteOption(QuestionOptionViewModel option)
        {
            Options.Options.Remove(option);
        }
    }
}