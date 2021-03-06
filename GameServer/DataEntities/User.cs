﻿using DataBase;
using GameServer.Abstract;
using GameServer.GameLogic;
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

        public int Credits
        {
            get { return _credits; }
            set
            {
                _credits = value;
                if (dbEntry != null) dbEntry.Credits = value;
            }
        }
        private int _credits;

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
            }
        }
        private Ship _enemyShip;

        public SpaceStation spaceStation
        {
            get { return _spaceStation; }
            set
            {
                _spaceStation = value;
            }
        }
        private SpaceStation _spaceStation;

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