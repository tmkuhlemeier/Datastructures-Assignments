using System;

namespace fibonacci
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            int aant = int.Parse(Console.ReadLine());
                Console.WriteLine(fibonacci(aant));
        }
        public static int fibonacci (int n)
        {
            int f = 0;
            if (n > 1)
            {
                f = fibonacci(n - 2) + fibonacci(n - 1);

            }
            else if (n == 1)
                f = 1;
            return f;
        }
    }
}
