using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm_Test_Console
{
    public class StudentContext : System.Data.Entity.DbContext
    {
        public DbSet<Student> Student { get; set; }

        public StudentContext(string conn) : base(conn)
        {

        }

        public StudentContext() : base()
        {
          
            
        }
    }
}
