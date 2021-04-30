using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace tree_game
{
    class Table : IEnumerable<Cell>
    {
        public static Exception badTableException = new Exception("The input file is not in the correct format.");
        public int size;
        Cell[][] contents;

        public Table(string file)
        {
            string[] strings = File.ReadAllLines(file);
            //Read the size of the table from the first line of the file
            size = int.Parse(strings[0]);
            if (strings.Length != size + 2)
            {
                throw badTableException;
            }
            else
            {
                contents = new Cell[size][];
                int i = 0;
                foreach (string s in strings.Skip(2))
                {
                    //Make sure that every row is the right size and that there are the right number of rows
                    if (s.Length != size || i >= size)
                    {
                        throw badTableException;
                    }
                    //Project one string into an array of cells
                    contents[i] = s.Select((x, pos) => new Cell(this, i, pos, Convert.ToInt32(x.ToString(), 16))).ToArray();
                    i++;
                }
            }
        }

        public Cell this[int row, int col] => contents[row][col];

        public IEnumerator<Cell> GetEnumerator()
        {
            return contents.SelectMany(x => x).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Print()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            //Print out each cell of the table
            foreach (Cell[] row in contents)
            {
                foreach (Cell c in row)
                {
                    //Set the color for the cell and print the appropriate character
                    Console.BackgroundColor = (ConsoleColor)c.color;
                    Console.Write((char)c.fill);
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
            }
            //Reset the console for printing by other parts of the program
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }


        public int CountRow(int row) => contents[row].Count(x => x.fill == CellFill.tree);
        public int CountCol(int col) => contents.Count(x => x[col].fill == CellFill.tree);
        public int CountColor(int color) => this.Count(x => x.color == color && x.fill == CellFill.tree);

        public IEnumerable<Cell> EmptyInRow(int row) => contents[row].Where(x => x.fill == CellFill.empty);
        public IEnumerable<Cell> EmptyInCol(int col) => contents.Select(x => x[col]).Where(x => x.fill == CellFill.empty);
        public IEnumerable<Cell> EmptyInColor(int color) => this.Where(x=>x.color == color && x.fill == CellFill.empty);
    }
}