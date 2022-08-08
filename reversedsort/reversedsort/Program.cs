using System;

namespace reversedsort
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            int aantal = int.Parse(Console.ReadLine());
            string[] array = Console.ReadLine().Split(' ');
            Console.WriteLine("Hello World!");
            insertionsortreversed(array);
        }
        public static void insertionsortreversed(string[] a)
        {
            for (int j = a.Length -2; j > -1; j--)
            {
                int key = int.Parse(a[j]);
                int i = j + 1;
                while(i<a.Length && int.Parse(a[i])< key)
                {
                    a[i - 1] = a[i];
                    i = i + 1;
                }
                a[i - 1] = key.ToString();
                for (int teller = 0; teller<a.Length; teller++)
                {
                    Console.Write(a[teller] + " ");
                }
                Console.Write("\n");
            }
        }
    }
}
