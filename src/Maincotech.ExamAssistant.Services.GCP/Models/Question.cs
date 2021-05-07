using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Services.Models
{
    [FirestoreData]
    public class Question
    {
        [FirestoreDocumentId]
        public DocumentReference Reference { get; set; }

        [FirestoreProperty]
        public string Title { get; set; }
        [FirestoreProperty]
        public string Analysis { get; set; }
        [FirestoreProperty]
        public QuestionTypes QuestionType { get; set; }

        [FirestoreProperty]
        public List<QuestionOption> Options { get; set; }
    }

    [FirestoreData]
    public class QuestionOption
    {
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public bool IsCorrect { get; set; }
        [FirestoreProperty]
        public string AnswerText { get; set; }
    }
}
