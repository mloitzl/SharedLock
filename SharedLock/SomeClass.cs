using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLock
{
    internal class SomeClass
    {
        internal  static object Lock = new object();
    }
}
