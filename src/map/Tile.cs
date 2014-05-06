/*
 * File: Tile.cs
 * 
 * Program: Pathfinding Profiler
 * 
 * Desc: Declares and defines a tile classed used by the map.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pathfinder
{
    /// <summary>
    /// The <c>Tile</c> class is used by the <c>Map</c> <see cref="Pathfinder.Map"/>
    /// </summary>
    public class Tile
    {
        private bool blocked;
        private Color renderColor;

        public Tile()
        {
            blocked = false;
            renderColor = Color.White;
        }

        public Tile(bool isBlocked) : this()
        {
            blocked = isBlocked;
        }

        /// <summary>
        /// Set the color to render this tile as.
        /// </summary>
        /// <param name="color">The color to render the tile as.</param>
        public void SetRenderColor(Color color)
        {
            renderColor = color;
        }

        /// <summary>
        /// Toggle whether or not this tile is blocked.
        /// </summary>
        public void ToggleBlock()
        {
            blocked = !blocked;
        }

        public bool IsBlocked { get { return blocked; } }
        public Color RenderColor { get { return renderColor; } }
    }
}
