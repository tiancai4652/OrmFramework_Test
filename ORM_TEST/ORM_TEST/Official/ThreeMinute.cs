using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SqlSugar_Test.Official
{
    //http://www.codeisbug.com/Doc/8/1166 Test
    public class ThreeMinute
    {
        static Student Jack = new Student() { Id = 1, Name = "Jack" };
        static Student Sam = new Student() { Id = 2, Name = "Sam" };
        static Student Shaco = new Student() { Id = 3, Name = "Shaco" };

        [Trait("Generics", "DataBase_Generics")]
        [Fact]
        public void DataBase_Generics()
        {
            StudentDao a = new StudentDao();
            dynamic[] list = new dynamic[] { 0, 1, 2, 3 };
            try
            {
                bool xx = a.CurrentDb.DeleteByIds(list);
            }
            catch (Exception ex)
            {
                Global.logger.Error(ex);
            }
        
            a.CurrentDb.InsertRange(new Student[] { Jack, Sam, Shaco });
            Assert.Equal(3, a.CurrentDb.GetList().Count);
        }
    }

    public class StudentDao : DbContext<Student>
    {

        //我们如果有特殊需要可以重写DbContext中默认 增、删、查、改、方法

    }

    public class DbContext<T> where T : class, new()
    {
        public DbContext()
        {
            string dbPath = "Student.db";
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = $"Data Source={dbPath}",
                DbType = DbType.Sqlite,//设置数据库类型
                IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
            });
            //调式代码 用来打印SQL 
            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Global.logger.Trace(sql + "\r\n" +
                    Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
            };

        }
        //注意：不能写成静态的
        public SqlSugarClient Db;//用来处理事务多表查询和复杂的操作
        public SimpleClient<Student> StudentDb { get { return new SimpleClient<Student>(Db); } }//用来处理Student表的常用操作
        //public SimpleClient<School> SchoolDb { get { return new SimpleClient<School>(Db); } }//用来处理School表的常用操作
        public SimpleClient<T> CurrentDb { get { return new SimpleClient<T>(Db); } }//用来处理T表的常用操作


        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetList()
        {
            return CurrentDb.GetList();
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Delete(dynamic id)
        {
            return CurrentDb.Delete(id);
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Update(T obj)
        {
            return CurrentDb.Update(obj);
        }

    }
}
