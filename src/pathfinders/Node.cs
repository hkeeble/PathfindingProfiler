/*
 * File: Node.cs
 * 
 * Author: Henri Keeble
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines a class that represents an individual node in a pathfinder, and a collection class used to manage nodes.
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathfinder
{
    /// <summary>
    /// Represents an individual node within a pathfinder.
    /// </summary>
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

        /// <summary>
        /// Create a new node.
        /// </summary>
        /// <param name="location">The coordinate of the node.</param>
        public Node(Coord2 location) : this()
        {
            this.position = location;
        }
    }

    /// <summary>
    /// Represents a collection of nodes.
    /// </summary>
    class NodeCollection
    {
        private Node[,] nodes;
        private int gridSize;

        /// <summary>
        /// Create a new collection of nodes.
        /// </summary>
        /// <param name="gridSize">The size of the node grid.</param>
        public NodeCollection(int gridSize)
        {
            nodes = new Node[gridSize, gridSize];
            this.gridSize = gridSize;
            for (int x = 0; x < gridSize; x++)
                for (int y = 0; y < gridSize; y++)
                    nodes[x, y] = new Node(new Coord2(x, y));
        }

        /// <summary>
        /// Check if the given location is a valid one within this node collection.
        /// </summary>
        public bool IsValid(Coord2 loc)
        {
            if (loc.X < 0) return false;
            else if (loc.X >= gridSize) return false;
            else if (loc.Y < 0) return false;
            else if (loc.Y >= gridSize) return false;
            else return true;
        }

        /// <summary>
        /// Check if the given location is a valid one within this node collection.
        /// </summary>
        public bool IsValid(int x, int y)
        {
            if (x < 0) return false;
            else if (x >= gridSize) return false;
            else if (y < 0) return false;
            else if (y >= gridSize) return false;
            else return true;
        }

        /// <summary>
        /// Retrieve the node at the given location, if it is valid.
        /// </summary>
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

        /// <summary>
        /// Retrieve the node at the given location, if it is valid.
        /// </summary>
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

        /// <summary>
        /// Clears the current node collection.
        /// </summary>
        public void Clear()
        {
            for (int x = 0; x < nodes.GetLength(0); x++)
                for (int y = 0; y < nodes.GetLength(1); y++)
                    nodes[x, y] = new Node(); 
        }
    }
}
