using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace tree_game
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Start of game");
            Table t = new Table("./example110.txt");
            t.Print();
            Console.WriteLine("Running cells");
            t.RunCells();
            t.Print();
            Console.WriteLine("Done");
        }



    }

    
}
