using DatabaseGenerater.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseGenerater
{
    class Program
    {
        // 通过EF_Code First创建数据库表结构
        //只能创建Mysql和Sqlserver表
        //Sqlite不支持自动创建
        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            //UserContext context = new UserContext("MySQL");
            //UserContext context = new UserContext("Sqlite");
            UserContext context = new UserContext("SqlServer");

            Authority auth1 = new Authority("Level 1");
            Authority auth2 = new Authority("Level 2");
            Authority auth3 = new Authority("Level 3");
            Authority auth4 = new Authority("Level 4");
            Authority auth5 = new Authority("Level 5");
            Authority auth6 = new Authority("Level 6");
            User a = new User("ZR", "644", new List<Authority>() { auth1, auth2, auth3 });
            User b = new User("DXW", "DXW", new List<Authority> { auth4 });
            User c = new User("LQ", "LQ", new List<Authority> { auth5 });
            User d = new User("FHJ", "FHJ", new List<Authority> { auth6 });
            User e = new User("LWJ", "LWJ", new List<Authority> { auth1 });
            Group A = new Group(new List<User>() { a, b, c }, "abc");
            Group B = new Group(new List<User>() { c, d, e }, "cde");

          
            context.Group.Add(A);
            context.Group.Add(B);
            context.User.Add(a);
            context.User.Add(b);
            context.User.Add(c);
            context.User.Add(d);
            context.User.Add(e);
            context.Authority.Add(auth1);
            context.Authority.Add(auth2);
            context.Authority.Add(auth3);
            context.Authority.Add(auth4);
            context.Authority.Add(auth5);
            context.Authority.Add(auth6);
            context.SaveChanges();


            context.Group.Remove(A);
            context.Group.Remove(B);
            context.User.Remove(a);
            context.User.Remove(b);
            context.User.Remove(c);
            context.User.Remove(d);
            context.User.Remove(e);
            context.Authority.Remove(auth1);
            context.Authority.Remove(auth2);
            context.Authority.Remove(auth3);
            context.Authority.Remove(auth4);
            context.Authority.Remove(auth5);
            context.Authority.Remove(auth6);
            context.SaveChanges();

        }
    }
}
