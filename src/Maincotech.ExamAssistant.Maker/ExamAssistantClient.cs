using System;
using System.Net.Http;
using System.Text;

namespace Maincotech.ExamAssistant
{
    public partial class ExamAssistantClient
    {
        public string UserId { get; internal set; }

        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
        {
            if (UserId.IsNotNullOrEmpty())
            {
                if(request.Headers.Contains("X-ExamAssistant-UserID"))
                {
                    request.Headers.Remove("X-ExamAssistant-UserID");
                }
                request.Headers.Add("X-ExamAssistant-UserID", UserId);
            }
        }

        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder)
        {
            if (UserId.IsNotNullOrEmpty())
            {
                if (request.Headers.Contains("X-ExamAssistant-UserID"))
                {
                    request.Headers.Remove("X-ExamAssistant-UserID");
                }
                request.Headers.Add("X-ExamAssistant-UserID", UserId);
            }
        }
    }
}