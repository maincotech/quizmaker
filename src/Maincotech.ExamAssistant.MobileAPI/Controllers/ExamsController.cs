using Maincotech.ExamAssistant.Dtos;
using Maincotech.ExamAssistant.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.MobileAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExamsController : ControllerBase
    {
        private IExamService _service;

        public ExamsController(IExamService examService)
        {
            _service = examService;
        }

        /// <summary>
        /// Get exams
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetExams")]
        public async Task<IEnumerable<ExamDto>> GetExams()
        {
            return await _service.GetExams();
        }

        /// <summary>
        /// Get all questions under the specified section
        /// </summary>
        /// <param name="examId"></param>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        [HttpGet("{examId}/questions", Name = "GetQuestions")]
        public async Task<IEnumerable<QuestionDto>> GetQuestions(string examId)
        {
            var result = new List<QuestionDto>();
            var query = new LoadMoreQuery
            {
                Limit = 100
            };
            var response = await _service.GetQuestions(examId, query);
            while(response.HasMoreData)
            {
                result.AddRange(response.Items);
                query.Offset = response.NextOffset;
                response = await _service.GetQuestions(examId, query);
            }
            result.AddRange(response.Items);
            return result;
        }
    }
}