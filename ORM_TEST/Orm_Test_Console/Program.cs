using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orm_Test_Console
{
    public class Program
    {
        static ILogger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {

            int count = 100;
            do {
                Thread.Sleep(50);
                Optput("Please set the data count...."); }
            while(!int.TryParse( Console.ReadLine(),out count));

            var ef = new EF();
            var dapper = new Dapper();
            var list = DataGenerater.GetListStudent(count);

            //insert
            //DateTime d1 = DateTime.Now;
            //ef.Insert(list);
            //DateTime d2 = DateTime.Now;
            //dapper.Insert(list);
            //DateTime d3 = DateTime.Now;
            //Optput(
            //    "Insert:" + count + Environment.NewLine +
            //    "EF:" + $"{(d2 - d1).TotalMilliseconds}" + "ms" + Environment.NewLine +
            //    "Dapper:" + $"{(d3 - d2).TotalMilliseconds}" + "ms" + Environment.NewLine
            //    );

            InsertMethodDelayConsole(ef.Insert, dapper.Insert, "Insert", count);
            MethodDelayConsole(ef.FindAll, dapper.FindAll, "FFindAll");
            MethodDelayConsole(ef.FindByBoolTrue, dapper.FindByBoolTrue, "FindByBoolTrue");
            MethodDelayConsole(ef.FindByIntOver100, dapper.FindByIntOver100, "FindByIntOver100");
            MethodDelayConsole(ef.FindByIntOver100AndBoolTrue, dapper.FindByIntOver100AndBoolTrue, "FindByIntOver100AndBoolTrue");
            MethodDelayConsole(ef.Delete, dapper.Delete, "DeleteAll");
            Optput(Environment.NewLine+"------------------------------------------");
            Console.ReadKey();

         
        }


        //利用EF特性生成数据库
        static void Creat()
        {
            using (StudentContext context = new StudentContext("SqlServer_EF"))
            {
                Student a = DataGenerater.CreatOne();
                context.Student.Add(a);
                context.SaveChanges();
                var x= context.Student.Find(a.ID);
                context.Student.Remove(a);
                context.SaveChanges();
            }

            using (StudentContext context = new StudentContext("SqlServer_Dapper"))
            {
                Student a = DataGenerater.CreatOne();
                context.Student.Add(a);
                context.SaveChanges();
                var x = context.Student.Find(a.ID);
                context.Student.Remove(a);
                context.SaveChanges();
            }
        }

        static void InsertMethodDelayConsole(Action<List<Student>> ef,Action<List<Student>> dapper,string operat,int count)
        {
            var list = DataGenerater.GetListStudent(count);
            DateTime d1 = DateTime.Now;
            ef.Invoke(list);
            DateTime d2 = DateTime.Now;
            dapper.Invoke(list);
            DateTime d3 = DateTime.Now;

            Optput(
                $"{operat}:" + count + Environment.NewLine +
                "EF:" + $"{(d2 - d1).TotalMilliseconds}" + "ms" + Environment.NewLine +
                "Dapper:" + $"{(d3 - d2).TotalMilliseconds}" + "ms" + Environment.NewLine
                );

        }

        static void MethodDelayConsole(Action ef, Action dapper, string operat)
        {
          
            DateTime d1 = DateTime.Now;
            ef.Invoke();
            DateTime d2 = DateTime.Now;
            dapper.Invoke();
            DateTime d3 = DateTime.Now;
            Optput(
            $"{operat}:" + Environment.NewLine +
                "EF:" + $"{(d2 - d1).TotalMilliseconds}" + "ms" + Environment.NewLine +
                "Dapper:" + $"{(d3 - d2).TotalMilliseconds}" + "ms" + Environment.NewLine
                );

        }

        static void Optput(string s)
        {
            Console.WriteLine(s);
            logger.Trace(s);
        }

    }
}
