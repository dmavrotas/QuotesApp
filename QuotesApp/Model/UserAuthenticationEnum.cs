using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesApp.Model
{
    public enum UserAuthenticationEnum
    {
        Success = 0,
        UserNotFound = 1,
        UserCredentialsWrong = 2
    }
}
