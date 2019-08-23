using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perso
{
    public class Statistic
    {
        public string name;
        public int? min;
        public int? max;
        public int? value;

        public Statistic()
        {
            name = "";
            min =null;
            max = null;
            value = null;
        }

    }
}
