using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ORM_Test_Demo_wpf
{
    //测试项目
    //增删改查-表
    //增删改查-视图
    //第一次载入
    //EF/Dapper 缓存机制开启与否
    //分页查询
    //特性-映射
    //同步/异步
    public class Dapper : IDisposable
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

        #region MysqlConn
        protected MySqlConnection _Mysqlconnection;
        protected MySqlConnection Mysqlconnection => _Mysqlconnection ?? (_Mysqlconnection = GetMysqlOpenConnection());
        public static MySqlConnection GetMysqlOpenConnection()
        {
            var cs = MysqlConnectionString;
          
            var connection = new MySqlConnection(cs);
            connection.Open();
            return connection;
        }
        public static string MysqlConnectionString => @"Data Source=ZR644\ACALSQLEXPRESS;Persist Security Info=True;User ID=sa;Password=ACal@Server123456";
        #endregion

        #region SqliteConn
        protected SQLiteConnection _Sqliteconnection;
        protected SQLiteConnection Sqliteconnection => _Sqliteconnection ?? (_Sqliteconnection = GetSqliteOpenConnection());
        public static SQLiteConnection GetSqliteOpenConnection()
        {
            var cs = SqliteConnectionString;
            var connection = new SQLiteConnection(cs);
            connection.Open();
            return connection;
        }
        public static string SqliteConnectionString => @"Data Source=ZR644\ACALSQLEXPRESS;Persist Security Info=True;User ID=sa;Password=ACal@Server123456";

        #endregion

        public void Dispose()
        {
            _Connection?.Dispose();
            _Mysqlconnection?.Dispose();
        }

        #region Insert

        void InsertGroupOne(Group group)
        {
            if (group.Users != null)
            {
                var Users = group.Users;
                foreach (var User in Users)
                {
                    InsertUserOne(User);
                }
            }
            ThisConn.Execute("insert Groups(ID,Describe values(@ID,@Describe))", group);
        }

        void InsertUserOne(User user)
        {
            if (user.Authorities != null)
            {
                foreach (var item in user.Authorities)
                {
                    InsertAuthorityOne(item);
                }
            }
            //int tally= ThisConn.Execute("",)
            ThisConn.Execute("insert Users (ID,Name,Password,Age,NickName) values(@ID,@Name,@Password,@Age,@NickName)", user);
        }

        void InsertAuthorityOne(Authority authority)
        {
            int tally = ThisConn.Execute("insert Authorities (ID,AuthorityDescribe) values(@ID, @AuthorityDescribe)", new List<Authority>
            {
               authority
            });
        }

        void InsertAuthorityManyAlone(List<Authority> authorities)
        {
            int tally = ThisConn.Execute("insert Authorities (ID,AuthorityDescribe) values(@ID, @AuthorityDescribe)", authorities);
        }

        void InsertUserManyAlone(List<User> users)
        {
            ThisConn.Execute("insert Users (ID,Name,Password,Age,NickName) values(@ID,@Name,@Password,@Age,@NickName)", users);
        }

        void InsertGroupManyAlone(List<Group> groups)
        {
            ThisConn.Execute("insert Groups(ID,Describe) values(@ID,@Describe)", groups);
        }

        /// <summary>
        /// 每个表的数据都一条一条插入()
        /// </summary>
        /// <param name="groups"></param>
        public void InsertOneByOne(List<Group> groups)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                InsertGroupOne(groups[i]);
            }
        }

        /// <summary>
        /// 每个表一次插入一堆数据
        /// </summary>
        public void InsertDataPerTable(List<Group> groups)
        {
            var usersList = groups.Select(t => t.Users).ToList();
            var users = new List<User>();
            usersList.ForEach(t => users.AddRange(t));
            var atsList = users.Select(t => t.Authorities).ToList();
            var ats = new List<Authority>();
            atsList.ForEach(t => ats.AddRange(t));
            InsertAuthorityManyAlone(ats);
            InsertGroupManyAlone(groups);
            InsertUserManyAlone(users);

        }

        #endregion

        #region delete

        void DeleteGroupAlone(List<Group> groups)
        {
            ThisConn.Execute("delete from Groups where id=@ID", groups);
        }

        void DeleteUserAlone(List<User> users)
        {
            ThisConn.Execute("delete from Users where id=@ID", users);
        }

        void DeleteAuthorityAlone(List<Authority> ats)
        {
            ThisConn.Execute("delete from Authorities where id=@ID", ats);
        }

        public void DeleteDataPerTable(List<Group> groups)
        {
            var usersList = groups.Select(t => t.Users).ToList();
            var users = new List<User>();
            usersList.ForEach(t => users.AddRange(t));
            var atsList = users.Select(t => t.Authorities).ToList();
            var ats = new List<Authority>();
            atsList.ForEach(t => ats.AddRange(t));
            DeleteAuthorityAlone(ats);
            DeleteGroupAlone(groups);
            DeleteUserAlone(users);
        }

        public void DeleteGroupAll()
        {
             ThisConn.Execute("DELETE FROM Groups");
        }

        public void DeleteUserAll()
        {
            ThisConn.Execute("DELETE FROM Users");
        }

        public void DeleteAuthorityAll()
        {
            ThisConn.Execute("DELETE FROM Authorities");
        }

        #endregion

        #region Find

        public List<Group> FindGroupAll()
        {
            return ThisConn.Query<Group>("select * from Groups").ToList();
        }

        public List<User> FindUserAll()
        {
            return ThisConn.Query<User>("select * from Users").ToList();
        }

        public List<Authority> FindAuthorityAll()
        {
            return ThisConn.Query<Authority>("select * from Authorities").ToList();
        }

        public List<Group> FindGroupByDescribe(string x)
        {
            return ThisConn.Query<Group>("select * from Group where Describe=@Describe",new{ Describe=x }).ToList();
        }

        public List<User> FindUserByName(string x)
        {
            return ThisConn.Query<User>("select * from Users where Name=@Name", new { Name=x }).ToList();
        }

        public List<Authority> FindAuthoritiesByName(string x)
        {
            return ThisConn.Query<Authority>("select * from Authorities where AuthorityDescribe=@AuthorityDescribe", new { AuthorityDescribe=x }).ToList();
        }

        #endregion

        #region Fact

        [Fact]
        public void xxx()
        {
            InsertAuthorityOne(DataGenerater.GetAuthorityOne());
        }




        #endregion
    }
}
