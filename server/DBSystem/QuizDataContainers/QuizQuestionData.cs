using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace server.DBSystem.QuizDataContainers
{
    /// <summary>
    /// Class, that contains data of individual question.
    /// </summary>
    public sealed class QuizQuestionData
    {
        public uint QuestionId { get; set; }
        public string QuestionName { get; set; }
        public string QuestionAnswer { get; set; }

    }
}