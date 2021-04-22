using Google.Cloud.Firestore;
using Maincotech.Data;
using Maincotech.ExamAssitant.Dtos;
using Maincotech.ExamAssitant.Services.Models;
using Maincotech.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Maincotech.ExamAssitant.Services
{
    public class ExamService : IExamService
    {
        private static readonly string _examCollectionName = "exams";
        private static readonly string _sectionCollectionName = "sections";
        private static readonly string _questionCollectionName = "questions";
        private static readonly Logging.ILogger Logger = AppRuntimeContext.Current.GetLogger<ExamService>();
        private readonly FirestoreDb db;

        public ExamService(string projectId, string jsonCredentials)
        {
            var dbBuilder = new FirestoreDbBuilder
            {
                ProjectId = projectId,
                JsonCredentials = jsonCredentials
            };
            db = dbBuilder.Build();
        }

        public async Task<ExamDto> CreateOrUpdateExam(ExamDto dto)
        {
            var entity = dto.To<Exam>();

            if (string.IsNullOrEmpty(dto.Id))
            {
                //create new
                var docRef = await db.Collection(_examCollectionName).AddAsync(entity);
                dto.Id = docRef.Id;
            }
            else
            {
                //update
                await db.Collection(_examCollectionName).Document(dto.Id).SetAsync(entity);
            }

            dto.UpdateOn = DateTime.UtcNow;

            return dto;
        }

        public async Task<SectionDto> CreateOrUpdateSection(SectionDto dto)
        {
            var examDocRef = db.Collection(_examCollectionName).Document(dto.ExamId);
            var examSnapshot = await examDocRef.GetSnapshotAsync();
            ParameterChecker.Against<FileNotFoundException>(examSnapshot.Exists == false, $"Exam {examSnapshot.Id} does not exist!");

            var exam = examSnapshot.ConvertTo<Exam>();

            var sectionCollectionRef = examDocRef.Collection(_sectionCollectionName);

            var section = dto.To<Section>();
            if (string.IsNullOrEmpty(dto.Id))
            {
                //create new
                var docRef = await sectionCollectionRef.AddAsync(section);
                dto.Id = docRef.Id;

                //update exam section count
                await examDocRef.UpdateAsync(new Dictionary<string, object> { { "NumberOfSections", exam.NumberOfSections + 1 } });
            }
            else
            {
                //update
                await sectionCollectionRef.Document(dto.Id).SetAsync(section);
            }

            return dto;
        }

        public async Task<QuestionDto> CreateOrUpdateQuestion(QuestionDto dto)
        {
            var examDocRef = db.Collection(_examCollectionName).Document(dto.ExamId);
            var examSnapshot = await examDocRef.GetSnapshotAsync();
            ParameterChecker.Against<FileNotFoundException>(examSnapshot.Exists == false, $"Exam {examSnapshot.Id} does not exist!");

            var exam = examSnapshot.ConvertTo<Exam>();

            var sectionCollectionRef = examDocRef.Collection(_sectionCollectionName);
            var sectionDocRef = sectionCollectionRef.Document(dto.SectionId);
            var sectionSnapshot = await sectionDocRef.GetSnapshotAsync();
            ParameterChecker.Against<FileNotFoundException>(sectionSnapshot.Exists == false, $"Exam {exam.Name} does not contain section {dto.SectionId}!");

            var section = sectionSnapshot.ConvertTo<Section>();
            var entity = dto.To<Question>();
            var questionCollectionRef = sectionDocRef.Collection(_questionCollectionName);
            if (string.IsNullOrEmpty(dto.Id))
            {
                try
                {
                    //create new
                    var docRef = await questionCollectionRef.AddAsync(entity);
                    dto.Id = docRef.Id;

                    //update section question count
                    await sectionDocRef.UpdateAsync(new Dictionary<string, object> { { "NumberOfQuestions", section.NumberOfQuestions + 1 } });

                    //update exam question count
                    await examDocRef.UpdateAsync(new Dictionary<string, object> { { "NumberOfQuestions", exam.NumberOfQuestions + 1 } });
                }
                catch (Exception ex)
                {
                    Logger.Error("Failed to add questions", ex);
                    throw;
                }
            }
            else
            {
                //update
                await questionCollectionRef.Document(dto.Id).SetAsync(entity);
            }

            return dto;
        }

        public async Task DeleteExam(string id)
        {
            var examDocRef = db.Collection(_examCollectionName).Document(id);
            var examSnapshot = await examDocRef.GetSnapshotAsync();
            if (examSnapshot.Exists)
            {
                await examDocRef.DeleteAsync();
            }
        }

        public async Task DeleteQuestion(string examId, string sectionId, string id)
        {
            var examDocRef = db.Collection(_examCollectionName).Document(examId);
            var examSnapshot = await examDocRef.GetSnapshotAsync();
            ParameterChecker.Against<FileNotFoundException>(examSnapshot.Exists == false, $"Exam {examSnapshot.Id} does not exist!");

            var exam = examSnapshot.ConvertTo<Exam>();

            var sectionCollectionRef = examDocRef.Collection(_sectionCollectionName);
            var sectionDocRef = sectionCollectionRef.Document(sectionId);
            var sectionSnapshot = await sectionDocRef.GetSnapshotAsync();
            ParameterChecker.Against<FileNotFoundException>(sectionSnapshot.Exists == false, $"Exam {exam.Name} does not contain section {sectionId}!");

            var section = sectionSnapshot.ConvertTo<Section>();
            var questionDocRef = sectionDocRef.Collection(_questionCollectionName).Document(id);
            var questionSnapshot = await questionDocRef.GetSnapshotAsync();
            if (questionSnapshot.Exists)
            {
                //delet question
                await questionDocRef.DeleteAsync();

                //update section question count
                await sectionDocRef.UpdateAsync(new Dictionary<string, object> { { "NumberOfQuestions", section.NumberOfQuestions - 1 } });

                //update exam question count
                await examDocRef.UpdateAsync(new Dictionary<string, object> { { "NumberOfQuestions", exam.NumberOfQuestions - 1 } });
            }
        }

        public async Task DeleteSection(string examId, string sectionId)
        {
            var examDocRef = db.Collection(_examCollectionName).Document(examId);
            var examSnapshot = await examDocRef.GetSnapshotAsync();
            ParameterChecker.Against<FileNotFoundException>(examSnapshot.Exists == false, $"Exam {examSnapshot.Id} does not exist!");

            var exam = examSnapshot.ConvertTo<Exam>();
            var sectionCollectionRef = examDocRef.Collection(_sectionCollectionName);
            var docRef = sectionCollectionRef.Document(sectionId);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                var section = snapshot.ConvertTo<Section>();

                await examDocRef.UpdateAsync(new Dictionary<string, object> {
                    { "NumberOfQuestions", exam.NumberOfQuestions - section.NumberOfQuestions },
                    {"NumberOfSections", exam.NumberOfSections-1 } });
                await docRef.DeleteAsync();
            }
        }

        public async Task<ExamDto> GetExam(string id)
        {
            DocumentReference docRef = db.Collection(_examCollectionName).Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                var entity = snapshot.ConvertTo<Exam>();
                return entity.To<ExamDto>();
            }

            throw new FileNotFoundException($"Document {snapshot.Id} does not exist!");
        }

        public async Task<IEnumerable<QuestionDto>> GetQuestions(string examId, string sectionId)
        {
            var result = new List<QuestionDto>();
            Query query = db.Collection(_examCollectionName).Document(examId)
                .Collection(_sectionCollectionName).Document(sectionId).Collection(_questionCollectionName);
            var snapshot = await query.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
            {
                var entity = documentSnapshot.ConvertTo<Question>();
                var dto = entity.To<QuestionDto>();
                dto.ExamId = examId;
                dto.SectionId = sectionId;
                result.Add(dto);
            }
            return result;
        }

        public async Task<IEnumerable<SectionDto>> GetSections(string examId)
        {
            var result = new List<SectionDto>();
            Query allSectionsQuery = db.Collection(_examCollectionName).Document(examId).Collection(_sectionCollectionName);
            QuerySnapshot allCitiesQuerySnapshot = await allSectionsQuery.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in allCitiesQuerySnapshot.Documents)
            {
                var entity = documentSnapshot.ConvertTo<Section>();
                result.Add(entity.To<SectionDto>());
            }
            return result;
        }

        public async Task<IEnumerable<ExamDto>> GetExams()
        {
            var result = new List<ExamDto>();
            Query allCitiesQuery = db.Collection(_examCollectionName);
            QuerySnapshot allCitiesQuerySnapshot = await allCitiesQuery.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in allCitiesQuerySnapshot.Documents)
            {
                var entity = documentSnapshot.ConvertTo<Exam>();
                result.Add(entity.To<ExamDto>());
            }
            return result;
        }

        public async Task<PagedResult<QuestionDto>> GetQuestions(Pagination pagination, string examId, string searchText)
        {
            var examDocRef = db.Collection(_examCollectionName).Document(examId);
            var examSnapshot = await examDocRef.GetSnapshotAsync();
            ParameterChecker.Against<FileNotFoundException>(examSnapshot.Exists == false, $"Exam {examSnapshot.Id} does not exist!");

            var exam = examSnapshot.ConvertTo<Exam>();

            var sections = new List<Section>();
            Query allSectionsQuery = db.Collection(_examCollectionName).Document(examId).Collection(_sectionCollectionName);
            QuerySnapshot allCitiesQuerySnapshot = await allSectionsQuery.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in allCitiesQuerySnapshot.Documents)
            {
                var section = documentSnapshot.ConvertTo<Section>();
                sections.Add(section);
            }

            var startIndex = pagination.PageSize * (pagination.PageNumber-1) + pagination.Start;
            var endIndex = startIndex + pagination.PageSize;

            var needSections = new List<SectionQuery>();
            var currentIndex = 0;
            var remain = pagination.PageSize;
            foreach (var section in sections)
            {
                if (currentIndex < startIndex)
                {
                    if (currentIndex + section.NumberOfQuestions < startIndex)
                    {
                        currentIndex += section.NumberOfQuestions;
                        continue;
                    }
                    else
                    {
                        var start = startIndex - currentIndex;
                        var canTake = section.NumberOfQuestions - start;
                        var sectionQuery = new SectionQuery
                        {
                            ExamId = exam.Reference.Id,
                            Section = section,
                            Offset = start,
                            Limit = remain < canTake ? remain : canTake
                        };
                        remain -= sectionQuery.Limit;
                        needSections.Add(sectionQuery);
                    }
                }
                else
                {
                    var sectionQuery = new SectionQuery
                    {
                        ExamId = exam.Reference.Id,
                        Section = section,
                        Offset = 0,
                        Limit = remain < section.NumberOfQuestions ? remain : section.NumberOfQuestions
                    };
                    remain -= sectionQuery.Limit;
                    needSections.Add(sectionQuery);
                }
                if (remain == 0)
                {
                    break;
                }
            }
            var dtos = new List<QuestionDto>();
            foreach (var sectionQuery in needSections)
            {
                var queryResult = await QuerySection(sectionQuery);
                dtos.AddRange(queryResult);
            }

            return new PagedResult<QuestionDto>(exam.NumberOfQuestions, pagination.PageSize, pagination.PageNumber, dtos);
        }

        private async Task<IEnumerable<QuestionDto>> QuerySection(SectionQuery query)
        {
            var result = new List<QuestionDto>();
            var questionsQuery = query.Section.Reference.Collection(_questionCollectionName).Offset(query.Offset).Limit(query.Limit);
            QuerySnapshot questionsSnapshot = await questionsQuery.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in questionsSnapshot.Documents)
            {
                var entity = documentSnapshot.ConvertTo<Question>();
                var dto = entity.To<QuestionDto>();
                dto.ExamId = query.ExamId;
                dto.SectionId = query.Section.Reference.Id;
                result.Add(dto);
            }
            return result;
        }

        private class SectionQuery
        {
            public string ExamId { get; set; }
            public Section Section { get; set; }
            public int Offset { get; set; }
            public int Limit { get; set; }
            public string SearchText { get; set; }
        }
    }
}