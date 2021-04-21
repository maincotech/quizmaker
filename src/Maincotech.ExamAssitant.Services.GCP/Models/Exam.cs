using Google.Cloud.Firestore;

namespace Maincotech.ExamAssitant.Services.Models
{
    [FirestoreData]
    public class Exam
    {
        [FirestoreDocumentId]
        public DocumentReference Reference { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string Provider { get; set; }

        [FirestoreProperty]
        public int Duration { get; set; }

        [FirestoreDocumentUpdateTimestamp]
        public Timestamp UpdateOn { get; set; }

        [FirestoreProperty]
        public string Description { get; set; }

        [FirestoreProperty]
        public int NumberOfQuestions { get; set; }

        [FirestoreProperty]
        public int NumberOfSections { get; set; }
    }
}