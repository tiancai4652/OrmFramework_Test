using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm_Test_Console
{
    public class DataGenerater
    {
        public static List<Student> GetListStudent(int count)
        {
            List<Student> list = new List<Student>();
            for (int i = 0; i < count; i++)
            {
                list.Add(CreatOne());
            }
            return list;
        }

        public static Student CreatOne()
        {
            Student s = new Student();
            int randomNood = new Random(DateTime.Now.Millisecond >> 4).Next(1, 10);
            int randow = new Random(randomNood).Next(0, 255);
            s.Bool = Convert.ToBoolean(randow % 2);
            s.Byte = (byte)randow;
            s.ByteArray = new byte[3] { 1, 2, (byte)randow };
            s.Double = randow / 2;
            s.ID = Guid.NewGuid().ToString("N");
            s.Int = randow;
            s.Long = 11111111111111111;
            s.NullBool = s.Bool ? null : (bool?)false;
            s.NullDateTime = s.Bool ? null : (DateTime?)new DateTime(2019, 02, 02);
            s.NullDouble = s.Bool ? null : (double?)9.5;
            s.NullInt = s.Bool ? null : (int?)9;
            s.String = "dadasdfafaf";
            return s;
        }
    }
}
