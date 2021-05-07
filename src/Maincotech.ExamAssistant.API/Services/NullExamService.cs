using Maincotech.ExamAssistant.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Services
{
    public class NullExamService : IExamService
    {
        public Task<ExamDto> CreateOrUpdateExam(ExamDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<QuestionDto> CreateOrUpdateQuestion(QuestionDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<SectionDto> CreateOrUpdateSection(SectionDto dto)
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

        public Task<IEnumerable<ExamDto>> GetExams()
        {
            throw new NotImplementedException();
        }

        public Task<LoadMoreResult<QuestionDto>> GetQuestions(string examId, LoadMoreQuery query)
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
