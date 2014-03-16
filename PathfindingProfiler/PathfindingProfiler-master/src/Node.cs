using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathfinder
{
    class Node
    {
        public bool closed;
        public float cost;
        public Coord2 link;
        public bool inPath;

        public const int INITIAL_COST = 1000000;

        public Node()
        {
            closed = false;
            cost = INITIAL_COST;
            inPath = false;
            link = new Coord2(-1, -1);
        }
    }

    class NodeCollection
    {
        private Node[,] nodes;

        public NodeCollection(int gridSize)
        {
            nodes = new Node[gridSize, gridSize];
            for (int x = 0; x < gridSize; x++)
                for (int y = 0; y < gridSize; y++)
                    nodes[x, y] = new Node();
        }

        public Node Get(int x, int y)
        {
            return nodes[x, y];
        }

        public void Clear()
        {
            for (int x = 0; x < nodes.GetLength(0); x++)
                for (int y = 0; y < nodes.GetLength(1); y++)
                    nodes[x, y] = new Node(); 
        }
    }
}
