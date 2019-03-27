using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm_Test_Console
{
    public class Student
    {
        public string ID { get; set; }
        public string String { get; set; }
        public int Int { get; set; }
        public double Double { get; set; }
        public DateTime DateTime { get; set;}
        public byte Byte { get; set; }
        public byte[] ByteArray { get; set; }
        public long Long { get; set; }
        public bool Bool { get; set; }
        //List<int> ListInt { get; set; }
        //List<string> ListString { get; set; }
        //List<double> ListDouble { get; set; }
        ////List<DateTime> ListDateTime { get; set; }
        //List<long> ListLong { get; set; }
        //public List<bool> ListBool { get; set; }
        public Nullable<System.DateTime> NullDateTime { get; set; }
        public Nullable<double> NullDouble { get; set; }
        public Nullable<bool> NullBool { get; set; }
        public Nullable<int> NullInt { get; set; }

        public Student()
        {
            DateTime = DateTime.Now;
        }

    }
}
