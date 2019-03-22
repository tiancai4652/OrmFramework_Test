using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace SqlSugar_Test.Official
{
    //http://www.codeisbug.com/Doc/8/1163 Test
    //需要Nuget Sqlite.Dll
    //需要Newtoon.Json(SqlSugar 部分功能用到Newtonsoft.Json.dll，需要在Nuget上安装 Newtonsoft.Json 9.0.0.1及以上版本)
    //实体没有主键时不能自己添加实体主键
    //分页查询时从0开始查和从1开始查是一样的，请看Find_Pages方法
    //已启动输出SQL日志，SQL日志放在Debug/log.txt文件中


    public class OneMinute
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
                    logger.Trace(sql + "\r\n" +
                    db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                };
                return client;
            }
        }
        /// <summary>
        /// 日志
        /// </summary>
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        [Trait("Method","Inisert_One")]
        [Fact]
        public void Inisert_One()
        {
            Ini();
            var data0 = new StudentModel() { Id = 0, Name = "jack" };
            int x = db.Insertable(data0).ExecuteCommand();
            var list = db.Queryable<StudentModel>().ToList();//查询所有
            var getById = db.Queryable<StudentModel>().InSingle(0);//根据主键查询
            Assert.Equal(data0.Name, getById.Name);
        }

        [Trait("Method", "Update_OneProperty")]
        [Fact]
        public void Update_OneProperty()
        {
            Ini();
            var data0 = new StudentModel() { Id = 0, Name = "jack" };
            int x = db.Insertable(data0).ExecuteCommand();
            var data01 = new StudentModel() { Id = 0, Name = "Jack" };
            db.Updateable(data01).ExecuteCommand();
            var getById = db.Queryable<StudentModel>().InSingle(0);//根据主键查询
            Assert.Equal(getById.Name, data01.Name);
        }

        [Trait("Method", "Find_AllCount")]
        [Fact]
        public void Find_AllCount()
        {
            Ini();
            var data1 = new StudentModel() { Id = 1, Name = "Same" };
            var data2 = new StudentModel() { Id = 2, Name = "Shaco" };
            db.Insertable(data1).ExecuteCommand();
            db.Insertable(data2).ExecuteCommand();
            Assert.Equal(2, db.Queryable<StudentModel>().Count());
        }

        [Trait("Method", "Find_All")]
        [Fact]
        public void Find_All()
        {
            Ini();
            var data1 = new StudentModel() { Id = 1, Name = "Same" };
            var data2 = new StudentModel() { Id = 2, Name = "Shaco" };
            db.Insertable(data1).ExecuteCommand();
            db.Insertable(data2).ExecuteCommand();
            Assert.Contains(data1.Name,db.Queryable<StudentModel>().Select(t=>t.Name).ToList());
            Assert.Contains(data2.Name,db.Queryable<StudentModel>().Select(t => t.Name).ToList() );
        }

        [Trait("Method", "Find_One")]
        [Fact]
        public void Find_One()
        {
            Ini();
            var data1 = new StudentModel() { Id = 1, Name = "Same" };
            db.Insertable(data1).ExecuteCommand();
            var getById = db.Queryable<StudentModel>().InSingle(1);//根据主键查询
            Assert.Equal("Same", getById.Name);
        }

        [Trait("Method", "Find_WithCondition")]
        [Fact]
        public void Find_WithCondition()
        {
            Ini();
            var data1 = new StudentModel() { Id = 1, Name = "Same" };
            db.Insertable(data1).ExecuteCommand();
            var getByWhere = db.Queryable<StudentModel>().Where(it => it.Id == 1).ToList();//根据条件查询
            Assert.Equal("Same", getByWhere[0].Name);

        }

        [Trait("Method", "Find_Pages")]
        [Fact]
        public void Find_Pages()
        {
            Ini();
            var data0 = new StudentModel() { Id = 0, Name = "jack" };
            var data1 = new StudentModel() { Id = 1, Name = "Same" };
            var data2 = new StudentModel() { Id = 2, Name = "Shaco" };
            int x = db.Insertable(data0).ExecuteCommand();
            db.Insertable(data1).ExecuteCommand();
            db.Insertable(data2).ExecuteCommand();

            var total = 0;
            var getPage = db.Queryable<StudentModel>().ToPageList(0, 1, ref total);//根据分页查询
            Assert.Equal("jack", getPage[0].Name);
            getPage = db.Queryable<StudentModel>().ToPageList(1, 1, ref total);//根据分页查询
            Assert.Equal("jack", getPage[0].Name);
            getPage = db.Queryable<StudentModel>().ToPageList(2, 1, ref total);//根据分页查询
            Assert.Equal("Same", getPage[0].Name);
            getPage = db.Queryable<StudentModel>().ToPageList(3, 1, ref total);//根据分页查询
            Assert.Equal("Shaco", getPage[0].Name);
        }

        [Trait("Method", "Delete_ByPK")]
        [Fact]
        public void Delete_ByPK()
        {
            Ini();
            var data1 = new StudentModel() { Id = 1, Name = "Same" };
            db.Insertable(data1).ExecuteCommand();
            var getById = db.Queryable<StudentModel>().InSingle(1);//根据主键查询
            Assert.Equal("Same", getById.Name);
            db.Deleteable<StudentModel>(1).ExecuteCommand();
            Assert.Null(db.Queryable<StudentModel>().InSingle(1));
        }

        void Ini()
        {
            
            try
            {
                db.Deleteable<StudentModel>(0).ExecuteCommand();
                db.Deleteable<StudentModel>(1).ExecuteCommand();
                db.Deleteable<StudentModel>(2).ExecuteCommand();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }




    //如果实体类名称和表名不一致可以加上SugarTable特性指定表名
    [SugarTable("Student")]
    public class StudentModel
    {
        //指定主键和自增列，当然数据库中也要设置主键和自增列才会有效
        [SugarColumn(IsPrimaryKey = true)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
