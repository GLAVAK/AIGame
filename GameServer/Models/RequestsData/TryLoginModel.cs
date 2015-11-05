using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameServer.Models.RequestsData
{
    public class TryLoginModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}