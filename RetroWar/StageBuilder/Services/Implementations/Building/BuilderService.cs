using Newtonsoft.Json;
using RetroWar.Models.Level;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Services.Interfaces.Collision.Grid;
using StageBuilder.Model.UI;
using StageBuilder.Services.Interfaces.Building;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StageBuilder.Services.Implementations.Building
{
    public class BuilderService : IBuilderService
    {
        private static IDictionary<Tuple<int, int>, Tile> ExistingTiles;
        private readonly IGridService gridService;

        public BuilderService(IGridService gridService)
        {
            this.gridService = gridService;

            ExistingTiles = new Dictionary<Tuple<int, int>, Tile>();
        }

        public void AddTileToStage(ConstructionData constructionData)
        {
            var masterTile = constructionData.Tiles.ElementAt(constructionData.TileIndex);
            var masterJson = JsonConvert.SerializeObject(masterTile);
            var tile = JsonConvert.DeserializeObject<Tile>(masterJson);

            tile.SpriteId = $"{Guid.NewGuid().ToString()}_{masterTile.SpriteId}";
            tile.X = constructionData.Cursor.X;
            tile.Y = constructionData.Cursor.Y;
            tile.CurrentDirection = RetroWar.Models.Sprites.Direction.Right;

            Tile existingTile;

            ExistingTiles.TryGetValue(new Tuple<int, int>((int)tile.X, (int)tile.Y), out existingTile);

            if (existingTile != null)
            {
                constructionData.Stage.Tiles.Remove(existingTile);
                gridService.RemoveSpriteFromGrid(constructionData.Stage.Grids, existingTile);
                ExistingTiles.Remove(new Tuple<int, int>((int)existingTile.X, (int)existingTile.Y));
            }

            constructionData.Stage.Tiles.Add(tile);
            gridService.AddSpriteToGrid(constructionData.Stage.Grids, tile);
            ExistingTiles.Add(new Tuple<int, int>((int)tile.X, (int)tile.Y), tile);

            UpdateTileRelations(constructionData.Stage, tile, true);
        }

        public void RemoveTileFromStage(ConstructionData constructionData)
        {
            Tile existingTile;

            ExistingTiles.TryGetValue(new Tuple<int, int>((int)constructionData.Cursor.X, (int)constructionData.Cursor.Y), out existingTile);

            if (existingTile != null)
            {
                constructionData.Stage.Tiles.Remove(existingTile);
                gridService.RemoveSpriteFromGrid(constructionData.Stage.Grids, existingTile);
                ExistingTiles.Remove(new Tuple<int, int>((int)existingTile.X, (int)existingTile.Y));

                UpdateTileRelations(constructionData.Stage, existingTile, true);
            }
        }

        private void UpdateTileRelations(Stage stage, Tile tile, bool isNewTile)
        {
            if (tile == null)
            {
                return;
            }

            var tileAbove = GetNeighboringTile(stage, tile.X, tile.Y - 16);
            var tileBelow = GetNeighboringTile(stage, tile.X, tile.Y + 16);
            var tileToLeft = GetNeighboringTile(stage, tile.X - 16, tile.Y);
            var tileToRight = GetNeighboringTile(stage, tile.X + 16, tile.Y);

            tile.HasTileAbove = tileAbove != null;
            tile.HasTileBelow = tileBelow != null;
            tile.HasTileToLeft = tileToLeft != null;
            tile.HasTileToRight = tileToRight != null;

            if (!isNewTile)
            {
                return;
            }

            UpdateTileRelations(stage, tileAbove, false);
            UpdateTileRelations(stage, tileBelow, false);
            UpdateTileRelations(stage, tileToLeft, false);
            UpdateTileRelations(stage, tileToRight, false);

        }

        private Tile GetNeighboringTile(Stage stage, float x, float y)
        {
            foreach (var tile in stage.Tiles)
            {
                //var bX = tile.X;
                //var bMaxX = tile.X + 16;

                //var bY = tile.Y;
                //var bMaxY = tile.Y + 16;

                //if (bX <= x && x < bMaxX &&
                //    bY <= y && y < bMaxY)
                //{
                //    return tile;
                //}

                if (tile.X == x && tile.Y == y)
                {
                    return tile;
                }
            }

            return null;
        }
    }
}
