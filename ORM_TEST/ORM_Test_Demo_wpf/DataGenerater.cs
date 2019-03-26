using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_Test_Demo_wpf
{
    public static class DataGenerater
    {
        public static Authority GetAuthorityOne()
        {
            Authority a = new Authority(Guid.NewGuid().ToString("N"));
            return a;
        }

        public static List<Authority> GetAuthorityList(int count)
        {
            List<Authority> result = new List<Authority>();
            for (int i = 0; i < count; i++)
            {
                result.Add(GetAuthorityOne());
            }
            return result;
        }

        public static User GetUserOne()
        {
            int AuthorityCount = new Random(DateTime.Now.Millisecond >>4).Next(1, 10);
            List<User> us = new List<User>();
            string name = "name"+ AuthorityCount+Guid.NewGuid().ToString();
            string pw = Guid.NewGuid().ToString();
            string nn = Guid.NewGuid().ToString();
            User a = new User(name, pw, GetAuthorityList(AuthorityCount), nn, AuthorityCount);
            return a;
        }

        public static List<User> GetUserList(int count)
        {
            List<User> result = new List<User>();
            for (int i = 0; i < count; i++)
            {
                result.Add(GetUserOne());
            }
            return result;
        }

        public static Group GetGroupOne()
        {
            int AuthorityCount = new Random(DateTime.Now.Millisecond >>4).Next(1, 10);
            string d ="describe"+ AuthorityCount+Guid.NewGuid().ToString("N");
            Group a = new Group(GetUserList(AuthorityCount), d);
            return a;
        }

        public static List<Group> GetGroupList(int Count)
        {
            List<Group> list = new List<Group>();
            for (int i = 0; i < Count; i++)
            {
                list.Add(GetGroupOne());
            }
            return list;
        }
    }
}
