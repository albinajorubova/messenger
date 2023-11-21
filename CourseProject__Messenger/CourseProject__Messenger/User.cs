using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject__Messenger.usercontrols
{
    internal class User
    {
    }
    public class Usercontrols
    {
        public string Email { get; set; }
        public string UserName { get; set; }

        public Usercontrols(string email, string userName)
        {
            Email = email;
            UserName = userName;
        }
    }
}
