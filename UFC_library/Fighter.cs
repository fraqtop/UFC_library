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
        private float speed { get; set; }  // Скорость влияет на итоговую силу удара
        public float weight { get; set; } // Вес руки влияет на силу удара
        public float height { get; set; }
        private float endurance { get; set; } // Влияет на скорость регенерации
        private float accuracy { get; set; } // Влияет на вероятность попадания
        private float tactics { get; set; } // Влияет на критический уровень показателей и "обдуманность" решений
        public float agressivness { get; set; } // Влияет на вероятность нанесения удара в каждый момент времени
        private float _stamina; // Влияет на силу удара (мышечная усталость)
        private Dictionary<string, float> defence { get; set; } // Уровни защиты для разных частей тела
        public Random rnd { get; set; } // Генерация случайных чисел
        public bool block { get; set; } // Стоит или нет блок, влияет на урон
        public float recieved_damage { get; private set; } // Общий полученный урон
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
        protected List<Skill> skills; // Приёмы
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
            block = true;
            skills = new List<Skill>();
            skills.Add(new Skill(n_name, "Heal", "", 0f, 0f)); // Объект, возвращаемый при защите
            skills.Add(new Skill(n_name, "Uppercut", "head", 1.2f, 14));
            skills.Add(new Skill(n_name, "Low_kick", "legs", 1.15f, 14));
            skills.Add(new Skill(n_name, "Jab", "head", 0.9f, 14));
            skills.Add(new Skill(n_name, "Cross", "stomach", 1.25f, 14));
            skills.Add(new Skill(n_name, "Hook", "head", 1.8f, 14));
        }

        protected void recount_probabilities() // Подсчёт относительных вероятностей нанесения удара
        {
            float sum = skills.Sum(x => x.probability);
            float segment = 0;
            foreach (Skill s in skills)
            {
                s.probability /= sum;
                s.probability += segment;
                segment = s.probability;
            }
        }

        private float dmg_count(float k) // Подсчёт наносимого врагу урона
        {
            if ((float)rnd.NextDouble() > accuracy) return 0f; // Если промахнулся
            float kinetic = (float)(Math.Pow(speed, 2) * weight) / 2f;
            return kinetic * k * stamina;
        }

        private Skill attack()
        {
            float r = (float)this.rnd.NextDouble();
            Skill result = skills.Where(x => x.probability > r).First(); //Выбор приёма
            result.dmg = dmg_count(result.k);
            stamina -= (result.k - endurance) / 4;
            return result;
        }

        public void repay(Skill currstrike) // Реакция на удар соперника
        {
            Judje.log.Add(currstrike); // Зарегистрировать удар
            float k = 0;
            defence.TryGetValue(currstrike.place, out k);
            if (block & k!=0) // Если стоит блок, урон уменьшается
            {
                currstrike.dmg /= k * 10;
            }
            this.hp.damage(currstrike);
            recieved_damage += currstrike.dmg;
        }

        private void Extradamage(Dictionary<string, float> dic) // Дополнительный урон (на другие хар-ки)
        {

        }

        private Skill defend()
        {
            stamina += endurance / 3;
            hp.recovery(endurance);
            List<string> keys = new List<string>(defence.Keys);
            return skills.First(x => x.name == "Heal");
        }

        public Skill act() // Выбор между атакой и защитой
        {
            if (agressivness > tactics & (float)rnd.NextDouble() < agressivness) return attack();
            if (make_desicion()) return attack();
            else return defend();
        }

        private bool make_desicion() // Механизм принятия решения
        {
            if (!hp.check(tactics)) return false;
            if (stamina - tactics < -0.3f) return false;
            return true;
        }
    }
}
