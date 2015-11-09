using GameServer.Abstract;
using GameServer.DataEntities;
using System;

namespace GameServer.GameLogic
{
    public class WorldUpdater
    {
        IUserRepository repository;

        public WorldUpdater(IUserRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Makes one game step: executes all users code, updates all ships
        /// </summary>
        public void MakeStep()
        {
            foreach (User user in repository.Users)
            {
                if (!user.IsDead)
                {
                    user.ship.makeStep();
                    try
                    {
                        user.engine.Execute(user.Code);
                        if (user.consoleCommand != null)
                        {
                            user.engine.Execute(user.consoleCommand);
                            user.consoleCommand = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        user.Log.Add("Exception while executing your code: " + ex.Message);
                        user.consoleCommand = null;
                    }
                }
            }

            foreach (User user in repository.Users)
            {
                if (user.ship.IsBroken()) user.IsDead = true;
            }
        }
    }
}