using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Maincotech.ExamAssistant.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly FirebaseAuth _firebaseAuth;

        public AppUserService(string jsonCredentials)
        {
            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromJson(jsonCredentials),
            });

            // Retrieve services by passing the defaultApp variable...
            _firebaseAuth = FirebaseAuth.GetAuth(defaultApp);
        }

        public async Task<AppUserDto> CreateUserAsync(AppUserDto dto)
        {
            var userRecord = await _firebaseAuth.CreateUserAsync(new UserRecordArgs
            {
                Email = dto.Email,
                Password = dto.Password,
            });

            dto.Uid = userRecord.Uid;
            return dto;
        }

        public async Task DisableUser(string uid)
        {
            await _firebaseAuth.UpdateUserAsync(new UserRecordArgs
            {
                Uid = uid,
                Disabled = true
            });
        }

        public async Task<ListDataResult<AppUserDto>> ListUsersAsync(string pageToken)
        {
            var users = new List<AppUserDto>();
            string nextPageToken = null;
            var pagedEnumerable = _firebaseAuth.ListUsersAsync(new ListUsersOptions { PageToken = pageToken });
            var responses = pagedEnumerable.AsRawResponses().GetAsyncEnumerator();

            while (await responses.MoveNextAsync())
            {
                ExportedUserRecords response = responses.Current;
                nextPageToken = response.NextPageToken;
                foreach (ExportedUserRecord user in response.Users)
                {
                    // user.UserMetaData.LastSignInTimestamp
                    users.Add(user.To<AppUserDto>());
                }
            }
            return new ListDataResult<AppUserDto>(nextPageToken, users);
        }

        public async Task ResetPassword(string email)
        {
            await _firebaseAuth.GeneratePasswordResetLinkAsync(email);
        }
    }
}