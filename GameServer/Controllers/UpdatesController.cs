using GameServer.Abstract;
using GameServer.DataEntities;
using GameServer.Infrastructure;
using GameServer.Models.ResponseData;
using System.Web;
using System.Web.Http;

namespace GameServer.Controllers
{
    public class UpdatesController : ApiController
    {
        IUserRepository repository;
        SecurityHelper helper;

        public UpdatesController(IUserRepository repository, SecurityHelper helper)
        {
            this.repository = repository;
            this.helper = helper;
        }

        [HttpGet]
        [ActionName("Index")]
        public UpdateData GetUpdates()
        {
            User currentUser = helper.GetCurrentUser(HttpContext.Current.Session["UserId"]);
            if (currentUser == null) return null;

            UpdateData result = new UpdateData();

            result.Events = new Event[currentUser.Events.Count];
            currentUser.Events.CopyTo((Event[])result.Events);
            currentUser.Events.Clear();

            result.ShipStatus = currentUser.ship.status;

            result.Log = new string[currentUser.Log.Count];
            currentUser.Log.CopyTo((string[])result.Log);
            currentUser.Log.Clear();

            if(currentUser.enemyShip != null)
            {
                result.RadarType = RadarType.EnemyShip;
                result.RadarData = currentUser.enemyShip.status;
            }

            return result;
        }
    }
}
