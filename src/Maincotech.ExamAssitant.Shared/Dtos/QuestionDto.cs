using System.Collections.Generic;

namespace Maincotech.ExamAssitant.Dtos
{
    public class QuestionDto
    {
        public string ExamId { get; set; }
        public string SectionId { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Analysis { get; set; }
        public QuestionTypes QuestionType { get; set; }
        public IList<QuestionOptionDto> Options { get; set; }
    }

    public class QuestionOptionDto
    {
        public string Name { get; set; }
        public bool IsCorrect { get; set; }
        public string AnswerText { get; set; }
    }
}