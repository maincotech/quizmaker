using Maincotech.ExamAssitant.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maincotech.ExamAssitant.Services
{
    public interface IExamService
    {
        Task<ExamDto> CreateOrUpdateExam(ExamDto dto);

        Task<SectionDto> CreateOrUpdateSection(SectionDto dto);

        Task<QuestionDto> CreateOrUpdateQuestion(QuestionDto dto);

        Task<ExamDto> GetExam(string id);

        Task<IEnumerable<ExamDto>> GetExams();

        Task<IEnumerable<SectionDto>> GetSections(string examId);

        Task<IEnumerable<QuestionDto>> GetQuestions(string examId);

        Task<IEnumerable<QuestionDto>> GetQuestions(string examId, string sectionId);

        Task DeleteExam(string id);

        Task DeleteSection(string examId, string sectionId);

        Task DeleteQuestion(string examId, string sectionId, string id);
    }
}