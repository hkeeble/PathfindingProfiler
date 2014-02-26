using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathfinder
{
    enum PathfinderAlgorithm
    {
        Dijkstra,
        AStar,
        ScentMap
    }

    abstract class Pathfinder
    {
        protected int gridSize;

        protected Map map;
        protected AiBotBase bot;
        protected Player plr;

        protected List<Coord2> path;

        public Pathfinder(int gridSize)
        {
            this.gridSize = gridSize;
            path = new List<Coord2>();
        }

        public virtual void Build(Map map, AiBotBase bot, Player player)
        {
            this.map = map;
            this.bot = bot;
            this.plr = player;
            path = new List<Coord2>();
        }

        public void Clear()
        {
            this.map = null;
            this.bot = null;
            this.plr = null;
        }

        public bool IsInPath(int x, int y)
        {
            return path.Contains(new Coord2(x, y));
        }

        public List<Coord2> Path { get { return path; } }
    }
}
