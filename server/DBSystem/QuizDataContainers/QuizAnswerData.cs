using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace server.DBSystem.QuizDataContainers
{
    /// <summary>
    /// Class, that contains data of individual question.
    /// </summary>
    public sealed class QuizAnswerData
    {
        public uint QuizId { get; set; }
        public uint OwnerId { get; set; }
        public List<QuizQuestionData> Questions { get; set; }

        public QuizAnswerData(uint quizId, uint ownerId, List<QuizQuestionData> questions)
        {
            QuizId = quizId;
            OwnerId = ownerId;
            Questions = questions;
        }

        public QuizAnswerData(QuizAnswerData anotherQuiz)
        {
            QuizId = anotherQuiz.QuizId;
            OwnerId = anotherQuiz.OwnerId;
            Questions = anotherQuiz.Questions;
        }
    }
}