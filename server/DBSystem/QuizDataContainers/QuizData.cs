using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace server.DBSystem.QuizDataContainers
{
    /// <summary>
    /// Class, that contains data of the classroom.
    /// </summary>
    [BsonIgnoreExtraElements]
    public sealed class QuizData
    {
        public uint QuizId { get; set; }
        public uint ClassId { get; set; }
        public string QuizName { get; set; }
        public List<uint> AnswerIds { get; set; }
        public List<QuizQuestionData> Questions { get; set; }

        public QuizData(uint quizId, uint classId, string quizName, List<uint> answers, List<QuizQuestionData> questions)
        {
            QuizId = quizId;
            ClassId = classId;
            QuizName = quizName;
            AnswerIds = answers;
            Questions = questions;
        }

        public QuizData(QuizData anotherQuiz)
        {
            QuizId = anotherQuiz.QuizId;
            ClassId = anotherQuiz.ClassId;
            QuizName = anotherQuiz.QuizName;
            AnswerIds = anotherQuiz.AnswerIds;
            Questions = anotherQuiz.Questions;
        }

    }
}