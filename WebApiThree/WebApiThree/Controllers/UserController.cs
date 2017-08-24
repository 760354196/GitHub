using ApiBll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiModel;

namespace WebApiThree.Controllers
{
    public class UserController : ApiController
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        [HttpGet]
        public UserInfo GetUserInfo(string userName)
        {
            var model = new UserBll().GetUserInfo(userName);
            return model;
        }
    }
}
