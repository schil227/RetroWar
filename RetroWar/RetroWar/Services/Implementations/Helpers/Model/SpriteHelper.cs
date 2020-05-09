using Microsoft.Xna.Framework;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Actions;
using RetroWar.Models.Sprites.HitBoxes;
using RetroWar.Models.Sprites.Textures;
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

        public HitBox GetHitBox(Sprite sprite)
        {
            foreach (var action in sprite.CurrentActions)
            {
                var hitBoxSet = sprite.ActionDataSet.First(a => a.Action == action).ActionHitBoxSet;

                if (hitBoxSet.Count() == 0 || hitBoxSet.First().IsEmptyHitbox)
                {
                    continue;
                }

                return hitBoxSet.ElementAt(sprite.CurrentActionSequence[action]);
            }

            return null;
        }

        public IDictionary<TextureData, Models.Sprites.Actions.Action> GetCurrentTextureData(Sprite sprite)
        {
            var textureData = new Dictionary<TextureData, Models.Sprites.Actions.Action>();

            foreach (var action in sprite.CurrentActions)
            {
                var textures = sprite.ActionDataSet.First(a => a.Action == action).ActionTextureSet.ElementAt(0).TextureData;

                foreach (var texture in textures)
                {
                    textureData.Add(texture, action);
                }
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

            var hitBox = GetHitBox(sprite);
            var textures = GetCurrentTextureData(sprite);

            if (hitBox != null)
            {
                point.X = Math.Max(point.X, spriteX + (hitBox.RelativeX) + hitBox.Width);
                point.Y = Math.Max(point.Y, spriteY + (hitBox.RelativeY) + hitBox.Height);
            }

            foreach (var texture in textures)
            {
                point.X = Math.Max(point.X, spriteX + (texture.Key.RelativeX * 16) + texture.Key.Width);
                point.Y = Math.Max(point.Y, spriteY + (texture.Key.RelativeY * 16) + texture.Key.Height);
            }

            return point;
        }

        public Point GetMaximumHitboxPoints(Sprite sprite)
        {
            var point = new Point();
            var hitbox = GetHitBox(sprite);

            point.X = (int)sprite.X + GetHitboxXOffset(sprite, hitbox.RelativeX, hitbox.Width) + hitbox.Width;
            point.Y = (int)sprite.Y + hitbox.RelativeY + hitbox.Height;

            return point;
        }
    }
}
