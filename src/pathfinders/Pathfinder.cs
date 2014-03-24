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
        JPS,
        ScentMap
    }

    /// <summary>
    /// Represents a single set of profiling data.
    /// </summary>
    struct Profile
    {
        private string name;
        private long startTicks;
        private long endTicks;
        private long profileTicks;

        public Profile(string name)
        {
            this.name = name;
            startTicks = endTicks = profileTicks = 0;
        }

        public void Start(GameTime gameTime)
        {
            startTicks = gameTime.TotalGameTime.Ticks;
            Console.WriteLine("Profile for:" + name + "started at " + DateTime.Now.ToLongTimeString());
        }

        public void End(GameTime gameTime)
        {
            endTicks = gameTime.TotalGameTime.Ticks;
            Console.WriteLine("Profile for:" + name + "finished at " + DateTime.Now.ToLongTimeString());
            profileTicks = endTicks - startTicks;
            Console.WriteLine("This profile took " + profileTicks + " ticks to complete.");
        }

        public long MillisecondsTaken { get { return profileTicks; } }
    }

    /// <summary>
    /// Interface implemented by all pathfinder classes.
    /// </summary>
    public interface IPathfinder
    {
        void Build(Coord2 startPos, Coord2 targetPos);
        void Clear();
        bool IsInPath(int x, int y);
        string GetName();
        List<Coord2> GetPath();
        void DrawPath(SpriteBatch sb);

        // Usually used to retrieve data required for visual representation. These will have very different implementations for each pathfinder.
        int GetValue(int x, int y);
        int HighestValue();
        int LowestValue();
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
        public static IPathfinder CreatePathfinder(PathfinderAlgorithm algorithm, int gridSize, Map map)
        {
            switch (algorithm)
            {
                case PathfinderAlgorithm.Dijkstra:
                    return (IPathfinder)new Dijkstra(gridSize, map);
                case PathfinderAlgorithm.AStar:
                    return (IPathfinder)new AStar(gridSize, map);
                case PathfinderAlgorithm.JPS:
                    return (IPathfinder)new JPS(gridSize, map);
                default:
                    Console.WriteLine("PathfinderFactory: Attempted to create unrecognized pathfinder type. Returning Dijkstra.\n");
                    return (IPathfinder)new Dijkstra(gridSize, map);
            }
        }
    }
}
