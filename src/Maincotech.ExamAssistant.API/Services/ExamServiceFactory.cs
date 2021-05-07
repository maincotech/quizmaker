namespace Maincotech.ExamAssistant.Services
{
    public static class ExamServiceFactory
    {
        public static IExamService GetExamService(string userId)
        {
            var settingService = AppRuntimeContext.Current.Resolve<ISettingService>();
            var firestoreSettings = settingService.GetFirebaseSetting(userId).GetAwaiter().GetResult();
            return new ExamService(firestoreSettings.ProjectId, firestoreSettings.JsonCredentials);
        }
    }
}