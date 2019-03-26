using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_Test_Demo_wpf
{
    public class Authority
    {
        public string ID { get; set; }
        public string AuthorityDescribe { get; set; }
        public virtual List<User> Users {get;set;}

        public Authority(string describe)
        {
            ID = Guid.NewGuid().ToString("N");
            AuthorityDescribe = describe;
        }

        public Authority()
        { }
    }
}
