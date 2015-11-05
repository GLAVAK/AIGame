using GameServer.Abstract;
using GameServer.DataEntities;
using GameServer.Infrastructure;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace GameServer.Controllers
{
    public class ShipController : ApiController
    {
        IUserRepository repository;
        SecurityHelper helper;

        public ShipController(IUserRepository repository, SecurityHelper helper)
        {
            this.repository = repository;
            this.helper = helper;
        }

        [HttpGet]
        [ActionName("Index")]
        public Ship GetShip()
        {
            User currentUser = helper.GetCurrentUser(HttpContext.Current.Session["UserId"]);
            if (currentUser == null) return null;

            return currentUser.ship;
        }

        [HttpGet]
        [ActionName("enemyShip")]
        public Ship GetEnemyShip()
        {
            User currentUser = helper.GetCurrentUser(HttpContext.Current.Session["UserId"]);
            if (currentUser == null) return null;

            if (currentUser == null || currentUser.enemyShip == null) return null;

            return currentUser.enemyShip;
        }
    }
}
