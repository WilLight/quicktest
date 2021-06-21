using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace server.DBSystem.QuizDataContainers
{
    /// <summary>
    /// Class, that contains data of the classroom.
    /// </summary>
    [BsonIgnoreExtraElements]
    public sealed class QuizQuestionData
    {
        private readonly uint _questionId;
        private readonly string _questionName;
        private readonly string _questionAnswer;

        public uint QuestionId => _questionId;
        public string QuestionName => _questionName;
        public string QuestionAnswer => _questionAnswer;

    }
}