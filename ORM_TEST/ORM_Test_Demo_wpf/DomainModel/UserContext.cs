using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_Test_Demo_wpf
{

    /// <summary>
    /// 其中User和Authority是多对多关系.
    /// </summary>
    public class UserContext: System.Data.Entity.DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Authority> Authority { get; set; }

        public UserContext(string conn) : base(conn)
        {

        }

        public UserContext() : base()
        {

        }
    }
}
