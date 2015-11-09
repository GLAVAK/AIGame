using DataBase;
using GameServer.Abstract;
using GameServer.Concrete;
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
using System.Web.Security;

namespace GameServer.Controllers
{
    public class LoginController : ApiController
    {
        IUserRepository repository;
        GameServerContext context;
        SecurityHelper helper;

        public LoginController(IUserRepository repository,
            GameServerContext context, SecurityHelper helper)
        {
            this.repository = repository;
            this.context = context;
            this.helper = helper;
        }

        [HttpPost]
        [ActionName("Index")]
        public object TryLogin(TryLoginModel tryLoginModel)
        {
            User user = repository.Users
                .Where(u => u.Login == tryLoginModel.Login &&
                    helper.ValidatePassword(tryLoginModel.Password, u.PasswordHash))
                .FirstOrDefault();

            if (user != null)
            {
                HttpContext.Current.Session["UserId"] = user.UserId;
                user.Events.Clear();
                return new { success = true };
            }
            else
            {
                return new { error = "Wrong username/password" };
            }
        }

        [HttpPost]
        public object Register(RegisterModel addUserModel)
        {
            User userWithSameLogin = repository.Users
                .Where(u => u.Login == addUserModel.Login)
                .FirstOrDefault();
            if (userWithSameLogin != null)
                return new { error = "The username already taken" };

            if (addUserModel.Password != addUserModel.PasswordRepeat)
                return new { error = "Passwords does not match" };

            if (addUserModel.Password.Length < 3)
                return new { error = "Password too short" };

            DBUser user = new DBUser();
            user.Username = addUserModel.Login;
            user.PasswordHash = helper.CreateHash(addUserModel.Password);
            user.ShipPresetId = 1;
            user.Code = "";

            user.CellTypes = new PresetShipRepository()
                .GetShip(user.ShipPresetId)
                .DefaultLayout;            

            context.Users.Add(user);
            context.SaveChanges();

            repository.UpdateFromDB(context);
            return new { success = true };
        }
    }
}
