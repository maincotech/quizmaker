using Maincotech.ExamAssistant.Dtos;
using Maincotech.ExamAssistant.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.API.Controllers
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
        /// Get the specified exam information
        /// </summary>
        /// <param name="id">the exam's id</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetExam")]
        public async Task<ExamDto> GetExam(string id)
        {
            return await _service.GetExam(id);
        }

        /// <summary>
        /// Create or update a section
        /// </summary>
        /// <param name="examId">Exam id</param>
        /// <param name="dto">Section information</param>
        /// <returns></returns>
        [HttpPost("{examId}/sections", Name = "CreateOrUpdateSection")]
        public async Task<SectionDto> CreateOrUpdateSection(string examId, SectionDto dto)
        {
            return await _service.CreateOrUpdateSection(dto);
        }

        /// <summary>
        /// Create or update a questions
        /// </summary>
        /// <param name="examId">Exam id</param>
        /// <param name="sectionId">Section id</param>
        /// <param name="dto">Question information</param>
        /// <returns></returns>
        [HttpPost("{examId}/sections/{sectionId}/questions", Name = "CreateOrUpdateQuestion")]
        public async Task<QuestionDto> CreateOrUpdateQuestion(string examId, string sectionId, QuestionDto dto)
        {
            return await _service.CreateOrUpdateQuestion(dto);
        }

        /// <summary>
        /// Get sections in the specified exam
        /// </summary>
        /// <param name="id">The exam id</param>
        /// <returns></returns>
        [HttpGet("{examId}/sections", Name = "GetSections")]
        [ProducesResponseType(typeof(IEnumerable<SectionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<SectionDto>> GetSections(string examId)
        {
            return await _service.GetSections(examId);
        }

        /// <summary>
        /// Create a new exam
        /// </summary>
        /// <param name="exam">The exam information</param>
        /// <returns>return the new created exam with id</returns>
        [HttpPost(Name = "CreateOrUpdateExam")]
        [ProducesResponseType(typeof(ExamDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IDictionary<string, string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ExamDto> CreateOrUpdateExam(ExamDto exam)
        {
            return await _service.CreateOrUpdateExam(exam);
        }

        /// <summary>
        /// Get all questions under the specified section
        /// </summary>
        /// <param name="examId"></param>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        [HttpGet("{examId}/sections/{sectionId}/questions", Name = "GetQuestionsInSection")]
        public async Task<IEnumerable<QuestionDto>> GetQuestions(string examId, string sectionId)
        {
            return await _service.GetQuestions(examId, sectionId);
        }

        /// <summary>
        /// Get paged questions in the specified the exam
        /// </summary>
        /// <param name="examId">Exam Id</param>
        /// <param name="query">paging query</param>
        /// <returns></returns>
        [HttpGet("{examId}/questions", Name = "GetQuestions")]
        public async Task<LoadMoreResult<QuestionDto>> GetQuestions(string examId, LoadMoreQuery query)
        {
            return await _service.GetQuestions(examId, query);
        }

        /// <summary>
        /// delete the specified exam
        /// </summary>
        /// <param name="id">The id of the exam</param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteExam")]
        public async Task DeleteExam(string id)
        {
            await _service.DeleteExam(id);
        }

        /// <summary>
        /// Delete the specified section
        /// </summary>
        /// <param name="examId">Exam id</param>
        /// <param name="sectionId">Section id</param>
        /// <returns></returns>
        [HttpDelete("{examId}/sections/{sectionId}", Name = "DeleteSection")]
        public async Task DeleteSection(string examId, string sectionId)
        {
            await _service.DeleteSection(examId, sectionId);
        }

        /// <summary>
        /// Delete the specified question.
        /// </summary>
        /// <param name="examId">Exam id</param>
        /// <param name="sectionId">Section id</param>
        /// <param name="questionId">Question id</param>
        /// <returns></returns>
        [HttpDelete("{examId}/sections/{sectionId}/questions/{questionId}", Name = "DeleteQuestion")]
        public async Task DeleteQuestion(string examId, string sectionId, string questionId)
        {
            await _service.DeleteQuestion(examId, sectionId, questionId);
        }
    }
}