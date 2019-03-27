using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm_Test_Console
{
    public class EF: IAction
    {
        public void Insert(List<Student> list)
        {
            using (StudentContext context = new StudentContext("SqlServer_EF"))
            {
                context.Student.AddRange(list);
                context.SaveChanges();
            }
        }

        public void Delete()
        {
            using (StudentContext context = new StudentContext("SqlServer_EF"))
            {
                context.Database.ExecuteSqlCommand("delete from students");
                context.SaveChanges();
            }
        }

        public void FindAll()
        {
            using (StudentContext context = new StudentContext("SqlServer_EF"))
            {
                var list = context.Student.Where(t=>1==1).ToList();
                var count = list.Count();
                Console.WriteLine($"EF.Count={count}");
            }
        }

        public void FindByBoolTrue()
        {
            using (StudentContext context = new StudentContext("SqlServer_EF"))
            {
                var list = context.Student.Where(t => t.Bool.Equals(true));
                var count = list.Count();
                Console.WriteLine($"EF.Count={count}");
            }
        }

        public void FindByIntOver100()
        {
            using (StudentContext context = new StudentContext("SqlServer_EF"))
            {
                var list = context.Student.Where(t => t.Int>100);
                var count = list.Count();
                Console.WriteLine($"EF.Count={count}");
            }
        }

        public void FindByIntOver100AndBoolTrue()
        {
            using (StudentContext context = new StudentContext("SqlServer_EF"))
            {
                var list = context.Student.Where(t => t.Int > 100&& t.Bool.Equals(true));
                var count = list.Count();
                Console.WriteLine($"EF.Count={count}");
            }
        }
    }
}
