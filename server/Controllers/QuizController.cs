using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

using server.DBSystem;
using server.DBSystem.UserDataContainers;
using server.DBSystem.QuizDataContainers;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class QuizController : ControllerBase
    {
        private readonly ILogger<QuizController> _logger;
        private readonly DbManager _dbManager;

        public QuizController(ILogger<QuizController> logger, DbManager dbManager)
        {
            _logger = logger;
            _dbManager = dbManager;
            _dbManager.Initialize();
        }

        [HttpGet]
        public ActionResult<QuizData> GetQuiz(uint quizId)
        {
            if (_dbManager.TryGetQuizData(quizId, out var quizData))
            {
                return Ok(quizData);
            }
            return BadRequest();
        }

        [HttpGet]
        public ActionResult<IEnumerable<QuizData>> GetUserQuizes(uint userId)
        {

            if (!_dbManager.TryGetUserData(userId, out var userData))
            {
                return BadRequest("There is no such userId");
            }
            if (_dbManager.TryGetQuizDataByUser(userId, out var quizDatas))
            {
                return Ok(quizDatas);
            }

            return BadRequest();
        }

        [HttpGet]
        public ActionResult<IEnumerable<QuizData>> GetClassroomQuizes(uint classroomId)
        {

            if (!_dbManager.TryGetClassroomData(classroomId, out var userData))
            {
                return BadRequest("There is no such classroomId");
            }

            if (_dbManager.TryGetQuizDataByClassroom(classroomId, out var quizDatas))
            {
                return Ok(quizDatas);
            }

            return BadRequest();
        }

        [HttpGet]
        public ActionResult<QuizAnswerData> GetQuizAnswer(uint quizAnswerId)
        {
            if (!_dbManager.TryGetQuizAnswer(quizAnswerId, out var quizAnswer))
            {
                return BadRequest("there is no such quizAnswerId");
            }
            return Ok(quizAnswer);
        }

        [HttpGet]
        public ActionResult<IEnumerable<QuizAnswerData>> GetQuizAnswersByQuiz(uint quizId)
        {
            if (!_dbManager.TryGetQuizData(quizId, out var quizData))
            {
                return BadRequest("There is no such quizId");
            }

            if (_dbManager.TryGetQuizAnswersByQuiz(quizId, out var quizAnswers))
            {
                return Ok(quizAnswers);
            }
            return BadRequest();
        }

        [HttpPost]
        public ActionResult<QuizData> AddQuiz([FromBody] JObject data)
        {
            var userId = data["userId"].ToObject<uint>();
            var quizName = data["quizName"].ToObject<string>();
            var quizQuestions = data["questions"].ToObject<IEnumerable<QuizQuestionData>>().ToList();
            if (!_dbManager.TryGetClassroomData(userId, out var classroomData))
            {
                return BadRequest("There is no teacher with such userId");
            }
            if (_dbManager.TryCreateQuiz(userId, quizName, quizQuestions, out var quizData))
            {
                return Ok(quizData);
            }
            return BadRequest();
        }

        [HttpPost]
        public ActionResult<QuizAnswerData> AddQuizAnswer([FromBody] JObject data)
        {
            var quizParentId = data["quizParentId"].ToObject<uint>();
            var quizOwnerId = data["ownerId"].ToObject<uint>();
            var quizAnswers = data["answers"].ToObject<IEnumerable<QuizQuestionData>>().ToList();

            if (!_dbManager.TryGetQuizData(quizParentId, out var quizData))
            {
                return BadRequest("There is no Quiz with such QuizId");
            }
            if (_dbManager.TryCreateQuizAnswer(quizParentId, quizOwnerId, quizAnswers, out var quizAnswer))
            {
                return Ok(quizAnswer);
            }
            return BadRequest();
        }
    }
}