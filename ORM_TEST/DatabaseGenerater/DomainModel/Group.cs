using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseGenerater.DomainModel
{
    public class Group
    {
        public string ID { get; set; }
        public string Describe { get; set; }
        public virtual List<User> Users { get; set; }

        public Group(List<User> users,string describe)
        {
            ID = Guid.NewGuid().ToString("N");
            Describe = describe;
            Users = users;
        }

        public Group()
        { }
    }
}
