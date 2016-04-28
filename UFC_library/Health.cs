using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFC_library
{
    //Класс, инвапсулиующий работу с хп
    public class Health
    {
        private float[] hps; // Массив с хп по разным частям тела
        public Health(float n_head, float n_body, float legs, float hands)
        {
            hps = new float[4];
            hps[0] = n_head;
            hps[1] = n_body;
            hps[2] = legs;
            hps[3] = hands;
        }

        public void damage(Skill shot) // Процедура нанесения повреждений игроку
        {
            switch (shot.place)
            {
                case "head":
                    hps[0] -= shot.dmg;
                    break;
                case "stomach":
                    hps[1] -= shot.dmg;
                    break;
                case "legs": // хп соотв части тела уменьшается на количество урона
                    hps[2] -= shot.dmg;
                    break;
                case "hands":
                    hps[3] -= shot.dmg;
                    break;
            }
        }
        public bool check(float currtac)  // Проверка на то, стоит ли уходить в защиту
        {
            for (int i = 0; i < 3; i++)
            {
                if ((hps[i] - currtac * 10) < -30) return false; // Если значение хп меньше критического, то лучше уйти в защиту
            }
            return true;
        }
        public void recovery(float currendurance) // Восстановление хп при уходе в глухую оборону
        {
            for (int i = 0; i < 3; i++)
            {
                hps[i] += currendurance * 10;
                if (hps[i] > 100) hps[i] = 100; // Не может быть больше ста хп
            }
        }
        public bool is_alive() // Проверка на боеспособность
        {
            for (int i = 0; i < 3; i++)
            {
                if (hps[i] <= 0) return false; // Если хп меньше нуля, то это нокаут
            }
            return true;
        }
    }
}
