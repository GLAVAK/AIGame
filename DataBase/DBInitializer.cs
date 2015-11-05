using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class DBInitializer : DropCreateDatabaseIfModelChanges<GameServerContext>
    {
        protected override void Seed(GameServerContext context)
        {
            var Users = new List<DBUser>() { 
                new DBUser() { Username = "Glavak" }
            };
            Users.ForEach(s => context.Users.Add(s));
            context.SaveChanges();
        }
    }
}
