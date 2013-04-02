using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTD.BO
{
    public class SearchNode
    {
        public Point Position;
        public bool Walkable;
        public SearchNode[] Neighbors;
        public SearchNode Parent;
        public bool InOpenList;
        public bool InClosedList;
        public float DistanceToGoal;
        public float DistanceTraveled;
    }
}
