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
        private static Timer timer;
        public static Fighter f1;
        public static Fighter f2;
        public static int balance = 800;
        private static Random rnd;
        public static void start_fight( EventHandler new_event)
        {
            timer = new Timer();
            timer.Tick += new_event;
            timer.Interval = 1000;
            int time_elapsed = 0;
            timer.Tag = DateTime.Now;
            timer.Start();
            rnd = new Random();
        }
        private static string check_for_winner()
        {
            if (!f1.hp.is_alive()) return "f2 won battle";
            if (!f2.hp.is_alive()) return "f1 won battle";
            return "";
        }
        public static string action()
        {
            if (check_for_winner() == "")
            {
                if ((float)rnd.NextDouble() < f1.agressivness)
                {
                    Strike result = f2.repay(f1.act());
                    if (result == null) { return ("f1 defends"); }
                    else return (f1.name + " heats " + f2.name + " to " + result.place + " with " + result.dmg.ToString());
                }
                else
                {
                    Strike result = f1.repay(f2.act());
                    if (result == null) { return("f2 defends"); }
                    else return(f2.name + " heats " + f1.name + " to " + result.place + " with " + result.dmg.ToString());
                }
            }
            else
            {
                timer.Stop();
                return (check_for_winner());                
            }

        }
        public static int balance_count(int[] characts)
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
