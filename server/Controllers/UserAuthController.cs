using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

using server.DBSystem;
using server.DBSystem.UserDataContainers;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserAuthController : ControllerBase
    {
        private readonly ILogger<UserAuthController> _logger;
        private readonly DbManager _dbManager;

        public UserAuthController(ILogger<UserAuthController> logger, DbManager dbManager)
        {
            _logger = logger;
            _dbManager = dbManager;
        }

        [HttpPost]
        public ActionResult<bool> LogIn(UserCredentials loginData)
        {
            UserData userData;
            if (_dbManager.TryLogInUser(loginData, out userData))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<bool> Register([FromBody] JObject data)
        {
            UserCredentials registerData = data["registerData"].ToObject<UserCredentials>();
            UserRole userRole = data["userRole"].ToObject<UserRole>();
            UserData userData;
            if (_dbManager.TryRegisterUser(userRole, registerData, out userData))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<bool> Remove(uint userId)
        {
            _dbManager.RemoveUser(userId);

            return Ok();
        }
    }
}
