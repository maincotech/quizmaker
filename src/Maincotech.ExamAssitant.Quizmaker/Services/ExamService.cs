using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Google.Cloud.Firestore;
using Maincotech.ExamAssitant.Dtos;
using Maincotech.ExamAssitant.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Services
{
    public class ExamService : IExamService
    {
        private FirestoreDb db;

        public ExamService(IOptions<AzSettings> azSettings, IOptions<FirestoreSettings> firestoreSettings)
        {
            Init(azSettings.Value, firestoreSettings.Value);
        }

        private void Init(AzSettings azSettings, FirestoreSettings firestoreSettings)
        {
            var keyVaultClient = new SecretClient(new Uri(azSettings.VaultURI),
                new ClientSecretCredential(azSettings.TenantId, azSettings.ClientId, azSettings.ClientSecret));
            var gAuthJson = keyVaultClient.GetSecret(firestoreSettings.ProfileName).Value;

            var dbBuilder = new FirestoreDbBuilder
            {
                ProjectId = firestoreSettings.ProjectId,
                JsonCredentials = gAuthJson.Value
            };
            db = dbBuilder.Build();
        }

        public Task CreateOrUpdateExam(ExamDto dto)
        {
            throw new NotImplementedException();
        }

        public Task CreateOrUpdateSection(SectionDto dto)
        {
            throw new NotImplementedException();
        }

        public Task CreateOrUpdateSection(QuestionDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteExam(string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteQuestion(string examId, string sectionId, string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSection(string examId, string sectionId)
        {
            throw new NotImplementedException();
        }

        public Task<ExamDto> GetExam(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<QuestionDto>> GetQuestions(string examId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<QuestionDto>> GetQuestions(string examId, string sectionId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SectionDto>> GetSections(string examId)
        {
            throw new NotImplementedException();
        }
    }
}