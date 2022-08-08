using System;
using System.Linq;

namespace air
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            int aantal = int.Parse(Console.ReadLine().Split(' ')[0]);
            Punt[] array = new Punt[aantal];
            for (int a=0; a<aantal; a++)
            {
                string[] line = Console.ReadLine().Split(' ');
                ulong X = ulong.Parse(line[0]);
                ulong Y = ulong.Parse(line[1]);
                array[a] = new Punt(X, Y);
            }
            Punt[] xgesoorteerdepoints = array.OrderBy(p => p.X).ToArray(); //orderby methode, want ik weet niet een andere manier om een array op x-coordinaat te sorteren
            int lengte = xgesoorteerdepoints.Length;
            Console.WriteLine(airtraffic(xgesoorteerdepoints, 0, lengte - 1, lengte).ToString());
        }

        public static ulong airtraffic(Punt[] points, int links, int rechts, int n)
        {
            if (rechts - links <= 2) //base case (als het verschil kleiner of gelijk aan 2 is, dan is het aantal punten kleiner of gelijk aan 3).
            {
                Punt[] points2 = new Punt[rechts-links+1]; // nieuwe array met lengte van deze punten
                Array.Copy(points, links, points2, 0, rechts - links + 1); //kopieer de punten naar die array
                return dommekracht(points2); //bereken kleinste afstand, op de standaard manier
            }
            int midden = (links + rechts) / 2;
            ulong lmin = airtraffic(points, links, midden, 1 + midden - links); //recursief links
            ulong rmin = airtraffic(points, midden + 1, rechts, rechts - midden); // recursief rechts
            ulong dmin = Math.Min(lmin, rmin);
            ulong cmin = minimumacross(points, midden, dmin, n); //bereken minimale afstand tussen links & rechts
            return Math.Min(dmin, cmin); // return minimale van dmin en cmin
        }

        public static ulong minimumacross(Punt[] points, int midden, ulong md, int n)
        {
            Punt[] strook1 = new Punt[n]; //vul array met punten binnen dmin afstand
            int b = 0;
            for (int a = 0; a < n; a++)
                if ((points[a].X > points[midden].X ? points[a].X - points[midden].X : points[midden].X - points[a].X) < md) // als verschil kleiner is dan dmin
                {
                    strook1[b] = points[a];
                    b = b + 1;
                }
            Punt[] strook = new Punt[b]; // strook array
            Array.Copy(strook1, 0, strook, 0, b); // kopieer elementen van de strook naar de array
            Punt[] sortedstrook = strook.OrderBy(p => p.Y).ToArray(); // orderby sorteer y-coordinaat
            ulong smallestdist = ulong.MaxValue; // groot omdat elke afstand kleiner moet zijn dan de initiele waarde
            for (int p1 = 0; p1 < b; p1++)
            {
                int p2 = p1 + 1;
                while (p2 < b && sortedstrook[p2].Y - sortedstrook[p1].Y < smallestdist)
                {
                    ulong distance = calculatedist(sortedstrook[p1], sortedstrook[p2]);
                    if (distance < smallestdist)
                    {
                        smallestdist = distance;
                    }
                p2 = p2 + 1;
                }
            }
            return smallestdist;
        }
        public static ulong dommekracht(Punt[] points)
        {
            ulong smallestdist = ulong.MaxValue;
            for (int punt = 0; punt < points.Length; punt++)
            {
                int punt2 = punt + 1;
                while (punt2 < points.Length)
                {
                    ulong distance = (calculatedist(points[punt], points[punt2]));
                    if (distance < smallestdist)
                    {
                        smallestdist = distance;
                    }
                    punt2 = punt2 + 1;
                }
            }
            return smallestdist;
        }

        public static ulong calculatedist(Punt punt1, Punt punt2)
        {
            return (punt1.X - punt2.X) * (punt1.X - punt2.X) + (punt1.Y - punt2.Y) * (punt1.Y - punt2.Y);
        }
    }
    public class Punt
    {
        public ulong X { get; }
        public ulong Y { get; }
        public Punt(ulong x, ulong y)
        {
            X = x;
            Y = y;
        }
    }
}
