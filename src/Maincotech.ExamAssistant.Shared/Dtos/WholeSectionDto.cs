using System.Collections.Generic;

namespace Maincotech.ExamAssistant.Dtos
{
    public class WholeSectionDto
    {
        public string ExamId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }
}