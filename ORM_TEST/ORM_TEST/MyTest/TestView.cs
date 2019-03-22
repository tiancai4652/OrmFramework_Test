using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SqlSugar_Test.Official
{
    /// <summary>
    /// 测试一下视图
    /// </summary>
   public class TestView
    {
        /// <summary>
        /// 测试数据库
        /// </summary>
        static string dbPath = "Student.db";
        /// <summary>
        /// 连接实例
        /// </summary>
        public static SqlSugarClient db
        {
            get
            {
                var client = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = $"Data Source={dbPath}",
                    DbType = DbType.Sqlite,//设置数据库类型
                    IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                    InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
                });
                client.Ado.IsEnableLogEvent = true;
                //用来打印Sql方便你调式    
                client.Ado.LogEventStarting = (sql, pars) =>
                {
                    Global.logger.Trace(sql + "\r\n" +
                    db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                };
                return client;
            }
        }

        [Fact]
        public void xxx()
        {
            //Ini();
            //var data0 = new Student() { Id = 0, Name = "jack" };
            //int x = db.Insertable(data0).ExecuteCommand();
            var list = db.Queryable<StudentSchool>().ToList();//查询所有
            Assert.Single(list);
        }

        void Ini()
        {

            try
            {
                db.Deleteable<Student>(0).ExecuteCommand();
                db.Deleteable<Student>(1).ExecuteCommand();
                db.Deleteable<Student>(2).ExecuteCommand();
                db.Deleteable<Student>(3).ExecuteCommand();
            }
            catch (Exception ex)
            {
                Global.logger.Error(ex);
            }
        }



    }

    //如果实体类名称和表名不一致可以加上SugarTable特性指定表名
    [SugarTable("SC")]
    public class StudentSchool
    {
        //指定主键和自增列，当然数据库中也要设置主键和自增列才会有效
        [SugarColumn(IsPrimaryKey = true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string School_Name { get; set; }
    }
}
