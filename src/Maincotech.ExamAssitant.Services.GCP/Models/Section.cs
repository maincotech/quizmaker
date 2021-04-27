using Google.Cloud.Firestore;

namespace Maincotech.ExamAssistant.Services.Models
{
    [FirestoreData]
    public class Section
    {
        [FirestoreDocumentId]
        public DocumentReference Reference { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string Description { get; set; }

        [FirestoreProperty]
        public int NumberOfQuestions { get; set; }
    }
}