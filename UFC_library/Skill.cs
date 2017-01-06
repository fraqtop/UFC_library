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
        private float[] weights;
        public Skill(string new_who_is, string new_name, string new_place, float new_k, float new_pro)
        {
            who_is = new_who_is;
            name = new_name;
            if (name == "Heal") weights = new float[3];
            else weights = new float[4];
            Random rnd = new Random();
            for (int i = 0; i < weights.Length-1; i++)
            {
                weights[i] = (float)rnd.NextDouble();
            }
            place = new_place;
            k = new_k;
            probability = new_pro;
            dmg = 0f;
        }
        public void prob_recount(float[] paramz)
        {
            probability = 0;
            for (int i = 0; i < weights.Length-1; i++)
            {
                probability += weights[i] * paramz[i];
            }
        }
        public void correct_weights(float tactics)
        {
            for (int i = 0; i < weights.Length-1; i++)
                weights[i] += tactics;
        }
    }
}
