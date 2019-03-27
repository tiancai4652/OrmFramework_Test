using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ORM_Test_Demo_wpf
{
    public class EF
    {

        string DB = "SqlServer";

        [Fact]
        public void xx()
        {
            //UserContext context = new UserContext(DB);
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


        /// <summary>
        /// 每个表的数据都一条一条插入()
        /// </summary>
        /// <param name="groups"></param>
        public void InsertOneByOne(List<Group> groups)
        {
            using (UserContext context = new UserContext(DB))
            {
                foreach (var item in groups)
                {
                    context.Group.Add(item);
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// 每个表一次插入一堆数据
        /// </summary>
        public void InsertDataPerTable(List<Group> groups)
        {
            using (UserContext context = new UserContext(DB))
            {
                context.Group.AddRange(groups);
                context.SaveChanges();
            }
        }

        public void DeleteDataPerTable(List<Group> groups)
        {
            using (UserContext context = new UserContext(DB))
            {
                context.Group.RemoveRange(groups);
                context.SaveChanges();
            }
        }

        public List<Group> FindGroupAll()
        {
            using (UserContext context = new UserContext(DB))
            {
               return context.Group.ToList(); ;
            }
        }

        public List<User> FindUserAll()
        {
            using (UserContext context = new UserContext(DB))
            {
                return context.User.ToList(); ;
            }
        }

        public List<Authority> FindAuthorityAll()
        {
            using (UserContext context = new UserContext(DB))
            {
                return context.Authority.ToList(); ;
            }
        }

        public List<Group> FindGroupByDescribe(string x)
        {
            using (UserContext context = new UserContext(DB))
            {
                return context.Group.Where(t=>t.Describe== x).ToList(); ;
            }
        }

        public List<User> FindUserByName(string x)
        {
            using (UserContext context = new UserContext(DB))
            {
                return context.User.Where(t => t.Name == x).ToList(); ;
            }
        }

        public List<Authority> FindAuthoritiesByName(string x)
        {
            using (UserContext context = new UserContext(DB))
            {
                return context.Authority.Where(t => t.AuthorityDescribe == x).ToList(); ;
            }
        }

        public void DeleteGroupAll()
        {
            using (UserContext context = new UserContext(DB))
            {
                //context.Group.RemoveRange(context.Group.ToList());
                
                context.Database.ExecuteSqlCommand("DELETE FROM Groups");
                context.SaveChanges();
            }
        }

        public void DeleteUserAll()
        {
            using (UserContext context = new UserContext(DB))
            {
                //context.User.RemoveRange(context.User.ToList());
                context.Database.ExecuteSqlCommand("DELETE FROM Users");
                context.SaveChanges();
            }
        }

        public void DeleteAuthorityAll()
        {
            using (UserContext context = new UserContext(DB))
            {
                //context.Authority.RemoveRange(context.Authority.ToList());
                context.Database.ExecuteSqlCommand("DELETE FROM Authorities");
                context.SaveChanges();
            }
        }
    }
}
