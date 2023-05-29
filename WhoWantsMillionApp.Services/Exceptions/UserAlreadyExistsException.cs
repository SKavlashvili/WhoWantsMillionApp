using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoWantsMillionApp.Services.Exceptions
{
    public class UserAlreadyExistsException : BaseServiceException
    {
        public UserAlreadyExistsException() : base("this user already exists",400)
        {

        }
    }
}
