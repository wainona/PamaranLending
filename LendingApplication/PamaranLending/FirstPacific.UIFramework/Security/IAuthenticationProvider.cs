using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstPacific.UIFramework
{
    public interface IAuthenticationProvider
    {
        string Username { get;}
        string UserType { get; }
        int UserId { get; }

        bool IsAuthenticated { get; }

        bool Authenticate(string username, string password);
    }
}
