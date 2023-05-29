using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoWantsMillionApp.Services.Exceptions
{
    public class UserNotFoundException : BaseServiceException
    {
        public UserNotFoundException() : base("User not found with this credentials",400)
        {

        }
    }
}
