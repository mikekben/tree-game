using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace tree_game
{
    class Program
    {
        static int tableSize = 1;
        public static Cell[][] table;


        static void Main(string[] args)
        {
            Console.WriteLine("Start of game");
            Table t = new Table("./example110.txt");
            t.Print();
            Console.WriteLine("Placing one tree");
            t[0,5].PlaceTree();
            t.Print();
            Console.WriteLine("Placing second tree");
            t[0,7].PlaceTree();
            t.Print();
            Console.WriteLine("Done");
        }



    }

    
}
