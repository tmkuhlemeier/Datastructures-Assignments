using System;


namespace schijven
{
    class Program
    {
        public static void Main(string[] args)
        {
            string line = Console.ReadLine();
            ulong inputnumber = ulong.Parse(line.Split(null)[0]);
            ulong[] array = new ulong[inputnumber + 2];
            array[0] = 0;
            for (ulong x = 1; x < inputnumber + 1; x++)
            {
                string sline = Console.ReadLine();
                ulong startnumber = ulong.Parse(sline.Split(null)[0]);
                array[x] = startnumber;

            }
            line = Console.ReadLine();
            inputnumber = ulong.Parse(line.Split(null)[0]);
            ulong solution = 0;
            for (ulong x = 0; x < inputnumber; x++)
            {
                string sline = Console.ReadLine();
                ulong startnumber = ulong.Parse(sline.Split(null)[0]);
                array[array.Length - 1] = startnumber + 1;
                solution += calculateschijven(array, startnumber);

            }
            Console.WriteLine(solution);
        }
        static ulong calculateschijven(ulong[] x, ulong hoogte)
        {
            long laag = 0;
            long hoog = x.Length - 1;
            while (hoog - laag > 1)
            {
                long midden = (laag + hoog) / 2;
                if (x[midden] <= hoogte)
                {
                    laag = midden;
                }
                else
                {
                    hoog = midden;
                }
            }

            return hoogte - x[laag];
        }
    }
}

    

        