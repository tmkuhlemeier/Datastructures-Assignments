using System;
using System.Text;

namespace Toetsen
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
                string sline = Console.ReadLine();
                string s = Wachtwoord(sline);
                array[x] = s;
            }
            for (int x = 0; x < inputnumber; x++)
            {
                Console.WriteLine(array[x]);
            }
        }

        /// <summary>
        /// Leest de input karakter voor karakter en roept de bijpassende
        /// methodes aan van een linked list, welke vervolgens op die list opereren.
        /// Construeert het wachtwoord.
        /// </summary>
        /// <param name="toetsen"></param>
        /// <returns>Het wachtwoord</returns>
        public static string Wachtwoord(string toetsen)
        {
            int i = 0;
            string c;
            CustomizedLinkedList<string> wachtwoordlist = new CustomizedLinkedList<string>();
            while (i < toetsen.Length)
            {
                c = toetsen[i].ToString();

                switch (c)
                {
                    case "<":
                        wachtwoordlist.MoveLeft();
                        break;

                    case ">":
                        wachtwoordlist.MoveRight();
                        break;

                    case "-":
                        wachtwoordlist.Delete();
                        break;

                    default:
                        wachtwoordlist.Insert(c);
                        break;
                }
                i = i + 1;

            }
            string wachtwoord = wachtwoordlist.ConvertToString();
            return wachtwoord;
        }
    }

    /// <summary>
    /// Doubly linked list welke afgestemd is op het dynamisch opslaan van karakters.
    /// </summary>
    class CustomizedLinkedList <Elem>
    {
        class Element
        {
            public Elem key;
            public Element prev;
            public Element next;
            public Element()
            {
                key = default;
                prev = null;
                next = null;
            }
        }

        Element head;
        Element current;

        public CustomizedLinkedList()
        {
            head = null;
            current = null;
        }

        // methodes
        public void Insert(Elem c)
        {
            Element new_element = new Element();
            new_element.key = c;
            if (current != null)
            {
                new_element.prev = current;
                Element temporary = current.next;
                if (current.next != null)
                    current.next.prev = new_element;
                current.next = new_element;
                new_element.next = temporary;
            }
            else
            {
                new_element.next = head;
                if (head != null)
                    head.prev = new_element;
                head = new_element;
                new_element.prev = null;
            }
            current = new_element;
            this.Update();
        }

        public void Delete()
        {
            if (current != null)
            {
                if (current.prev == null)
                {
                    if (current.next != null)
                    {
                        current.next.prev = null;
                        head = current.next;
                        current = null;
                    }
                    else
                    {
                        head = null;
                        current = head;
                    }
                }
                else
                {
                    Element temp;
                    temp = current.prev;
                    if (current.prev != null)
                        current.prev.next = current.next;
                    else
                    {
                        head = current.next;
                    }
                    if (current.next != null)
                        current.next.prev = current.prev;
                    current = temp;
                }
                this.Update();
            }
        }

        public void MoveLeft()
        {
            if (current != null)
            {
                current = current.prev;
                this.Update();
            }
        }

        public void MoveRight()
        {
            if (current == null)
            {
                current = head;
            }
            else if (current.next != null)
            {
                current = current.next;
            }
            this.Update();
        }

        // Zorgt ervoor dat de current en de head op de goede plek staan.
        public void Update()
        {
            if (current != null)
            {
                if (current.prev == null)
                {
                    head = current;
                }
                if (current.prev == head)
                    head.next = current;
            }
        }

        public string ConvertToString()
        {
            StringBuilder sb = new StringBuilder();
            Element element = head;

            while (element != null)
            {
                sb.Append(element.key);
                element = element.next;
            }
            return sb.ToString();
        }
    }
}
