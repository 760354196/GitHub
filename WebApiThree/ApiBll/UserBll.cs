using ApiDal;
using ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBll
{
    public class UserBll
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public UserInfo GetUserInfo(string userName)
        {
            var sql = "select * from  User_Info where  UserName=@userName";
            return  DbService.GetList<UserInfo>(sql, new { userName }).FirstOrDefault();
        }

    }
}
