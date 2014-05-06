/*
 * File: Pathfinder.cs
 * 
 * Author: Henri Keeble
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines an abstract pathfinder such that the program can make use of polymorphism for clearer code.
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinder
{
    /// <summary>
    /// Represents different pathfinding algorithms in an enumeration.
    /// </summary>
    public enum PathfinderAlgorithm
    {
        Dijkstra,
        AStar,
        ScentMap
    }

    /// <summary>
    /// Abstract pathfinder class.
    /// </summary>
    public abstract class Pathfinder
    {
        public abstract void                Build(Coord2 startPos, Coord2 targetPos, bool testMode = false);
        public abstract void                Clear();
        public abstract void                DrawPath(SpriteBatch sb);
        public abstract bool                IsInPath(int x, int y);
        public abstract bool                IsClosed(int x, int y);
        public abstract bool                IsClosed(Coord2 coord);
        public abstract string              GetName();
        public abstract PathfinderAlgorithm GetAlgorithm();
        public abstract int                 NodesSearched();
        public abstract List<Coord2>        GetPath();
    }

    /// <summary>
    /// Factory class used to instantiate pathfinders.
    /// </summary>
    static class PathfinderFactory
    {
        /// <summary>
        /// Create a new pathfinder of the given type.
        /// </summary>
        /// <param name="algorithm">The type of algorithm the pathfinder should use.</param>
        /// <param name="gridSize">The grid size the pathfinder is to work with.</param>
        /// <returns></returns>
        public static Pathfinder CreatePathfinder(PathfinderAlgorithm algorithm, int gridSize, Map map)
        {
            switch (algorithm)
            {
                case PathfinderAlgorithm.Dijkstra:
                    return (Pathfinder)new Dijkstra(gridSize, map);
                case PathfinderAlgorithm.AStar:
                    return (Pathfinder)new AStar(gridSize, map);
                case PathfinderAlgorithm.ScentMap:
                    return (Pathfinder)new ScentMap(gridSize, map);
                default:
                    Console.WriteLine("PathfinderFactory: Attempted to create unrecognized pathfinder type. Returning Dijkstra.\n");
                    return (Pathfinder)new Dijkstra(gridSize, map);
            }
        }
    }
}
