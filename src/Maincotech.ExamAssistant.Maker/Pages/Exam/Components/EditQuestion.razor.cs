﻿using AntDesign;
using Maincotech.Web.Components.Vditor;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Pages.Exam.Components
{
    public partial class EditQuestion
    {
        private Dictionary<string, object> EditorOptions => new()
        {
            { "upload", new UploadOptions { Url = "/api/files/vditorUpload" } }
        };

        private AntDesign.Form<QuestionViewModel> form;
        private bool _hasError;

        private void ResetValidation()
        {
            if (_hasError)
            {
                form.ValidationReset();
                _hasError = false;
            }
        }

        private async ValueTask OnFinish(EditContext editContext)
        {
            var messageStore = new ValidationMessageStore(editContext);
            //1. Options must have least one
            if (Options.Options.Count == 0)
            {
                messageStore.Add(editContext.Field("Options"), "There must be at least one option.");
                _hasError = true;
                editContext.NotifyValidationStateChanged();
                return;
            }

            switch (Options.QuestionType)
            {
                case ViewModels.QuestionTypes.SingleChoice:
                    var answerCount = Options.Options.Count(x => x.IsCorrect);
                    if (answerCount != 1)
                    {
                        messageStore.Add(editContext.Field("Options"), "There must be one correct answer for single choice question.");
                        editContext.NotifyValidationStateChanged();
                        _hasError = true;
                        return;
                    }
                    break;

                case ViewModels.QuestionTypes.MultipleChoice:
                    if (Options.Options.Count(x => x.IsCorrect) <= 1)
                    {
                        messageStore.Add(editContext.Field("Options"), "There must be at least two correct answers for multiple choice question.");
                        editContext.NotifyValidationStateChanged();
                        _hasError = true;
                        return;
                    }
                    break;

                case ViewModels.QuestionTypes.DragDrop:
                    if (Options.Options.Any(x => string.IsNullOrEmpty(x.AnswerText)))
                    {
                        messageStore.Add(editContext.Field("Options"), "Please specify all the answer text for each option.");
                        _hasError = true;
                        editContext.NotifyValidationStateChanged();
                        return;
                    }
                    break;
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