using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFC_library
{
    public class Fighter
    {
        public string name { get; set; }
        public Health hp { get; set; }
        private float speed { get; set; }
        public float weight { get; set; }
        public float height { get; set; }
        private float endurance { get; set; }
        private float accuracy { get; set; }
        private float tactics { get; set; }
        public float agressivness { get; set; }
        private float _stamina;
        private Dictionary<string, float> defence { get; set; }
        public Random rnd { get; set; }
        public bool rack { get; set; }
        public bool block { get; set; }
        public float recieved_damage { get; private set; }
        public float stamina
        {
            get { return _stamina; }
            set
            {
                if (value < 0) _stamina = 0.1f;
                if (value >= 1) _stamina = 1;
                if (value < 1 & value > 0) _stamina = value;
            }
        }
        protected List<Strike> strikes;
        public Fighter(string n_name, Health n_hp, float n_sp, float n_end, float n_acc,
            float n_tac, float n_agr, float n_we, float n_he, Dictionary<string, float> n_de)
        {
            List<string> keys = new List<string>(n_de.Keys);
            defence = n_de;
            foreach (string k in keys)
            {
                defence[k] /= 100;
            }
            name = n_name;
            hp = n_hp;
            speed = n_sp / 10;
            endurance = n_end / 100;
            accuracy = n_acc / 100;
            tactics = n_tac / 100;
            agressivness = n_agr / 100;
            _stamina = 1f;
            weight = n_we / 25;
            height = n_he / 100 * 46;
            recieved_damage = 0f;
            rnd = new Random();
            rack = true;
            block = true;
            strikes = new List<Strike>();
            strikes.Add(new Strike("Uppercut", "head", 1.2f, 14));
            strikes.Add(new Strike("Low_kick", "legs", 1.15f, 14));
            strikes.Add(new Strike("Jab", "head", 0.9f, 14));
            strikes.Add(new Strike("Cross", "stomach", 1.25f, 14));
            strikes.Add(new Strike("Hook", "head", 1.8f, 14));
        }

        protected void recount_probabilities()
        {
            float sum = strikes.Sum(x => x.probability);
            float segment = 0;
            foreach (Strike s in strikes)
            {
                s.probability /= sum;
                s.probability += segment;
                segment = s.probability;
            }
        }

        private float dmg_count(float k)
        {
            if ((float)rnd.NextDouble() > accuracy) return 0f;
            float kinetic = (float)(Math.Pow(speed, 2) * weight) / 2f;
            return kinetic * k * stamina;
        }

        private Strike attack()
        {
            float r = (float)this.rnd.NextDouble();
            Strike result = strikes.Where(x => x.probability > r).First();
            result.dmg = dmg_count(result.k);
            stamina -= (result.k - endurance) / 4;
            return result;
        }

        public Strike repay(Strike currstrike)
        {
            if (currstrike == null) return null;
            if (currstrike.dmg == 0) return currstrike;
            float k = 0;
            defence.TryGetValue(currstrike.place, out k);
            if (block != false | (float)rnd.NextDouble() < k)
            {
                currstrike.dmg /= k * 10;
            }
            this.hp.damage(currstrike);
            recieved_damage += currstrike.dmg;
            return currstrike;
        }

        private void Extradamage(Dictionary<string, float> dic)
        {

        }

        private void defend()
        {
            stamina += endurance / 3;
            hp.recovery(endurance);
            List<string> keys = new List<string>(defence.Keys);
        }

        public Strike act()
        {
            if (agressivness > tactics & (float)rnd.NextDouble() < agressivness) return attack();
            if (make_desicion() == true) return attack();
            else defend();
            return null;
        }

        private bool make_desicion()
        {
            if (!hp.check(tactics)) return false;
            if (stamina - tactics < -0.3f) return false;
            return true;
        }
    }
}
