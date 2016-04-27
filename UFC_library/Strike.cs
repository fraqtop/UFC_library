using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFC_library
{
    public class Strike
    {
        public string name;
        public string place;
        public float k;
        public float probability;
        public float dmg;
        public Strike(string new_name, string new_place, float new_k, float new_pro)
        {
            name = new_name;
            place = new_place;
            k = new_k;
            probability = new_pro;
        }

    }
}
