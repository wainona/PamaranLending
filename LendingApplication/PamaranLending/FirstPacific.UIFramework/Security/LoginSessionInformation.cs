using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstPacific.UIFramework
{
    public class LoginSessionInformation
    {
        public string Username { get; set; }
        public string UserType { get; set; }
        public int UserId { get; set; }

        public bool IsAuthenticated { get; set; }

        private LoginSessionInformation()
        {
            this.Username = string.Empty;
            this.IsAuthenticated = false;
        }

        public static LoginSessionInformation Instance
        {
            get
            {
                LoginSessionInformation sessionInfo = HttpContext.Current.Session["UserSessionInformation"] as LoginSessionInformation;
                if (sessionInfo == null)
                {
                    sessionInfo = new LoginSessionInformation();
                    HttpContext.Current.Session["UserSessionInformation"] = sessionInfo;
                }
                return sessionInfo;
            }
        }

        public bool SignIn(string username, string password, IAuthenticationProvider authProvider)
        {
            if (authProvider.Authenticate(username, password))
            {
                this.Username = authProvider.Username;
                this.UserType = authProvider.UserType;
                this.UserId = authProvider.UserId;
            }

            this.IsAuthenticated = authProvider.IsAuthenticated;

            return this.IsAuthenticated;
        }

        public void SignOut()
        {
            this.Username = string.Empty;
            this.IsAuthenticated = false;
            if (HttpContext.Current.Session["UserSessionInformation"] != null)
            {
                HttpContext.Current.Session["UserSessionInformation"] = null;
            }
        }
    }
}