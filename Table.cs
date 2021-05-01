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
        public int treeNumber;
        Cell[][] contents;

        int[] colors;

        public Table(string file)
        {
            string[] strings = File.ReadAllLines(file);
            //Read the size of the table from the first line of the file
            size = int.Parse(strings[0].Split(',')[0]);
            treeNumber = int.Parse(strings[0].Split(',')[1]);
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
            colors = this.Select(x => x.color).Distinct().ToArray();
        }

        public Table(int size, int treeNumber, Cell[][] contents)
        {
            this.size = size;
            this.treeNumber = treeNumber;
            this.contents = contents;
            this.colors = this.Select(x => x.color).Distinct().ToArray();
        }

        public Table Copy()
        {
            //Make a completely new copy of every cell
            Cell[][] newArray = new Cell[size][];
            for (int i = 0; i < size; i++)
            {
                newArray[i] = new Cell[size];
                for (int j = 0; j < size; j++)
                {
                    Cell t = new Cell(null, i, j, this[i, j].color);
                    newArray[i][j] = t;
                }
            }
            Table toReturn = new Table(size, treeNumber, newArray);
            //This can only be done after the table has been constructed
            foreach (Cell c in toReturn) { c.table = toReturn; }
            return toReturn;
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

        public void RunCells()
        {
            //Try adding a tree at each cell to exclude 
            foreach (Cell c in this)
            {
                if (c.fill == CellFill.empty)
                {
                    Table newTab = Copy();
                    newTab[c.row, c.col].PlaceTree();
                    if (!newTab.Validate()) { c.fill = CellFill.excluded; }
                }
            }
        }
        public bool Validate()
        {
            for (int i = 0; i < size; i++)
            {
                if (treeNumber - CountRow(i) > EmptyInRow(i).Count()) { return false; }
                if (treeNumber - CountCol(i) > EmptyInCol(i).Count()) { return false; }
            }
            foreach (int c in colors)
            {
                if (treeNumber - CountColor(c) > EmptyInColor(c).Count()) { return false; }
            }
            return true;
        }


        public int CountRow(int row) => contents[row].Count(x => x.fill == CellFill.tree);
        public int CountCol(int col) => contents.Count(x => x[col].fill == CellFill.tree);
        public int CountColor(int color) => this.Count(x => x.color == color && x.fill == CellFill.tree);

        public IEnumerable<Cell> EmptyInRow(int row) => contents[row].Where(x => x.fill == CellFill.empty);
        public IEnumerable<Cell> EmptyInCol(int col) => contents.Select(x => x[col]).Where(x => x.fill == CellFill.empty);
        public IEnumerable<Cell> EmptyInColor(int color) => this.Where(x => x.color == color && x.fill == CellFill.empty);
    }
}