using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_Test_Demo_wpf
{
    public class User
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public string NickName { get; set; }

        public virtual List<Authority> Authorities { get; set; }

        public User(string name, string pw, List<Authority> authorities,string nickname="No Nickname", int age = 10)
        {
            ID = Guid.NewGuid().ToString("N");
            Name = name;
            Password = pw;
            Age = age;
            NickName = nickname;
            Authorities = authorities;
        }

        public User()
        { }
    }
}
