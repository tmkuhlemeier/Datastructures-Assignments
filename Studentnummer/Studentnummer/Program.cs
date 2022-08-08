using System;

namespace Studentnummer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, enter student number.");
            string studentnumber;
            hulp helpje = new hulp();
            studentnumber = Console.ReadLine();
            var charArray = studentnumber.ToCharArray();
            int newnumber = 0;
            for (int x = 0; x < 7; x++)
            {
                int g = 7 - x;
                int n1 = newnumber;
                newnumber = n1 + (int.Parse(new string(charArray[x], 1)) * (g));
                Console.WriteLine("berekening "+ n1.ToString() + " + " + charArray[x].ToString() + " * " + g.ToString() + " = " + newnumber.ToString());
            }
            if (helpje.divide(newnumber) == true)
                Console.WriteLine("this is a studentnumber");
            else Console.WriteLine("error");

        }
    }
    public class hulp
    {
        public bool divide(int x)
        {
            return x % 11 == 0;
        }
    }
}
