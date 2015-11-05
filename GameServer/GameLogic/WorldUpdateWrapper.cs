using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace GameServer.GameLogic
{
    public class WorldUpdateWrapper
    {
        static void WorldUpdate(object updater)
        {
            //Infinite loop for every-second updates
            while (true)
            {
                //TODO: meassure step time
                Thread.Sleep(1000);

                ((WorldUpdater)updater).MakeStep();
            }
        }

        /// <summary>
        /// Launches thread, that updates the world (makes step) every second
        /// </summary>
        /// <param name="container"></param>
        public static void StartWorldUpdate(UnityContainer container)
        {
            Thread updateThread = new Thread(WorldUpdateWrapper.WorldUpdate);

            updateThread.Start(container.Resolve<WorldUpdater>());
        }
    }
}