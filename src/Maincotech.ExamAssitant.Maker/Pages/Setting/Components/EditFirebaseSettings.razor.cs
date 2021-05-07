using AntDesign;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Pages.Setting.Components
{
    public partial class EditFirebaseSettings
    {
        private static readonly Logging.ILogger Logger = AppRuntimeContext.Current.GetLogger<EditFirebaseSettings>();

        private async ValueTask OnFinish(EditContext editContext)
        {
            var messageStore = new ValidationMessageStore(editContext);
            //verify credential
            try
            {
                var client = AppRuntimeContext.Current.Resolve<ExamAssistantClient>();
                var isValid = await  client.ValidateFirebaseSettingAsync(Options.To<FirebaseSettingDto>());
                if(!isValid)
                {
                     messageStore.Add(editContext.Field("JsonCredentials"), $"Failed to validate the provided credential. Please check again.");
                editContext.NotifyValidationStateChanged();
                return;
                }
                //var examService = new ExamService(Options.ProjectId, Options.JsonCredentials);
            }
            catch (System.Exception ex)
            {
                Logger.Error("Failed to valid the JsonCredentials provided", ex);
                messageStore.Add(editContext.Field("JsonCredentials"), $"Failed to validate the provided credential. Please check again.");
                editContext.NotifyValidationStateChanged();
                return;
            }
            await ((DrawerRef<bool>)base.FeedbackRef)?.CloseAsync(true);
            // await this.OkCancelRefWithResult.OkAsync(true);
            // await this.CloseFeedbackAsync();
            // await this.CloseAsync(true);
        }
    }
}