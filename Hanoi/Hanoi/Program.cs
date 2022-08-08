using System;

namespace Hanoi
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            int aantal1 = int.Parse(Console.ReadLine());
            Hanoiclass hanoi = new Hanoiclass();
            hanoi.runHanoi(aantal1);
        }
    }
    public class Hanoiclass
    {
        int aantalZetten = 0;
        int cA;
        int cB = 0;
        int cC = 0;
        public void runHanoi(int aantal)
        {
            cA = aantal;
            Hanoi('A', 'C', aantal);
        }
        public void Hanoi(char van, char naar, int aantal)
        {
            char temp = TempPin(van, naar); // tijdelijke pin is temp
            if (aantal == 1)
            { // nu gaan we een echte zet doen
                aantalZetten++;
                cA = count(van, naar, 'A', cA);
                cB = count(van, naar, 'B', cB);
                cC = count(van, naar, 'C', cC);
                Console.WriteLine("A: " + cA + " B: " + cB + " C: " + cC + " " + aantalZetten +" "+ temp);
            }
            else
            { // aantal is groter dan 1, dus ...
                Hanoi(van, temp, aantal - 1); // zet n-1 aan de kant
                Hanoi(van, naar, 1); // zet 1 schijf goed
                Hanoi(temp, naar, aantal - 1); // zet de n-1 terug

            }
        }
        public static char TempPin(char van, char naar)
        {
            if (van == 'A')
            {
                if (naar == 'B')
                {
                    return 'C';
                }
                else
                {
                    return 'B';
                }
            }
            if (van == 'B')
            {
                if (naar == 'C')
                {
                    return 'A';
                }
                else
                {
                    return 'C';
                }
            }
            else if (van == 'C')
            {
                if (naar == 'A')
                {
                    return 'B';
                }
                else
                {
                    return 'A';
                }
            }
            return 'e';
        }
        static int count(char van, char naar, char c, int co)
        {
            if (c == van)
            {
                return co - 1;
            }
            if (c == naar)
            {
                return co + 1;
            }
            return co;
        }
    }

}
