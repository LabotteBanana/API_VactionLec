using DotnetCoreServer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetCoreServer.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        IUserDao userDao;

        public UserController(IUserDao userDao){
            this.userDao = userDao;
        }

        [HttpGet]
        public UserResult Info(long UserID){
            Console.WriteLine("info");
            UserResult result = new UserResult();
            result.Data = userDao.GetUser(UserID);
            return result;

        }

        [HttpPost]
        public UserResult Update([FromBody] User requestUser){

            UserResult result = new UserResult();
            userDao.UpdateUser(requestUser);
            
            result.Data = userDao.GetUser(requestUser.UserID);

            result.ResultCode = 1;
            result.Message = "Success";

            return result;
        }

    }

}