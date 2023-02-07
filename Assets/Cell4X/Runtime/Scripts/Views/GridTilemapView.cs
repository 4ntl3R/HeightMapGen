using System.Collections.Generic;
using Cell4X.Runtime.Scripts.Data;
using Cell4X.Runtime.Scripts.Extensions;
using Cell4X.Runtime.Scripts.Views.Interfaces;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Cell4X.Runtime.Scripts.Views
{
    public class GridTilemapView : IGridView
    {
        private readonly Tilemap _tilemap;
        private readonly Tile _baseTile;
        private readonly Dictionary<int, Color> _tileColors;
        
        public GridTilemapView(Tilemap tilemap, Tile baseTile, HeightColorsData tileColors)
        {
            _tilemap = tilemap;
            _baseTile = baseTile;
            _tileColors = tileColors.GetColorDictionary;
        }
        
        public void DrawGrid(int[,] grid)
        {
            var gridSize = grid.GetMatrixSize();
            for (var x = 0; x < gridSize.x; x++)
            {
                for (var y = 0; y < gridSize.y; y++)
                {
                    var position = new Vector3Int(x, y, 0);
                    _tilemap.SetTile(position, _baseTile);
                    _tilemap.SetTileFlags(position, TileFlags.None);
                    _tilemap.SetColor(position, _tileColors[grid[x, y]]);
                }
            }
        }
    }
}
