using AntDesign;
using Maincotech.ExamAssitant.Services;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Pages.Setting.Components
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
                var examService = new ExamService(Options.ProjectId, Options.JsonCredentials);
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