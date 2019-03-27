using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm_Test_Console
{
    public class Dapper: IAction
    {
        protected DbConnection ThisConn => Connection;

        #region SqlServerConn
        protected SqlConnection _Connection;
        protected SqlConnection Connection => _Connection ?? (_Connection = GetOpenConnection());
        public static SqlConnection GetOpenConnection(bool mars = false)
        {
            var cs = ConnectionString;
            if (mars)
            {
                var scsb = new SqlConnectionStringBuilder(cs)
                {
                    MultipleActiveResultSets = true
                };
                cs = scsb.ConnectionString;
            }
            var connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }

        public static string ConnectionString => @"Data Source=ZR644\ACALSQLEXPRESS;Initial Catalog=ACalLog;Persist Security Info=True;User ID=sa;Password=ACal@Server123456";
        #endregion


        public void Insert(List<Student> list)
        {
            string sql = @"INSERT INTO  Students  ( ID , String , Int , Double , DateTime , Byte , ByteArray , Long , Bool , NullDateTime , NullDouble , NullBool , NullInt ) 
VALUES(@ID, @String, @Int, @Double, @DateTime, @Byte, @ByteArray, @Long, @Bool, @NullDateTime, @NullDouble, @NullBool, @NullInt);
            ";
            ThisConn.Execute("INSERT INTO [Students] ([ID],[String],[Int],[Double],[DateTime],[Byte],[ByteArray],[Long],[Bool],[NullDateTime],[NullDouble],[NullBool],[NullInt]) VALUES(@ID, @String, @Int, @Double, @DateTime, @Byte, @ByteArray, @Long, @Bool, @NullDateTime, @NullDouble, @NullBool, @NullInt)", list);
        }

        public void Delete()
        {
            ThisConn.Execute("delete from students");
        }

        public void FindAll()
        {
            var list= ThisConn.Query<Student>("select * from students");
            var count = list.Count();
            Console.WriteLine($"Dapper.Count={count}");
        }

        public void FindByBoolTrue()
        {
            var list = ThisConn.Query<Student>("select * from students where Bool=@Bool", new { Bool = true });
            var count = list.Count();
            Console.WriteLine($"Dapper.Count={count}");
        }

        public void FindByIntOver100()
        {
            var list = ThisConn.Query<Student>("select * from students where Int>@Int", new { Int = 7 });
            var count = list.Count();
            Console.WriteLine($"Dapper.Count={count}");
        }

        public void FindByIntOver100AndBoolTrue()
        {
            List<object> listPara = new List<object>() { true, 100 };
            var list = ThisConn.Query<Student>("select * from students where Bool=@Bool and Int>=@Int", new { Bool = true, Int= 100 });
            var count = list.Count();
            Console.WriteLine($"Dapper.Count={count}");
        }

    }
}
