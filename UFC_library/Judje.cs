using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UFC_library
{
    public static class Judje
    {
        private static Timer timer; // Таймер, по которому судья выполняет инструкции
        public static List<Fighter> fighters = new List<Fighter>(2); // Собственно список из двух бойцов
        public static int balance = 800; // Балансовые очки. Нужны чтобы нельзя было все скиллы по сотке ставить
        private static int seconds_remaining = 15; // Секунд до конца боя
        private static Random rnd; // Для генерации случайных чисел
        public static List<Skill> log; // Список нанесённых ударов для вывода в интерфейс

        public static void start_fight( EventHandler new_event)
        {
            log = new List<Skill>();
            timer = new Timer();
            timer.Tick += new_event; // Событие, выполняющееся по тику таймера
            timer.Interval = 1000; // Через каждую секунду
            timer.Start();
            rnd = new Random();
        }
        private static Fighter check_for_winner()
        {
            if (seconds_remaining == -1) // Если бой закончен выявить победителя
            {
                float average = (fighters[0].recieved_damage + fighters[1].recieved_damage) / 2;
                return fighters.First(x => x.recieved_damage > average);
            }
            else
                try { return fighters.Where(x => x.hp.is_alive() == false).First(); } // Извлечь нокаутированного, если таковой есть
                catch { return null; } // Если нет, вернуть пустой объект
        }
        public static string action() // Функция, выполняющаяся каждую секунду
        {
            seconds_remaining--;
            if (check_for_winner() != null) // Если есть победитель
            {
                timer.Stop();
                return comment(check_for_winner()); // Вернуть сообщение о победе
            }
            if ((float)rnd.NextDouble() < fighters[0].agressivness / fighters.Sum(x => x.agressivness)) // Если рандомное число в пределах агрессии бойца, то его ход
            {
                fighters[1].repay(fighters[0].act());
            }
            else fighters[0].repay(fighters[1].act()); // Если нет, ходит другой
            return comment(); // Откомментировать последнее событие в списке
        }

        private static string comment() // Формирование комментария
        {
            Skill last = log.Last();
            log.Remove(last);
            if (last.name=="Heal") return last.who_is + " defends";
            if (last.dmg == 0) return last.who_is + " missed";
            return last.who_is + " hits to " + last.place + " with " + last.dmg + " pts of damage";
        }
        private static string comment(Fighter looser) // Формироваие сообщения о победе
        {
            Fighter winner = fighters.First(x => x != looser);
            return winner.name + 
                " won this battle. He made " + 
                looser.recieved_damage + 
                " points of damage and recieved only " + 
                winner.recieved_damage;
        }
        public static int balance_count(int[] characts) // Подсчёт баланса
        {
            int remains = balance;
            for (int i = 0; i < 9; i++)
            {
                remains -= characts[i];
            }
            return remains;
        }
    }
}
