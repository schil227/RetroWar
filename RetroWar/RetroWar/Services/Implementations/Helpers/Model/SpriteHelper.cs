using Microsoft.Xna.Framework;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Actions;
using RetroWar.Models.Sprites.HitBoxes;
using RetroWar.Models.Sprites.Textures;
using RetroWar.Services.Interfaces.Helpers.Model;
using System;
using System.Linq;

namespace RetroWar.Services.Implementations.Helpers.Model
{
    public class SpriteHelper : ISpriteHelper
    {
        public ActionData GetCurrentActionData(Sprite sprite)
        {
            return sprite.ActionDataSet.First(a => a.Action == sprite.CurrentAction);
        }

        public HitBox[] GetCurrentHitBoxes(Sprite sprite)
        {
            return sprite.ActionDataSet.First(a => a.Action == sprite.CurrentAction)
                .ActionHitBoxSet.ElementAt(sprite.CurrentSequence).HitBoxes.ToArray();
        }

        public TextureData[] GetCurrentTextureData(Sprite sprite)
        {
            return sprite.ActionDataSet.First(a => a.Action == sprite.CurrentAction)
                .ActionTextureSet.ElementAt(0).TextureData.ToArray();
            // check out this sick hack.   ^^^
        }

        public int GetHitboxXOffset(Sprite sprite, int currentXOffset, int hitBoxWidth)
        {
            // Since images are right-facing, when they are flipped due to the direction,
            // the hitbox must be moved to fit the new graphic.
            if (sprite.CurrentDirection == Direction.Left)
            {
                return (GetCurrentActionData(sprite).TextureTileWidthX * 16) - hitBoxWidth - currentXOffset;
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
