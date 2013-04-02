using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTD.BO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTD.BOL
{
    public class PathFinder
    {
        private SearchNode[,] searchNodes;

        public static Map map;

        private static PathFinder _instance;

        public static PathFinder Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PathFinder(map);
                return _instance;
            }
        }

        private int levelWidth;
        private int levelHeight;

        public PathFinder(Map map)
        {
            levelWidth = map.width;

            levelHeight = map.height;

            InitializeSearchNodes(map);
        }

        private void InitializeSearchNodes(Map map)
        {
            searchNodes = new SearchNode[levelWidth, levelHeight];

            for (int x = 0; x < levelWidth; ++x)
            {
                for (int y = 1; y < levelHeight; ++y)
                {
                    SearchNode node = new SearchNode();

                    node.Position = new Point(x, y);

                    node.Walkable = map.map[x, y].walkable;

                    if (node.Walkable)
                    {
                        node.Neighbors = new SearchNode[8];
                        searchNodes[x, y] = node;
                    }
                }
            }

            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    SearchNode node = searchNodes[x, y];
                    if (node == null || node.Walkable == false)
                    {
                        continue;
                    }

                    Point[] neighbors = new Point[]
                    {
                        new Point (x, y - 1),
                        new Point (x, y + 1),
                        new Point (x - 1, y),
                        new Point (x + 1, y)
                    };

                    for (int i = 0; i < neighbors.Length; i++)
                    {
                        Point position = neighbors[i];
                        if (position.X < 0 || position.X > levelWidth - 1 || position.Y < 0 || position.Y > levelHeight - 1)
                            continue;

                        SearchNode neighbor = searchNodes[position.X, position.Y];

                        if (neighbor == null || neighbor.Walkable == false)
                            continue;

                        node.Neighbors[i] = neighbor;

                    }
                }
            }
        }

        // Holds search nodes that are avaliable to search.
        private List<SearchNode> openList = new List<SearchNode>();
        // Holds the nodes that have already been searched.
        private List<SearchNode> closedList = new List<SearchNode>();

        /// <summary>
        /// Returns an estimate of the distance between two points. (H)
        /// </summary>
        private float Heuristic(Point point1, Point point2)
        {
            return Math.Abs(point1.X - point2.X) +
                   Math.Abs(point1.Y - point2.Y);
        }

        /// <summary>
        /// Resets the state of the search nodes.
        /// </summary>
        private void ResetSearchNodes()
        {
            openList.Clear();
            closedList.Clear();

            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    SearchNode node = searchNodes[x, y];

                    if (node == null)
                    {
                        continue;
                    }

                    node.InOpenList = false;
                    node.InClosedList = false;

                    node.DistanceTraveled = float.MaxValue;
                    node.DistanceToGoal = float.MaxValue;
                }
            }
        }

        /// <summary>
        /// Returns the node with the smallest distance to goal.
        /// </summary>
        private SearchNode FindBestNode()
        {
            SearchNode currentTile = openList[0];

            float smallestDistanceToGoal = float.MaxValue;

            // Find the closest node to the goal.
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].DistanceToGoal < smallestDistanceToGoal)
                {
                    currentTile = openList[i];
                    smallestDistanceToGoal = currentTile.DistanceToGoal;
                }
            }
            return currentTile;
        }

        /// <summary>
        /// Use the parent field of the search nodes to trace
        /// a path from the end node to the start node.
        /// </summary>
        private List<SearchNode> FindFinalPath(SearchNode startNode, SearchNode endNode)
        {
            closedList.Add(endNode);

            SearchNode parentTile = endNode.Parent;

            // Trace back through the nodes using the parent fields
            // to find the best path.
            while (parentTile != startNode)
            {
                closedList.Add(parentTile);
                parentTile = parentTile.Parent;
            }

     

            List<SearchNode> finalPath = new List<SearchNode>();

            // Reverse the path and transform into world space.
            for (int i = closedList.Count - 1; i >= 0; i--)
            {
                finalPath.Add(closedList[i]);
            }

            return finalPath;
        }

        /// <summary>
        /// Finds the optimal path from one point to another.
        /// </summary>
        public List<SearchNode> FindPath(Point startPoint, Point endPoint)
        {
            // Only try to find a path if the start and end points are different.
            if (startPoint == endPoint)
            {
                return new List<SearchNode>();
            }

            /////////////////////////////////////////////////////////////////////
            // Step 1 : Clear the Open and Closed Lists and reset each node’s F 
            //          and G values in case they are still set from the last 
            //          time we tried to find a path. 
            /////////////////////////////////////////////////////////////////////
            ResetSearchNodes();

            // Store references to the start and end nodes for convenience.
            SearchNode startNode = searchNodes[startPoint.X, startPoint.Y];
            SearchNode endNode = searchNodes[endPoint.X, endPoint.Y];

            /////////////////////////////////////////////////////////////////////
            // Step 2 : Set the start node’s G value to 0 and its F value to the 
            //          estimated distance between the start node and goal node 
            //          (this is where our H function comes in) and add it to the 
            //          Open List. 
            /////////////////////////////////////////////////////////////////////
            startNode.InOpenList = true;

            startNode.DistanceToGoal = Heuristic(startPoint, endPoint);
            startNode.DistanceTraveled = 0;

            openList.Add(startNode);

            /////////////////////////////////////////////////////////////////////
            // Setp 3 : While there are still nodes to look at in the Open list : 
            /////////////////////////////////////////////////////////////////////
            while (openList.Count > 0)
            {
                /////////////////////////////////////////////////////////////////
                // a) : Loop through the Open List and find the node that 
                //      has the smallest F value.
                /////////////////////////////////////////////////////////////////
                SearchNode currentNode = FindBestNode();

                /////////////////////////////////////////////////////////////////
                // b) : If the Open List empty or no node can be found, 
                //      no path can be found so the algorithm terminates.
                /////////////////////////////////////////////////////////////////
                if (currentNode == null)
                {
                    break;
                }

                /////////////////////////////////////////////////////////////////
                // c) : If the Active Node is the goal node, we will 
                //      find and return the final path.
                /////////////////////////////////////////////////////////////////
                if (currentNode == endNode)
                {
                    // Trace our path back to the start.
                    return FindFinalPath(startNode, endNode);
                }

                /////////////////////////////////////////////////////////////////
                // d) : Else, for each of the Active Node’s neighbours :
                /////////////////////////////////////////////////////////////////
                for (int i = 0; i < currentNode.Neighbors.Length; i++)
                {
                    SearchNode neighbor = currentNode.Neighbors[i];

                    //////////////////////////////////////////////////
                    // i) : Make sure that the neighbouring node can 
                    //      be walked across. 
                    //////////////////////////////////////////////////
                    if (neighbor == null || neighbor.Walkable == false)
                    {
                        continue;
                    }

                    //////////////////////////////////////////////////
                    // ii) Calculate a new G value for the neighbouring node.
                    //////////////////////////////////////////////////
                    float distanceTraveled = currentNode.DistanceTraveled + 1;

                    // An estimate of the distance from this node to the end node.
                    float heuristic = Heuristic(neighbor.Position, endPoint);

                    //////////////////////////////////////////////////
                    // iii) If the neighbouring node is not in either the Open 
                    //      List or the Closed List : 
                    //////////////////////////////////////////////////
                    if (neighbor.InOpenList == false && neighbor.InClosedList == false)
                    {
                        // (1) Set the neighbouring node’s G value to the G value 
                        //     we just calculated.
                        neighbor.DistanceTraveled = distanceTraveled;
                        // (2) Set the neighbouring node’s F value to the new G value + 
                        //     the estimated distance between the neighbouring node and
                        //     goal node.
                        neighbor.DistanceToGoal = distanceTraveled + heuristic;
                        // (3) Set the neighbouring node’s Parent property to point at the Active 
                        //     Node.
                        neighbor.Parent = currentNode;
                        // (4) Add the neighbouring node to the Open List.
                        neighbor.InOpenList = true;
                        openList.Add(neighbor);
                    }
                    //////////////////////////////////////////////////
                    // iv) Else if the neighbouring node is in either the Open 
                    //     List or the Closed List :
                    //////////////////////////////////////////////////
                    else if (neighbor.InOpenList || neighbor.InClosedList)
                    {
                        // (1) If our new G value is less than the neighbouring 
                        //     node’s G value, we basically do exactly the same 
                        //     steps as if the nodes are not in the Open and 
                        //     Closed Lists except we do not need to add this node 
                        //     the Open List again.
                        if (neighbor.DistanceTraveled > distanceTraveled)
                        {
                            neighbor.DistanceTraveled = distanceTraveled;
                            neighbor.DistanceToGoal = distanceTraveled + heuristic;

                            neighbor.Parent = currentNode;
                        }
                    }
                }

                /////////////////////////////////////////////////////////////////
                // e) Remove the Active Node from the Open List and add it to the 
                //    Closed List
                /////////////////////////////////////////////////////////////////
                openList.Remove(currentNode);
                currentNode.InClosedList = true;
            }

            // No path could be found.
            return new List<SearchNode>();
        }
    }
}
