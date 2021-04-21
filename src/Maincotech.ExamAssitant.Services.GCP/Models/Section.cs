using Google.Cloud.Firestore;

namespace Maincotech.ExamAssitant.Services.Models
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