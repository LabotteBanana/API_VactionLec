using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotnetCoreServer.Models;

namespace DotnetCoreServer.Controllers
{
    [ApiController]
    [Route("Login")]
    public class LoginController : ControllerBase
    {
        private readonly IUserDao userDao;

        public LoginController(IUserDao userDao)
        {
            this.userDao = userDao;
        }

        // GET Login/5
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var user = userDao.GetUser(id);
            return Ok(user);
        }

        // POST Login/Facebook
        [HttpPost("Facebook")]
        public IActionResult Facebook([FromBody] User requestUser)
        {
            if (requestUser == null)
                return BadRequest("Request body is null");

            if (string.IsNullOrEmpty(requestUser.FacebookID))
                return BadRequest("FacebookID is required");

            LoginResult result = new LoginResult();

            var user = userDao.FindUserByFUID(requestUser.FacebookID);

            if (user != null && user.UserID > 0)
            {
                result.Data = user;
                result.Message = "OK";
                result.ResultCode = 1;
            }
            else
            {
                requestUser.AccessToken = Guid.NewGuid().ToString();
                userDao.InsertUser(requestUser);

                user = userDao.FindUserByFUID(requestUser.FacebookID);

                result.Data = user;
                result.Message = "New User";
                result.ResultCode = 2;
            }

            return Ok(result);
        }
    }
}

/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotnetCoreServer.Models;

namespace DotnetCoreServer.Controllers
{
    [Route("[controller]/[action]")]
    public class LoginController : Controller
    {
        IUserDao userDao;
        public LoginController(IUserDao userDao){
            this.userDao = userDao;
        }
        
        // GET api/user/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            User user = userDao.GetUser(id);
            return user;
        }
        
        // POST Login/Facebook
        [HttpPost]
        public LoginResult Facebook([FromBody] User requestUser)
        {

            LoginResult result = new LoginResult();
            
            User user = userDao.FindUserByFUID(requestUser.FacebookID);
            
            if(user != null && user.UserID > 0){ // 이미 가입되어 있음
                
                result.Data = user;
                result.Message = "OK";
                result.ResultCode = 1;

                return result;

            } else { // 회원가입 해야함
                
                string AccessToken = Guid.NewGuid().ToString();

                requestUser.AccessToken = AccessToken;
                userDao.InsertUser(requestUser);
                user = userDao.FindUserByFUID(requestUser.FacebookID);
                result.Data = user;
                result.Message = "New User";
                result.ResultCode = 2;

                return result;

            }
        }
    }
}
*/