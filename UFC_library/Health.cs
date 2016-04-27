using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFC_library
{
    public class Health
    {
        private float[] hps;
        public Health(float n_head, float n_body, float legs, float hands)
        {
            hps = new float[4];
            hps[0] = n_head;
            hps[1] = n_body;
            hps[2] = legs;
            hps[3] = hands;
        }

        public void damage(Strike shot)
        {
            switch (shot.place)
            {
                case "head":
                    hps[0] -= shot.dmg;
                    break;
                case "stomach":
                    hps[1] -= shot.dmg;
                    break;
                case "legs":
                    hps[2] -= shot.dmg;
                    break;
                case "hands":
                    hps[3] -= shot.dmg;
                    break;
            }
        }
        public bool check(float currtac)
        {
            for (int i = 0; i < 3; i++)
            {
                if ((hps[i] - currtac * 10) < -30) return false;
            }
            return true;
        }
        public void recovery(float currendurance)
        {
            for (int i = 0; i < 3; i++)
            {
                hps[i] += currendurance * 10;
                if (hps[i] > 100) hps[i] = 100;
            }
        }
        public bool is_alive()
        {
            for (int i = 0; i < 3; i++)
            {
                if (hps[i] <= 0) return false;
            }
            return true;
        }
    }
}
