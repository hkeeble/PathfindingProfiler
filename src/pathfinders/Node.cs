using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathfinder
{
    public class Node
    {
        public Coord2 position;
        public Node parent;
        public float cost;
        public bool closed;
        public bool inPath;

        public const int INITIAL_COST = 1000000;

        public Node()
        {
            cost = INITIAL_COST;
            inPath = false;
            closed = false;
            parent = null;
            position = new Coord2(-1, -1);
        }

        public Node(Coord2 location) : this()
        {
            this.position = location;
        }
    }

    class NodeCollection
    {
        private Node[,] nodes;
        private int gridSize;

        public NodeCollection(int gridSize)
        {
            nodes = new Node[gridSize, gridSize];
            this.gridSize = gridSize;
            for (int x = 0; x < gridSize; x++)
                for (int y = 0; y < gridSize; y++)
                    nodes[x, y] = new Node(new Coord2(x, y));
        }

        public bool IsValid(Coord2 loc)
        {
            if (loc.X < 0) return false;
            else if (loc.X >= gridSize) return false;
            else if (loc.Y < 0) return false;
            else if (loc.Y >= gridSize) return false;
            else return true;
        }

        public bool IsValid(int x, int y)
        {
            if (x < 0) return false;
            else if (x >= gridSize) return false;
            else if (y < 0) return false;
            else if (y >= gridSize) return false;
            else return true;
        }

        public Node Get(Coord2 loc)
        {
            if(IsValid(loc))
                return nodes[loc.X, loc.Y];
            else
            {
                Console.WriteLine("Error, tried to access invalid node in node collection!\n");
                return null;
            }
        }

        public Node Get(int x, int y)
        {
            if(IsValid(x, y))
                return nodes[x, y];
            else
            {
                Console.WriteLine("Error, tried to access invalid node in node collection!\n");
                return null;
            }
        }

        public void Clear()
        {
            for (int x = 0; x < nodes.GetLength(0); x++)
                for (int y = 0; y < nodes.GetLength(1); y++)
                    nodes[x, y] = new Node(); 
        }
    }
}
