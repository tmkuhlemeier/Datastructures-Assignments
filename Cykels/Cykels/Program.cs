using System;
using System.Collections.Generic;
using System.Text;

namespace Cykels
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string line = Console.ReadLine(); //lees de grootte van de set
            int inputnumber = int.Parse(line.Split(null)[0]);
            string[] array = new string[inputnumber];
            for (int x = 0; x < inputnumber; x++) //lees de input en los op
            {
                int num_verteces = int.Parse(Console.ReadLine());
                int num_edges = int.Parse(Console.ReadLine());
                Graph graph = new Graph(num_verteces);
                for (int i =0; i< num_edges; i++)
                {
                    string[] sline = Console.ReadLine().Split(' ');
                    int vertex_1 = int.Parse(sline[0]);
                    int vertex_2 = int.Parse(sline[1]);
                    graph.AddEdge(vertex_1, vertex_2);
                }
                string oplossing = graph.Topologicalsort();
                array[x] = oplossing;
            }
            for (int x = 0; x < inputnumber; x++)
            {
                Console.WriteLine(array[x]);
            }
        }
    }

    class Graph
    {
        class Vertex
        {
            public int key;
            public string color;
            public Vertex p;
            public Vertex(int key1)
            {
                key = key1;
                p = null;
                color = "WHITE";
            }
        }
        Vertex[] V;
        LinkedList<int>[] Adj;
        LinkedList<int> ordering;
        Stack<(int v1, int v2)> potential_breaks; // edges die de cykel zouden kunnen verbreken
        bool fixing; // wordt er gefixt?
        bool cycle; // bevat de graaf een cykel?

        public Graph(int length)
        {
            Adj = new LinkedList<int>[length]; //een array van linked lists(van elke vertex één)
            V = new Vertex[length]; // lijst van verteces
            ordering = new LinkedList<int>();
            potential_breaks = new Stack<(int v1, int v2)>();
            fixing = false;
            cycle = false;
            AddVerteces(length); // voeg alle verteces toe van 1 tm length -1   
        }

        public void AddVerteces(int length)
        {
            for (int i = 0; i < length; i++)
            {
                V[i] = new Vertex(i);
                Adj[i] = new LinkedList<int>();
            }
        }

        public void AddEdge(int key1, int key2)
        {
            Adj[key1].AddLast(key2);
        }

        /// <summary>
        /// Maak een topologische ordening van een graaf:
        /// Roep DFS aan. Als de graaf niet cyclisch is, return dan de ordening.
        /// Soms kan het gefixt worden. Kan het niet, return dan no fix.
        /// </summary>
        /// <returns>Een string welke aangeeft of de graaf, acyclisch is, cyclisch is,
        ///  en of het gefixt kan worden. Eventueel topologische ordening.</returns>
        public string Topologicalsort()
        {
            string topological_ordering;
            (int v1, int v2) break_edge = (-1, -1); // default waarde van de tuple, edge -1, -1 bestaat niet
            DFS();

            if (cycle)
            {
             // Loop de edges van de cykel af (dit zijn potentiële breaks) en verwijder er
             // één, voer opnieuw DFS uit en en kijk ofdat er nu nog een cykel is.
             // Zo nee, stop, er is een fix, anders voeg de edge weer toe
             // aan de adjacency list/array en doe hetzelfde op een volgende edge in de cykel.
             // Als alle edges in de cykel afgelopen zijn en er is nog steeds een cykel, dan is er no fix.
                while (potential_breaks.Count != 0 && cycle)
                {
                    Reset();
                    break_edge = potential_breaks.Pop();
                    Adj[break_edge.v1].Remove(break_edge.v2);                 
                    DFS();
                    AddEdge(break_edge.v1, break_edge.v2);
                }
                if (cycle && potential_breaks.Count == 0)
                {
                    return "no fix";
                }
                else
                {
                    topological_ordering = ConvertToString(this.ordering);
                    return "fix " + break_edge.v1 + " " + break_edge.v2 + "\n" + topological_ordering;
                }
            }
            else
            {
                topological_ordering = ConvertToString(this.ordering);
                return "acyclic " + "\n" + topological_ordering;
            }
        }

        public void DFS()
        {
            foreach (Vertex u in V)
            {
                if (u.color == "WHITE")
                {
                    DFSVisit(u);
                }
            }
        }

        void DFSVisit(Vertex u)
        {
            u.color = "GRAY";
            foreach (int i in Adj[u.key])
            {
                Vertex v = V[i];
                if (v.color == "WHITE")
                {
                    v.p = u;
                    DFSVisit(v);
                }
                else if (v.color == "GRAY")
                {
                    cycle = true;
                    if (!fixing)
                    {
                        // construeer de cykel door parent pointers te volgen tot
                        // weer bij v aangekomen en voeg de edges toe aan een stack.
                        // doe dit alleen als we voor het eerst een cykel zien in de graaf
                        // en deze niet aan het fixen is.
                        Vertex x = u;
                        (int v1, int v2) edge = (v1: u.key, v2: v.key);
                        potential_breaks.Push(edge);
                        while (x.key != v.key)
                        {
                            edge = (v1: x.p.key, v2: x.key);
                            potential_breaks.Push(edge);
                            x = x.p;
                        }
                        fixing = true; // nu kunnen we alleen nog kijken of er gefixt kan worden
                    }
                }
            }
            u.color = "BLACK";
            ordering.AddFirst(u.key);
        }

        /// Reset de hele graaf
        public void Reset()
        {
            foreach (Vertex v in V)
            {
                v.p = null;
                v.color = "WHITE";
            }
            cycle = false;
            ordering = new LinkedList<int>();
        }

        public string ConvertToString(LinkedList<int> ordering)
        {
            StringBuilder sb = new StringBuilder();
            while (ordering.First != null)
            {
                sb.Append(ordering.First.Value + " ");
                ordering.RemoveFirst();
            }
            return sb.ToString();
        }
    }
}
