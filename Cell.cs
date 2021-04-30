using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace tree_game
{
    enum CellFill { empty = ' ', excluded = '-', tree = 'X' }
    class Cell
    {
        public static Exception cantPlaceException = new Exception("A tree cannot be placed in this cell.");

        public Table table;
        public int row;
        public int col;
        public int color;
        public CellFill fill;

        public Cell Right => col >= table.size - 1 ? null : table[row,col + 1];
        public Cell Left => col <= 0 ? null : table[row,col - 1];
        public Cell Below => row >= table.size - 1 ? null : table[row + 1,col];
        public Cell Above => row <= 0 ? null : table[row - 1,col];

        public Cell RB => Right == null ? null : Right.Below;
        public Cell RA => Right == null ? null : Right.Above;
        public Cell LB => Left == null ? null : Left.Below;
        public Cell LA => Left == null ? null : Left.Above;

        public IEnumerable<Cell> Adjacent => new List<Cell> { Right, Left, Below, Above, RB, RA, LB, LA }.Where(x => x != null);

        public Cell(Table table,int row, int col, int color)
        {
            this.table = table;
            this.row = row;
            this.col = col;
            this.color = color;
            this.fill = CellFill.empty;
        }

        public void PlaceTree()
        {
            //Check to make sure a tree can be placed here
            if (fill != CellFill.empty || table.CountRow(row) >= 2 || table.CountCol(col) >= 2 || table.CountColor(color) >= 2)
            {
                throw cantPlaceException;
            }
            else
            {
                //Place the tree
                fill = CellFill.tree;
                //Exclude all the adjacent cells
                foreach (Cell c in Adjacent)
                {
                    if (c.fill == CellFill.tree)
                    {
                        throw cantPlaceException;
                    }
                    else
                    {
                        c.fill = CellFill.excluded;
                    }
                }

                //Exclude the column, row, and color
                if (table.CountRow(row) >= 2)
                {
                    foreach (Cell c in table.EmptyInRow(row))
                    {
                        c.fill = CellFill.excluded;
                    }
                }
                if (table.CountCol(col) >= 2)
                {
                    foreach (Cell c in table.EmptyInCol(col))
                    {
                        c.fill = CellFill.excluded;
                    }
                }
                if (table.CountColor(color) >= 2)
                {
                    foreach (Cell c in table.EmptyInColor(color))
                    {
                        c.fill = CellFill.excluded;
                    }
                }

            }
        }
    }
}