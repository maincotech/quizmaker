using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maincotech.ExamAssitant.Dtos
{
    public class ExamDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public int Duration { get; set; }
        public DateTime UpdateOn { get; set; }
        public string Description { get; set; }
        public int NumberOfQuestions { get; set; }
        public int NumberOfSections { get; set; }
    }
}
