using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTD.BO
{
    public class Cell
    {
        public bool walkable = true;
        public bool bestPath = false;

        public int x;
        public int y;

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
