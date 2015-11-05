using DataBase;
using GameServer.Abstract;
using GameServer.Concrete;
using Microsoft.Practices.Unity;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GameServer.Infrastructure
{
    public class ContainerBootstrapper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            GameServerContext gameServerContext = new GameServerContext();

            container.RegisterInstance(
                (IUserRepository)new MemoryUserRepository(gameServerContext));
            container.RegisterInstance(gameServerContext);
            container.RegisterType<SecurityHelper, SecurityHelper>();
        }
    }
}