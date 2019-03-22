using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SqlSugar_Test.Official
{

    //Base DbContext
    //http://www.codeisbug.com/Doc/8/1165 Test
    //利用数据上下文的增删改查，增加一个事务处理（失败回滚）
    //delete 用的是dynamic
    //Find_Pages 当我没有条件时，那么我如何写？   var listStu = dbcontext.StudentDb.GetPageList(x=>x is Student,pageCondition);


    public class TwoMinutes
    {
        DbContext dbcontext = new DbContext();
        static Student Jack = new Student() { Id = 1, Name = "Jack" };
        static Student Sam = new Student() { Id = 2, Name = "Sam" };
        static Student Shaco = new Student() { Id = 3, Name = "Shaco" };

        [Trait("DbContext", "Insert_One")]
        [Fact]
        public void Insert_One()
        {
            Delete_All();
            dbcontext.StudentDb.Insert(Jack);
            Assert.NotNull(dbcontext.StudentDb.GetById(Jack.Id));
            dbcontext.StudentDb.Insert(Sam);
            Assert.NotNull(dbcontext.StudentDb.GetById(Sam.Id));
            dbcontext.StudentDb.Insert(Shaco);
            Assert.NotNull(dbcontext.StudentDb.GetById(Shaco.Id));
        }

        [Trait("DbContext", "Insert_Many")]
        [Fact]
        public void Insert_Many()
        {
            Delete_All();
            var list =new Student[]{ Jack, Sam, Shaco};
            dbcontext.StudentDb.InsertRange(list);
            Assert.NotNull(dbcontext.StudentDb.GetById(Jack.Id));
            Assert.NotNull(dbcontext.StudentDb.GetById(Sam.Id));
            Assert.NotNull(dbcontext.StudentDb.GetById(Shaco.Id));
        }

        [Trait("DbContext", "Delete_All")]
        [Fact]
        public void Delete_All()
        {
            dynamic[] list = new dynamic[] { 0, 1, 2, 3 };
            try
            {
                bool xx = dbcontext.StudentDb.DeleteByIds(list);
            }
            catch(Exception ex)
            {
                Global.logger.Error(ex);
            }
            dbcontext.StudentDb.Insert(Jack);
            dbcontext.StudentDb.Insert(Sam);
            dbcontext.StudentDb.Insert(Shaco);
           
            bool x = dbcontext.StudentDb.DeleteByIds(list);
            var listStu = dbcontext.StudentDb.GetList();
            Assert.Equal(0, dbcontext.StudentDb.Count(t => !string.IsNullOrEmpty(t.Name)));
        }

        [Trait("DbContext", "Delete_One_ById")]
        [Fact]
        public void Delete_One_ById()
        {
            Delete_All();
            dbcontext.StudentDb.Insert(Jack);
            bool x = dbcontext.StudentDb.DeleteById(Jack.Id);
            Assert.Null(dbcontext.StudentDb.GetById(Jack.Id));
        }

        [Trait("DbContext", "Find_One_ById")]
        [Fact]
        public void Find_One_ById()
        {
            Delete_All();
            dbcontext.StudentDb.Insert(Jack);
            Assert.Equal(Jack.Id, dbcontext.StudentDb.GetById(Jack.Id).Id);
            Assert.Equal(Jack.Name, dbcontext.StudentDb.GetById(Jack.Id).Name);
        }

        [Trait("DbContext", "Find_All")]
        [Fact]
        public void Find_All()
        {
            Delete_All();
            var list = new Student[] { Jack, Sam, Shaco };
            dbcontext.StudentDb.InsertRange(list);
            Assert.Equal(3, dbcontext.StudentDb.GetList().Count);
        }

        [Trait("DbContext", "Find_WithPages_WithCondition_WithAsc")]
        [Fact]
        public void Find_WithPages_WithCondition_WithAsc()
        {
            Delete_All();
            var list = new Student[] { Jack, Sam, Shaco };
            dbcontext.StudentDb.InsertRange(list);

            var pageCondition = new PageModel() { PageIndex = 1, PageSize = 2 };
            var listStu = dbcontext.StudentDb.GetPageList(x=> true,pageCondition,null,OrderByType.Asc);
            Assert.Equal(2, listStu.Count);
            Assert.Equal(Jack.Name, listStu[0].Name);
            Assert.Equal(Sam.Name, listStu[1].Name);
        }

        [Trait("DbContext", "Find_WithPages_WithCondition_WithAsc2")]
        [Fact]
        public void Find_WithPages_WithCondition_WithAsc2()
        {
            Delete_All();
            var list = new Student[] { Jack, Sam, Shaco };
            dbcontext.StudentDb.InsertRange(list);

            var pageCondition = new PageModel() { PageIndex = 1, PageSize = 1 };
            List<IConditionalModel> conModels = new List<IConditionalModel>();
            conModels.Add(new ConditionalModel() { FieldName = "Id", ConditionalType = ConditionalType.Equal, FieldValue = "1" });//id=1
            var listStu = dbcontext.StudentDb.GetPageList(conModels, pageCondition, it => it.Name, OrderByType.Asc);
            Assert.Single(listStu);
            Assert.Equal(Jack.Name, listStu[0].Name);
        }

        [Trait("DbContext", "Update_One")]
        [Fact]
        public void Update_One()
        {
            Delete_All();
            dbcontext.StudentDb.Insert(Jack);
            var student = new Student() { Id = 1, Name = "HHHH" };
            dbcontext.StudentDb.Update(student);
            Assert.Equal("HHHH", dbcontext.StudentDb.GetById(Jack.Id).Name);
        }

        [Trait("DbContext", "Update_Many")]
        [Fact]
        public void Update_Many()
        {
            Delete_All();
            dbcontext.StudentDb.Insert(Jack);
            dbcontext.StudentDb.Insert(Sam);
            var student = new Student() { Id = 1, Name = "HHHH" };
            var student2 = new Student() { Id = 2, Name = "MMMM" };
            dbcontext.StudentDb.UpdateRange(new Student[]{ student, student2});
            Assert.Equal("HHHH", dbcontext.StudentDb.GetById(Jack.Id).Name);
            Assert.Equal("MMMM", dbcontext.StudentDb.GetById(Sam.Id).Name);
        }

        [Trait("DbContext", "Find_AsQueryable")]
        [Fact]
        public void Find_AsQueryable()
        {

        }

        [Trait("DbContext", "Insert_AsInsertable")]
        [Fact]
        public void Insert_AsInsertable()
        {

        }

        [Trait("DbContext", "Delete_AsDeleteable")]
        [Fact]
        public void Delete_AsDeleteable()
        {

        }

        [Trait("DbContext", "Use_Trans")]
        [Fact]
        public void Use_Trans_True()
        {
            Delete_All();
            var result = dbcontext.Db.Ado.UseTran(() =>
            {
                //这里写你的逻辑
                var list = new Student[] { Jack, Sam, Shaco };
                dbcontext.StudentDb.InsertRange(list);

            });
            if (result.IsSuccess)
            {
                Assert.NotNull(dbcontext.StudentDb.GetById(Jack.Id));
                Assert.NotNull(dbcontext.StudentDb.GetById(Sam.Id));
                Assert.NotNull(dbcontext.StudentDb.GetById(Shaco.Id));
            }
            else
            {
                Global.logger.Error(result.ErrorMessage);
            }

          
        }

        [Trait("DbContext", "Use_Trans")]
        [Fact]
        public void Use_Trans_False()
        {
            Delete_All();
            var result = dbcontext.Db.Ado.UseTran(() =>
            {
                Student x = new Student() { Id = 1, Name = "DXW" };
                //这里写你的逻辑
                var list = new Student[] { Jack, Sam, Shaco };
                dbcontext.StudentDb.InsertRange(list);
                dbcontext.StudentDb.Insert(x);

            });
            if (result.IsSuccess)
            {
                Assert.ThrowsAsync<Exception>(() => throw new Exception("不应该走到这里"));
             
            }
            else
            {
                Assert.Null(dbcontext.StudentDb.GetById(Jack.Id));
                Assert.Null(dbcontext.StudentDb.GetById(Sam.Id));
                Assert.Null(dbcontext.StudentDb.GetById(Shaco.Id));
                Global.logger.Error(result.ErrorMessage);
            }
        }

    }


    public class DbContext
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
        //注意：不能写成静态的，不能写成静态的
        public SqlSugarClient Db;//用来处理事务多表查询和复杂的操作
        public SimpleClient<Student> StudentDb { get { return new SimpleClient<Student>(Db); } }//用来处理Student表的常用操作
        //public SimpleClient<School> SchoolDb { get { return new SimpleClient<School>(Db); } }//用来处理School表的常用操作
    }

}
