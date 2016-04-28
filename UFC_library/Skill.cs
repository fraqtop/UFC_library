using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFC_library
{
    public class Skill
    {
        public string who_is; // Кто юзает приём
        public string name; // Название приёма
        public string place; // Часть тела, на которую направлен приём
        public float k; // Коэф удара, чем больше, тем больше итоговый урон
        public float probability; // Вероятность нанесения именно этого удара
        public float dmg; // Урон, вчисляется позже с использованием коэффа, инициализируется нулём
        public Skill(string new_who_is, string new_name, string new_place, float new_k, float new_pro)
        {
            who_is = new_who_is;
            name = new_name;
            place = new_place;
            k = new_k;
            probability = new_pro;
            dmg = 0f;
        }

    }
}
