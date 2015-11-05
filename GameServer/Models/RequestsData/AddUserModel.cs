using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameServer.Models.RequestsData
{
    public class RegisterModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }
    }
}