using DataBase;
using GameServer.Abstract;
using GameServer.DataEntities;
using GameServer.GameLogic;
using GameServer.GameLogic.JSClasses;
using Jurassic.Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Concrete
{
    public class MemoryUserRepository : IUserRepository
    {
        public List<User> _users;

        public IQueryable<User> Users
        {
            get { return _users.AsQueryable(); }
        }

        public MemoryUserRepository(GameServerContext context)
        {
            /*DBUser u = new DBUser();
            u.Username = "Glavak";
            u.ShipPresetId = 1;
            u.Code = "";
            u.CellTypes = new int[] { 0, 2, 1, 3, 1, 2 };
            u.PasswordHash = "123";
            context.Users.Add(u);
            context.SaveChanges();*/

            UpdateFromDB(context);
        }

        /// <summary>
        /// Loading all users from database to Users list
        /// </summary>
        /// <param name="context"></param>
        public void UpdateFromDB(GameServerContext context)
        {
            _users = new List<User>();
            foreach (DBUser dbUser in context.Users)
            {
                addUserFromDB(dbUser);
            }

            if(_users.Count >= 2)
            {
                _users[0].enemyShip = _users[1].ship;
                _users[1].enemyShip = _users[0].ship;
            }
        }

        /// <summary>
        /// Converts dbUser to User and adds it to Users list
        /// </summary>
        /// <param name="dbUser"></param>
        public void addUserFromDB(DBUser dbUser)
        {
            User user = new User(dbUser, this);

            Ship ship = new ShipBuilder().BuildShip(user,
                new PresetShipRepository().GetShip(dbUser.ShipPresetId),
                dbUser.CellTypes);

            //Form array of Cells to be used in JavaScript
            ArrayInstance[] array = new ArrayInstance[ship.cells.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = user.engine.Array.New(ship.cells[i]);

                for (int j = 0; j < ship.cells[i].Length; j++)
                {
                    if (ship.cells[i][j] is CellEngine)
                    {
                        ((CellEngine)ship.cells[i][j]).repository = this;
                    }
                }
            }
            user.engine.SetGlobalValue("cells", user.engine.Array.New(array));
            user.engine.SetGlobalValue("radar", new Radar(user));

            Action<string> logAction = delegate(string s) { user.Log.Add(s); };
            user.engine.SetGlobalFunction("log", logAction);

            user.Login = dbUser.Username;
            user.PasswordHash = dbUser.PasswordHash;
            user.Code = dbUser.Code;
            user.ship = ship;
            _users.Add(user);
        }
    }
}
