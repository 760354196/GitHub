using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiThree.V1._0
{
    public class HomeController : ApiController
    {
        [HttpGet]
        public dynamic GetNewUser()
        {
            return new
            {
                UserID = 1,
                UserName = "张带兵"
            };
        }
    }
}
