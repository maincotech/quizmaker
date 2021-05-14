using AntDesign;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Pages.Account.Components
{
    public partial class AddAppUser
    {
        private bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        private async ValueTask OnFinish(EditContext editContext)
        {
            var messageStore = new ValidationMessageStore(editContext);
            if(!IsValidEmail(Options.Email))
            {
                messageStore.Add(editContext.Field("Email"), "The email address is not valid.");
                editContext.NotifyValidationStateChanged();
                return;
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");
           if(! hasNumber.IsMatch(Options.Password))
            {
                messageStore.Add(editContext.Field("Password"), "The password must contain at least one number.");
                editContext.NotifyValidationStateChanged();
                return;
            }
            if (!hasUpperChar.IsMatch(Options.Password))
            {
                messageStore.Add(editContext.Field("Password"), "The password must contain at least one upper-case letter.");
                editContext.NotifyValidationStateChanged();
                return;
            }
            if (!hasMinimum8Chars.IsMatch(Options.Password))
            {
                messageStore.Add(editContext.Field("Password"), "The password must contain at least 8 characters.");
                editContext.NotifyValidationStateChanged();
                return;
            }
            

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
