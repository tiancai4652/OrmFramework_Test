using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dapper_Test.Official
{
    //https://github.com/StackExchange/Dapper
    //Helper/Attribute

    class Dapper_Helper
    {

        /// <summary>
        /// 	• 执行查询并将结果映射到强类型中
        /// </summary>
        public void Helper1()
        {
            var guid = Guid.NewGuid();
            var dog = connection.Query<Dog>("select Age = @Age, Id = @Id", new { Age = (int?)null, Id = guid });

            Assert.Equal(1, dog.Count());
            Assert.Null(dog.First().Age);
            Assert.Equal(guid, dog.First().Id);
        }

        /// <summary>
        /// 	• 执行查询并将结果映射到动态对象(DynamicObject)中
        /// </summary>
        public void Helper2()
        {
            var rows = connection.Query("select 1 A, 2 B union all select 3, 4");

            Assert.Equal(1, (int)rows[0].A);
            Assert.Equal(2, (int)rows[0].B);
            Assert.Equal(3, (int)rows[1].A);
            Assert.Equal(4, (int)rows[1].B);
        }

        /// <summary>
        /// 	• 执行一条没有返回值的命令
        /// </summary>
        public void Helper3()
        {
            var count = connection.Execute(@"
  set nocount on
  create table #t(i int)
  set nocount off
  insert #t
  select @a a union all select @b
  set nocount on
  drop table #t", new { a = 1, b = 2 });
            Assert.Equal(2, count);
        }

        /// <summary>
        /// 多次执行同一个命令
        /// </summary>
        public void Helper4()
        {
            var count = connection.Execute(@"insert MyTable(colA, colB) values (@a, @b)",
      new[] { new { a = 1, b = 1 }, new { a = 2, b = 2 }, new { a = 3, b = 3 } }
    );
            Assert.Equal(3, count); // 3 rows inserted: "1,1", "2,2" and "3,3"
        }

        public void MultiMapping()
        {
            var sql =@"select * from #Posts pleft join #Users u on u.Id = p.OwnerIdOrder by p.Id";
            var data = connection.Query<Post, User, Post>(sql, (post, user) => { post.Owner = user; return post; });
            var post = data.First();
            Assert.Equal("Sams Post1", post.Content);
            Assert.Equal(1, post.Id);
            Assert.Equal("Sam", post.Owner.Name);
            Assert.Equal(99, post.Owner.Id);
        }

        public void MultiResult()
        {
            var sql =
@"
select * from Customers where CustomerId = @id
select * from Orders where CustomerId = @id
select * from Returns where CustomerId = @id";

            using (var multi = connection.QueryMultiple(sql, new { id = selectedId }))
            {
                var customer = multi.Read<Customer>().Single();
                var orders = multi.Read<Order>().ToList();
                var returns = multi.Read<Return>().ToList();

            }
        }

        public void StoredProcedures()
        {
            var user = cnn.Query<User>("spGetUser", new { Id = 1 },
        commandType: CommandType.StoredProcedure).SingleOrDefault();

            var p = new DynamicParameters();
            p.Add("@a", 11);
            p.Add("@b", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@c", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            cnn.Execute("spMagicProc", p, commandType: CommandType.StoredProcedure);

            int b = p.Get<int>("@b");
            int c = p.Get<int>("@c");
        }


    }



    public class Dog
    {
        public int? Age { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float? Weight { get; set; }

        public int IgnoredProperty { get { return 1; } }
    }

    class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public User Owner { get; set; }
    }

    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


}
