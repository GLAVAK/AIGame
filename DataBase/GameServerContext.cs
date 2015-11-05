using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class GameServerContext : DbContext
    {
        public DbSet<DBUser> Users { get; set; }
    }
}
