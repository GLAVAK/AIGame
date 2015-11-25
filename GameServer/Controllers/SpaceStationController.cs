using GameServer.Abstract;
using GameServer.DataEntities;
using GameServer.GameLogic;
using GameServer.Infrastructure;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace GameServer.Controllers
{
    public class SpaceStationController : ApiController
    {
        IUserRepository repository;
        SecurityHelper helper;

        public SpaceStationController(IUserRepository repository, SecurityHelper helper)
        {
            this.repository = repository;
            this.helper = helper;
        }

        [HttpGet]
        [ActionName("Index")]
        public SpaceStation GetStation()
        {
            User currentUser = helper.GetCurrentUser(HttpContext.Current.Session["UserId"]);
            if (currentUser == null) return null;

            if (currentUser.spaceStation == null) return null;

            return currentUser.spaceStation;
        }
    }
}
