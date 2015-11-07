using DataBase;
using GameServer.Abstract;
using GameServer.GameLogic.JSClasses;
using Jurassic;
using Jurassic.Library;
using System.Collections.Generic;

namespace GameServer.DataEntities
{
    public class User
    {
        private DBUser dbEntry;
        public IUserRepository repository;

        public int UserId
        {
            get { return dbEntry.UserId; }
        }

        public string Login
        {
            get { return _username; }
            set
            {
                _username = value;
                if (dbEntry != null) dbEntry.Username = value;
            }
        }
        private string _username;

        public string PasswordHash
        {
            get { return _passwordHash; }
            set
            {
                _passwordHash = value;
                if (dbEntry != null) dbEntry.PasswordHash = value;
            }
        }
        private string _passwordHash;

        public bool IsDead
        {
            get { return _isDead; }
            set
            {
                _isDead = value;
                if (dbEntry != null) dbEntry.IsDead = value;
            }
        }
        private bool _isDead;

        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                if (dbEntry != null) dbEntry.Code = value;
            }
        }
        private string _code;

        // Command to run once and then clear
        public string consoleCommand;

        public List<string> Log { get; set; }

        public Ship ship;
        public ScriptEngine engine;

        public Ship enemyShip
        {
            get { return _enemyShip; }
            set
            {
                _enemyShip = value;

                //Form array of Cells to be used in JavaScript
                /*ArrayInstance[] array = new ArrayInstance[ship.cells.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = engine.Array.New(ship.cells[i]);

                    for (int j = 0; j < ship.cells[i].Length; j++)
                    {
                        if (ship.cells[i][j] is CellEngine)
                        {
                            ((CellEngine)ship.cells[i][j]).repository = repository;
                        }
                    }
                }
                engine.SetGlobalValue("enemyCells", engine.Array.New(array));*/
            }
        }
        private Ship _enemyShip;

        public List<Event> Events = new List<Event>();

        public User(DBUser dbEntry, IUserRepository repository)
        {
            this.dbEntry = dbEntry;
            this.repository = repository;

            engine = new ScriptEngine();
            engine.ForceStrictMode = true;

            Log = new List<string>();
        }
    }
}