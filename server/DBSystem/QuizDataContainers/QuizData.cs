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
        public uint OwnerId { get; set; }
        public string QuizName { get; set; }
        public List<QuizAnswerData> Answers { get; set; }
        public List<QuizQuestionData> Questions { get; set; }

        public QuizData(uint quizId, uint ownerId, string quizName, List<QuizAnswerData> answers, List<QuizQuestionData> questions)
        {
            QuizId = quizId;
            OwnerId = ownerId;
            QuizName = quizName;
            Answers = answers;
            Questions = questions;
        }

        public QuizData(QuizData anotherQuiz)
        {
            QuizId = anotherQuiz.QuizId;
            OwnerId = anotherQuiz.OwnerId;
            QuizName = anotherQuiz.QuizName;
            Answers = anotherQuiz.Answers;
            Questions = anotherQuiz.Questions;
        }

    }
}