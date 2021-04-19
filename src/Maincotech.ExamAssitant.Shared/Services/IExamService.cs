using Maincotech.ExamAssitant.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maincotech.ExamAssitant.Services
{
    public interface IExamService
    {
        Task CreateOrUpdateExam(ExamDto dto);

        Task CreateOrUpdateSection(SectionDto dto);

        Task CreateOrUpdateSection(QuestionDto dto);

        Task<ExamDto> GetExam(string id);

        Task<IEnumerable<SectionDto>> GetSections(string examId);

        Task<IEnumerable<QuestionDto>> GetQuestions(string examId);

        Task<IEnumerable<QuestionDto>> GetQuestions(string examId, string sectionId);

        Task DeleteExam(string id);

        Task DeleteSection(string examId, string sectionId);

        Task DeleteQuestion(string examId, string sectionId, string id);
    }
}