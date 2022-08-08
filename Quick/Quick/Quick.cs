using System;

namespace Quick
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string line = Console.ReadLine(); //lees de grootte van de set
            int inputnumber = int.Parse(line.Split(null)[0]);
            Breuk[] array = new Breuk[inputnumber];
            int k = int.Parse(line.Split(' ')[1]);
            for (int x = 0; x < inputnumber; x++) //lees de input en los op
            {
                string sline = Console.ReadLine();
                Breuk a = new Breuk();
                a.teller = long.Parse(sline.Split(null)[0]);
                a.noemer = long.Parse(sline.Split(null)[1]);
                array[x] = a;
            }
            Quicksort(array, 0, array.Length - 1, k);
            Console.WriteLine(inputnumber);
            for (int x = 0; x < inputnumber; x++)
            {
                Console.WriteLine(array[x].teller + " " + array[x].noemer);
            }
        }
        public static void Quicksort(Breuk[] A, int p, int r, int k)
        {
            if (p < r)
            {
                if (r - p + 1 <= k)
                {
                    Selectionsort(A, p, r);
                }
                else
                {
                    int q = Partition(A, p, r);
                    Quicksort(A, p, q - 1, k);
                    Quicksort(A, q + 1, r, k);
                }
            }
        }
        public static int Partition(Breuk[] A, int p, int r)
        {
            Breuk x = A[r];
            int i = p - 1;
            for (int j = p; j < r; j++)
            {
                if (A[j].CompareTo(x) <= 0)
                {
                    i = i + 1;
                    Breuk tijdelijk = A[i];
                    A[i] = A[j];
                    A[j] = tijdelijk;
                }
            }
            Breuk tijdelijk2 = A[i + 1];
            A[i + 1] = A[r];
            A[r] = tijdelijk2;
            return i + 1;
        }
        public static void Selectionsort(Breuk[] A, int p, int r)
        {
            int i = p;
            while (i < r)
            {
                int j = i + 1;
                int kleinste = i;
                while (j < r + 1)
                {
                    if (A[j].CompareTo(A[kleinste]) < 0)
                    {
                        kleinste = j;
                    }
                    j = j + 1;
                }
                Breuk tijdelijk = A[i];
                A[i] = A[kleinste];
                A[kleinste] = tijdelijk;
                i = i + 1;
            }
        }
    }
    class Breuk : IComparable <Breuk>
    {
        public long teller;
        public long noemer;
        public int CompareTo(Breuk obj)
        {
            Breuk andere = obj;
            long teller1 = this.teller * andere.noemer;
            long teller2 = andere.teller * this.noemer;
            if (teller1 == teller2)
            {
                return (int)(this.noemer - andere.noemer);
            }
            else
            {
                return (int)(teller1 - teller2);
            }
            //throw new NotImplementedException();
        }
    }
}
