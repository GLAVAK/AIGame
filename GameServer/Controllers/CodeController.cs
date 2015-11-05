using DataBase;
using GameServer.Abstract;
using GameServer.DataEntities;
using GameServer.Infrastructure;
using GameServer.Models.RequestsData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace GameServer.Controllers
{
    public class CodeController : ApiController
    {
        IUserRepository repository;
        GameServerContext context;
        SecurityHelper helper;

        public CodeController(IUserRepository repository,
            GameServerContext context, SecurityHelper helper)
        {
            this.repository = repository;
            this.context = context;
            this.helper = helper;
        }

        [HttpGet]
        [ActionName("Index")]
        public string GetCode()
        {
            User currentUser = helper.GetCurrentUser(HttpContext.Current.Session["UserId"]);
            if (currentUser == null) return null;

            return currentUser.Code;
        }

        [HttpPost]
        [ActionName("Index")]
        public bool Submit(SubmitCodeModel submitCodeModel)
        {
            User currentUser = helper.GetCurrentUser(HttpContext.Current.Session["UserId"]);
            if (currentUser == null) return false;

            currentUser.Code = submitCodeModel.Code;
            context.SaveChanges();

            return true;
        }

        [HttpPost]
        [ActionName("Console")]
        public bool SubmitConsoleCommand(SubmitCodeModel submitCodeModel)
        {
            User currentUser = helper.GetCurrentUser(HttpContext.Current.Session["UserId"]);
            if (currentUser == null) return false;

            currentUser.consoleCommand += submitCodeModel.Code + "\n";

            return true;
        }
    }
}
