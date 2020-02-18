using Microsoft.Xna.Framework;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Actions;
using RetroWar.Models.Sprites.HitBoxes;
using RetroWar.Models.Sprites.Textures;
using RetroWar.Models.Vehicles.Vehicles.PlayerVehicle;
using RetroWar.Services.Interfaces.Helpers.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetroWar.Services.Implementations.Helpers.Model
{
    public class SpriteHelper : ISpriteHelper
    {
        public IEnumerable<ActionData> GetCurrentActionData(Sprite sprite)
        {
            return sprite.ActionDataSet.Where(a => sprite.CurrentActions.Contains(a.Action));
        }

        public IEnumerable<HitBox> GetCurrentHitBoxes(Sprite sprite)
        {
            var hitBoxes = new List<HitBox>();

            foreach (var action in sprite.CurrentActions)
            {
                var hitBoxSet = sprite.ActionDataSet.First(a => a.Action == action).ActionHitBoxSet;

                if (hitBoxSet.Count() == 0)
                {
                    continue;
                }

                hitBoxes.AddRange(hitBoxSet.ElementAt(sprite.CurrentActionSequence[action]).HitBoxes);
            }

            return hitBoxes;
        }

        public IEnumerable<TextureData> GetCurrentTextureData(Sprite sprite)
        {
            var textureData = new List<TextureData>();

            if (sprite is PlayerVehicle && sprite.CurrentActions.Contains(Models.Sprites.Actions.Action.Move))
            {
                Console.Write("m");
            }

            foreach (var action in sprite.CurrentActions)
            {
                textureData.AddRange(
                        sprite.ActionDataSet
                        .First(a => a.Action == action)
                        .ActionTextureSet.ElementAt(0).TextureData
                    );
            }

            return textureData;
        }

        public int GetHitboxXOffset(Sprite sprite, int currentXOffset, int hitBoxWidth)
        {
            // Since images are right-facing, when they are flipped due to the direction,
            // the hitbox must be moved to fit the new graphic.
            if (sprite.CurrentDirection == Direction.Left)
            {
                var textureOffset = GetCurrentActionData(sprite).FirstOrDefault(a => a.TextureTileWidthX != 0)?.TextureTileWidthX ?? 0;

                return (textureOffset * 16) - hitBoxWidth - currentXOffset;
            }

            return currentXOffset;
        }

        public Point GetMaximumPoints(Sprite sprite, int spriteX, int spriteY)
        {
            var point = new Point
            {
                X = int.MinValue,
                Y = int.MinValue
            };

            var hitBoxes = GetCurrentHitBoxes(sprite);
            var textures = GetCurrentTextureData(sprite);

            foreach (var hitbox in hitBoxes)
            {
                point.X = Math.Max(point.X, spriteX + (hitbox.RelativeX) + hitbox.Width);
                point.Y = Math.Max(point.Y, spriteY + (hitbox.RelativeY) + hitbox.Height);
            }

            foreach (var texture in textures)
            {
                point.X = Math.Max(point.X, spriteX + (texture.RelativeX * 16) + texture.Width);
                point.Y = Math.Max(point.Y, spriteY + (texture.RelativeY * 16) + texture.Height);
            }

            return point;
        }
    }
}
