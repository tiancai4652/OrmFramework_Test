using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm_Test_Console
{
    public interface IAction
    {
        void Insert(List<Student> list);


        void Delete();


        void FindAll();


        void FindByBoolTrue();


        void FindByIntOver100();


        void FindByIntOver100AndBoolTrue();

    }
}
