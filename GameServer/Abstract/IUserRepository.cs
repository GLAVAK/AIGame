using DataBase;
using GameServer.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Abstract
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }

        void UpdateFromDB(GameServerContext context);
    }
}
