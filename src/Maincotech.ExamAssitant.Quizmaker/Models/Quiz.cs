using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Models
{
    public class Quiz
    {
        public string Id { get; set; }
    }

    public enum QuestionTypes
    {
        SingleChoice,
        MultipleChoice,
        Statements,
        DragDrop
    }
}
