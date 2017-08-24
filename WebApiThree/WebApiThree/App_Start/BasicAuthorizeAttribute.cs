using System;
using System.Text;
using System.Text.RegularExpressions;
using RunTecMs.Common.DBUtility;
using RunTecMs.Common.DEncrypt;
using RunTecMs.Common.WebUtility;
using RunTecMs.Model.FaFaApi;

namespace RunTechMs.Api
{
    public class BasicAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization != null)
            {
                var auth = actionContext.Request.Headers.Authorization;
                string authHeader = actionContext.Request.Headers.Authorization.Parameter;

                if (auth == null || string.IsNullOrEmpty(authHeader))
                {
                    HandleUnauthorizedRequest(actionContext);
                    return;
                }

                if ((authHeader.Length % 4) != 0 && !Regex.IsMatch(authHeader, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
                {
                    HandleUnauthorizedRequest(actionContext);
                    return;
                }

                string userInfo = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader));

                if (!userInfo.Contains(":"))
                {
                    HandleUnauthorizedRequest(actionContext);
                    return;
                }

                var tokens = userInfo.Split(':');

                if (tokens.Length < 2)
                {
                    HandleUnauthorizedRequest(actionContext);
                    return;
                }

                if (auth.Scheme == "Basic")
                {
                    LoginNameLogin(actionContext, userInfo);
                    return;
                }

                if (auth.Scheme == "Weixin")
                {
                    WeixinLogin(actionContext, userInfo);
                    return;
                }
                HandleUnauthorizedRequest(actionContext);
            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var challengeMessage = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            challengeMessage.Headers.Add("WWW-Authenticate", "Basic");
            throw new System.Web.Http.HttpResponseException(challengeMessage);
        }

        private void LoginNameLogin(System.Web.Http.Controllers.HttpActionContext actionContext, string userInfo)
        {
            var tokens = userInfo.Split(':');
            var loginName = tokens[0].Trim();
            var password = tokens[1].Trim();

            var obj = CacheHelper.GetCache("bu_" + loginName);

            if (obj != null)
            {
                var cacheUser = (string)obj;
                var passArr = cacheUser.Split(':');
                var calcuPassword = DEncrypt.CalculatePassword(password, passArr[0]);

                if (calcuPassword == passArr[1])
                {
                    IsAuthorized(actionContext);
                    return;
                }
                CacheHelper.RemoveAllCache("bu_" + loginName);
            }

            RunTecMs.BLL.FaFaApi.User bll = new RunTecMs.BLL.FaFaApi.User();
            var user = bll.GetLoginUser(loginName);

            if (user == null)
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

            var calculatedPassword = DEncrypt.CalculatePassword(password, user.PwdSalt);

            if (calculatedPassword == user.Password)
            {
                IsAuthorized(actionContext);
                CacheHelper.SetCache("bu_" + loginName, user.PwdSalt + ":" + user.Password, TimeSpan.FromSeconds(20));
                user = null;
            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }

        }

        private void WeixinLogin(System.Web.Http.Controllers.HttpActionContext actionContext, string userInfo)
        {
            var tokens = userInfo.Split(':');
            var appId = tokens[0].Trim();
            var openId = tokens[1].Trim();

            var obj = CacheHelper.GetCache("bw_" + openId);

            if (obj != null)
            {
                if ((string)obj == appId)
                {
                    IsAuthorized(actionContext);
                    return;
                }
                CacheHelper.RemoveAllCache("bw_" + openId);
            }

            string appAppId = PubConstant.GetConfigString("AppAppId") ?? "";
            string mpAppId = PubConstant.GetConfigString("MpAppId") ?? "";
            string webAppId = PubConstant.GetConfigString("WebAppId") ?? "";

            RunTecMs.Model.FaFaApi.WeixinUser wxUser = new WeixinUser();

            if (!string.IsNullOrEmpty(appAppId))
            {
                if (appId == appAppId)
                {
                    wxUser.AppOpenID = openId;
                }
            }

            if (!string.IsNullOrEmpty(mpAppId))
            {
                if (appId == mpAppId)
                {
                    wxUser.MPOpenID = openId;
                }
            }

            if (!string.IsNullOrEmpty(webAppId))
            {
                if (appId == webAppId)
                {
                    wxUser.WebOpenID = openId;
                }
            }

            RunTecMs.BLL.FaFaApi.User bll = new RunTecMs.BLL.FaFaApi.User();
            var user = bll.GetWeixinLoginUser(wxUser);

            if (user == null)
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

            if ((user.AppOpenID == openId) || (user.MPOpenID == openId) || (user.WebOpenID == openId))
            {
                IsAuthorized(actionContext);
                CacheHelper.SetCache("bw_" + openId, appId, TimeSpan.FromSeconds(20));
                user = null;
            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }

        }
    }
}