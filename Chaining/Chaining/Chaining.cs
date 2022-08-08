using System;
using System.Collections.Generic;


namespace Chaining
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            char[] delimiterchars = { ' ', '\t' };
            string[] line = Console.ReadLine().Split(delimiterchars);
            int nclauses = int.Parse(line[1]);
            int nconclusions = int.Parse(line[2]);
            string method = line[3];
            //A knowledge base KB for forward chaining:
            HashSet<Clause> KB = new HashSet<Clause>();
            //A separate knowledge base KB2 optimized for fast search in backward chaining
            //This Knowledge base is a dictionary with the keys being the symbols of the conclusions
            //that is, the "head" of the clauses and each symbol has a hashset of clauses that have this
            //conclusion, or "head".
            Dictionary<string, HashSet<Clause>> KB2 = new Dictionary<string, HashSet<Clause>>();
            //A dictionary to lookup in which clauses a certain symbol is in its premise. This is useful later
            //in forward chaining.
            Dictionary<string, HashSet<Clause>> KBAnts = new Dictionary<string, HashSet<Clause>>();
            Queue<string> agenda = new Queue<string>();
            Dictionary<string, bool> inferred = new Dictionary<string, bool>();
            for (int x = 0; x < nclauses; x++)
            {
                string clauseline = Console.ReadLine();
                List<string> rawsymbs = removeEmpty(clauseline.Split(delimiterchars));
                string firstsym = rawsymbs[0];

                //if the last character of a conclusion is a period, this means that it has no premise
                //and is true, hence, it is a fact.
                if (firstsym[firstsym.Length - 1] == '.')
                {
                    string fact = firstsym.Substring(0, firstsym.Length - 1);
                    Clause c = new Clause(fact);
                    //if there is no key, value pair with this head in the Knowledge Base 2 yet, make one
                    if (!KB2.ContainsKey(fact))
                        KB2[fact] = new HashSet<Clause>();
                    if (!KBAnts.ContainsKey(fact))
                        KBAnts[fact] = new HashSet<Clause>();
                    KB.Add(c);
                    c.PCount = 0;
                    //add the clause to the hashset of clauses with this head in the Knowledge Base 2 
                    KB2[fact].Add(c);
                    agenda.Enqueue(fact);
                    inferred[fact] = false;
                }
                else //it must be an implication then
                {
                    string conclusion = firstsym;
                    Clause c = new Clause(conclusion);
                    if (!KB2.ContainsKey(conclusion)) //if not present yet, again, make one.
                        KB2[conclusion] = new HashSet<Clause>();

                    //now, start parsing the rest of the implication (it's premise), starting from
                    //the next element.
                    int i = 1; //the next element, this is always the ":-" symbol
                    string rawsymbol = rawsymbs[i];
                    //whilst we haven't yet reached the end of the premise, keep parsing the premise
                    while (rawsymbol[rawsymbol.Length - 1] != '.')
                    {
                        //this means we reached the end of the line and we must go to the next line.
                        if (i >= rawsymbs.Count)
                        {
                            rawsymbs = removeEmpty(Console.ReadLine().Split(delimiterchars));
                            i = 0;//reset the line-element counter
                        }
                        rawsymbol = rawsymbs[i];//read the line-element symbol
                        if (rawsymbol != ":-")//don't do anything if the symbol is the ":-"-symbol.
                        {
                            //this means we have a symbol. now, remove the last character from the symbol,
                            //since this is always either a "," or a "." since we are in the premise of the
                            //implication. the "," and "." do not belong to the symbols itself
                            string psym = rawsymbol.Substring(0, rawsymbol.Length - 1);
                            c.Premise.Add(psym);//add the symbol to the clause's premise.
                            if (!KBAnts.ContainsKey(psym))
                                KBAnts[psym] = new HashSet<Clause>();
                            KBAnts[psym].Add(c);
                        }
                        i += 1;//go to the next element on the line
                    }
                    KB.Add(c);//we have reached the end, so add the clauses to the knowledge bases.
                    c.PCount = c.Premise.Count;
                    //add the clause to the hashset of clauses KB2 at the key that is its head symbol in the KB2.
                    KB2[conclusion].Add(c);
                    if (!KBAnts.ContainsKey(conclusion))
                        KBAnts[conclusion] = new HashSet<Clause>();
                    inferred[conclusion] = false;
                }

            }
            //start proving:
            string[] conclusions = new string[nconclusions];
            for (int x2 = 0; x2 < nconclusions; x2++)
            {
                string[] rawconclusion = Console.ReadLine().Split(delimiterchars);
                string conclusion = rawconclusion[1].Substring(0, rawconclusion[1].Length-1);
                conclusions[x2] = conclusion;
            }
            if (method == "f")
            {
                foreach (string q in conclusions)
                {
                    if ((FC_Entails(KB, q,KBAnts,agenda,inferred)||q=="true")&&q!="false")
                        Console.WriteLine(q + ". " + "true.");
                    else
                        Console.WriteLine(q + ". " + "false.");
                }
            }
            else
            {
                foreach (string q in conclusions)
                {
                    if ((BC_Entails(KB2, q)||q=="true")&&q!="false")
                        Console.WriteLine(q + ". " + "true.");
                    else
                        Console.WriteLine(q + ". " + "false.");
                }

            }
        }
        //standard forward chaining algorithm, as in AIMA.
        public static bool FC_Entails(HashSet<Clause> KB, string q,Dictionary<string,HashSet<Clause>> KBAnts, Queue<string> agenda, Dictionary<string, bool> inferred)
        {
            foreach (Clause c in KB)
            {
                inferred[c.Conclusion] = false;
                if (c.PCount <= 0)
                    agenda.Enqueue(c.Conclusion);
            }
            while (agenda.Count != 0)
            {
                string p = agenda.Dequeue();
                if (p == q)
                    return true;

                if (!inferred[p])
                {
                    inferred[p] = true;
                    foreach (Clause c in KBAnts[p])
                    {
                        c.PCount -= 1;
                        if (c.PCount <= 0)
                            agenda.Enqueue(c.Conclusion);
                    }
                }
            }
            return false;
        }
        //I made this function myself.
        //apparently I get a run error on run 9, which is for backtracking. it gives me a memory
        //limit error. I don't know why, please help.
        public static bool BC_Entails(Dictionary<string, HashSet<Clause>> KB2, string q)
        {
            HashSet<string> ancestors = new HashSet<string>();
            return proveIt(KB2,q,ancestors);
        }

        //proveIt, a recursive algorithm that tries to prove the specified q,
        //by trying to prove its premise in turn.
        public static bool proveIt(Dictionary<string, HashSet<Clause>> KB2, string q, HashSet<string> ancestors)
        {
            //first, check if the symbol is a head of a clause in the knowledge base, otherwise,
            //it cannot be proven at all, and we must return false
            if (KB2.ContainsKey(q))
            {
                //try to prove some clause which has head q, one suffices. if none can be proven, then
                //we cannot prove the conclusion q, and we must return false
                foreach (Clause c in KB2[q])
                {
                    //if it has no symbols in its premise (left) to prove, then it is true and
                    //we can return this.
                    if (c.PCount <= 0)
                        return true;
                    ancestors.Add(q);
                    //otherwise, try to prove each and every symbol in its premise by calling the
                    //proveIt function recursively.
                    foreach (string p in c.Premise)
                    {
                        //if proven a symbol in the premise, the number of symbols to prove is decremented
                        //by one
                        if (!ancestors.Contains(p))
                        {
                            if (proveIt(KB2, p, ancestors))
                            {
                                c.PCount -= 1;
                                ancestors.Remove(p);
                            }
                        }
                        //now, check if we have proven all its symbols in its premise by checking if the PCount
                        //is zero (or smaller, just in case). if so, return true, else try another clause with
                        //the head q, or else, if none are left, the loop will end, and we must return false.
                        if (c.PCount <= 0)
                            return true;
                    }
                }
            }
            return false;
        }
        public static List<string> removeEmpty(string[] splitstring)
        {
            List<string> nonempty = new List<string>();
            foreach (string s in splitstring)
            {
                if (s == "%")
                    return nonempty;

                if (s.Length != 0 && !s.Contains(" ") && !s.Contains("\t") && !s.Contains("\n"))
                    nonempty.Add(s);
            }
            return nonempty;
        }
    }
}
class Clause
{
    public string Conclusion;
    public int PCount;
    public HashSet<string> Premise = new HashSet<string>();
    public Clause(string conclusion)
    {
        Conclusion = conclusion;
        PCount = 0;
    }
}



