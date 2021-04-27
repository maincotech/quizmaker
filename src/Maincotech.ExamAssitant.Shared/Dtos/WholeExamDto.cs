using System;
using System.Collections.Generic;

namespace Maincotech.ExamAssistant.Dtos
{
    public class WholeExamDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Vendor { get; set; }
        public int Duration { get; set; }
        public DateTime UpdateOn { get; set; }
        public string Description { get; set; }
        public int NumberOfQuestions { get; set; }
        public List<WholeSectionDto> Sections { get; set; }
    }
}