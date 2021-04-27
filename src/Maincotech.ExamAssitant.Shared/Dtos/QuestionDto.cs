using System.Collections.Generic;

namespace Maincotech.ExamAssistant.Dtos
{
    public class QuestionDto
    {
        public string ExamId { get; set; }
        public string SectionId { get; set; }
        public string SectionName { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Analysis { get; set; }
        public QuestionTypes QuestionType { get; set; }
        public List<QuestionOptionDto> Options { get; set; }
    }

    public class QuestionOptionDto
    {
        public string Name { get; set; }
        public bool IsCorrect { get; set; }
        public string AnswerText { get; set; }
    }
}