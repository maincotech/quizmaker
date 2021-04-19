using Maincotech.Quizmaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maincotech.Quizmaker.Services
{

    public interface IQuizService
    {
        Task<Quiz> GetQuiz(string id);

    }
    public class QuizService : IQuizService
    {
        public async Task<Quiz> GetQuiz(string id)
        {
            return new Quiz();
        }
    }
}
