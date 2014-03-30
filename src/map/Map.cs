
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace Pathfinder
{
    /// <summary>
    /// Represents a map loaded from an ASCII text file.
    /// </summary>
    public class Map
    {
        private int gridSize;
        public Tile [,] tiles;

        private int tileDimension;
        private Texture2D tile1Texture;
        private Texture2D tile2Texture;

        public Pathfinder pathfinder;
        private PathfinderAlgorithm algorithm;

        private string name;

        public string Name { get { return name; } }

        /// <summary>
        /// Constructor initializes tile array and tile textures.
        /// </summary>
        /// <param name="tile1Texture">Texture for floor tile.</param>
        /// <param name="tile2Texture">Texture for blocked/wall tile.</param>
        public Map(Texture2D tile1Texture, Texture2D tile2Texture)
        {
            tiles = new Tile[gridSize, gridSize];
            for (int i = 0; i < gridSize; i++)
                for (int j = 0; j < gridSize; j++)
                    tiles[i,j] = new Tile();

            pathfinder = null;

            this.tile1Texture = tile1Texture;
            this.tile2Texture = tile2Texture;
            tileDimension = tile1Texture.Width;
        }

        /// <summary>
        /// Set the current pathfinding algorithm.
        /// </summary>
        /// <param name="algorithm">The algorithm to use in pathfinding.</param>
        public void SetPathfinder(PathfinderAlgorithm algorithm)
        {
            this.algorithm = algorithm;
            pathfinder = PathfinderFactory.CreatePathfinder(algorithm, gridSize, this);
            Console.WriteLine("Map.cs: Pathfinding algorithm set to " + pathfinder.GetName() + ".\n");
        }

        // Get Accessors
        public int GridSize { get { return gridSize; } }
        public Texture2D Tile1Texture { get { return tile1Texture; } }
        public Texture2D Tile2Texture { get { return tile2Texture; } }
        public int TileSize { get { return tileDimension; } }
        public PathfinderAlgorithm PathfindingAlgorithm { get { return algorithm; } }

        /// <summary>
        /// Returns whether or not a coordinate is a valid position in the map.
        /// </summary>
        /// <param name="pos">The position to check for validity.</param>
        /// <returns></returns>
        public bool ValidPosition(Coord2 pos)
        {
            if (!IsWithinMap(pos)) return false;
            if (IsBlocked(pos))    return false;
            else                   return true;
        }

        /// <summary>
        /// Returns whether or not the given position is blocked. Does not ensure given positon is within the map. Returns false if position is
        /// out of map bounds.
        /// </summary>
        /// <param name="pos">Position to check for block</param>
        /// <returns></returns>
        public bool IsBlocked(Coord2 pos)
        {
            if(IsWithinMap(pos))
                return (tiles[pos.X, pos.Y].IsBlocked);
            else
                return false;
        }

        /// <summary>
        /// Integer override for <c>IsBlocked(Coord2 pos)</c>.
        /// </summary>
        /// <see cref="Pathfinder.Map.IsBlocked(Coord2 pos)"/>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <returns></returns>
        public bool IsBlocked(int x, int y)
        {
            return IsBlocked(new Coord2(x, y));
        }

        public bool IsWithinMap(Coord2 pos)
        {
            if (pos.X < 0)          return false;
            if (pos.X >= gridSize)  return false;
            if (pos.Y < 0)          return false;
            if (pos.Y >= gridSize)  return false;
            else                    return true;
        }

        /// <summary>
        /// Loads a map from a given text file.
        /// </summary>
        /// <param name="path">File path of the map file to load.</param>
        public void Loadmap(string path)
        {
            string[] sections = path.Split('/');
            name = sections[sections.Length-1];
            name.TrimEnd(new char[] { '.', 'm', 'a', 'p' });

            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(path))
            {
                // Read in the size of the map
                string line = reader.ReadLine();
                gridSize = line.Length;
                tiles = new Tile[gridSize, gridSize];
                for (int i = 0; i < gridSize; i++)
                    for (int j = 0; j < gridSize; j++)
                        tiles[i, j] = new Tile();

                Debug.Assert(line.Length == gridSize, String.Format("loaded map string line width must be {0}.", gridSize));
                while (line != null)
                {
                    lines.Add(line);
                    line = reader.ReadLine();
                }
            }
            Debug.Assert(lines.Count == gridSize, String.Format("loaded map string must have {0} lines.",gridSize));

            // Loop over every tile position,
            for (int y = 0; y < gridSize; ++y)
            {
                for (int x = 0; x < gridSize; ++x)
                {
                    // to load each tile.
                    char tileType = lines[y][x];
                    if (tileType == '.') tiles[x, y] = new Tile(false);
                    else tiles[x, y] = new Tile(true);
                }
            }
        }

        /// <summary>
        /// Draws the map.
        /// </summary>
        /// <param name="sb">The spritebatch to use.</param>
        public void Draw(SpriteBatch sb)
        {
            for(int x = 0; x < gridSize; x++)
            {
                for(int y = 0 ; y < gridSize; y++)
                {
                    if(tiles[x,y].IsBlocked)
                        sb.Draw(tile2Texture, new Vector2(x*tileDimension, y*tileDimension), tiles[x,y].RenderColor);
                    else
                        sb.Draw(tile1Texture, new Vector2(x*tileDimension, y*tileDimension), tiles[x,y].RenderColor);
                }
            }

            pathfinder.DrawPath(sb);
        }

        /// <summary>
        /// Sets the render color of the given tile. Useful for visualization of a pathfinding algorithm.
        /// </summary>
        /// <param name="pos">Position of the tile to set.</param>
        /// <param name="color">Color to draw the tile as.</param>
        public void SetRenderColor(Coord2 pos, Color color)
        {
            if(ValidPosition(pos))
                tiles[pos.X, pos.Y].SetRenderColor(color);
        }

        /// <summary>
        /// Clears the render colors for this map, sets all tiles to default render color.
        /// </summary>
        public void ClearColor()
        {
            for(int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                    tiles[x, y].SetRenderColor(Color.White);
            }
        }
    }
}
