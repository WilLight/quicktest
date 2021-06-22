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
        private readonly uint _quizId;
        private readonly uint _ownerId;
        private readonly string _quizName;
        private readonly List<uint> _studentIds;
        private readonly List<QuizQuestionData> _questions;

        public uint QuizId => _quizId;
        public uint OwnerId => _ownerId;
        public string QuizName => _quizName;
        public List<uint> StudentIds => _studentIds;
        public List<QuizQuestionData> Questions => _questions;

        public QuizData(uint quizId, uint ownerId, string quizName, List<uint> studentIds, List<QuizQuestionData> questions)
        {
            _quizId = quizId;
            _ownerId = ownerId;
            _quizName = quizName;
            _studentIds = studentIds;
            _questions = questions;
        }

        public QuizData(QuizData anotherQuiz)
        {
            _quizId = anotherQuiz.QuizId;
            _ownerId = anotherQuiz.OwnerId;
            _quizName = anotherQuiz.QuizName;
            _studentIds = anotherQuiz.StudentIds;
            _questions = anotherQuiz.Questions;
        }

    }
}