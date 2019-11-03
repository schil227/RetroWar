using Microsoft.Xna.Framework;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.HitBoxes;
using RetroWar.Models.Sprites.Textures;
using RetroWar.Services.Interfaces.Helpers.Model;
using System;
using System.Linq;

namespace RetroWar.Services.Implementations.Helpers.Model
{
    public class SpriteHelper : ISpriteHelper
    {
        public HitBox[] GetCurrentHitBoxes(Sprite sprite)
        {
            return sprite.ActionDataSet.First(a => a.Action == sprite.CurrentAction)
                        .ActionHitBoxSet.ElementAt(sprite.CurrentSequence).HitBoxes.ToArray();
        }

        public TextureData[] GetCurrentTextureData(Sprite sprite)
        {
            return sprite.ActionDataSet.First(a => a.Action == sprite.CurrentAction)
                            .ActionTextureSet.ElementAt(sprite.CurrentSequence).TextureData.ToArray();
        }

        public Point GetMaximumPoints(Sprite sprite)
        {
            var point = new Point
            {
                X = 0,
                Y = 0
            };

            var hitBoxes = GetCurrentHitBoxes(sprite);
            var textures = GetCurrentTextureData(sprite);

            foreach (var hitbox in hitBoxes)
            {
                point.X = Math.Max(point.X, (int)sprite.X + (hitbox.RelativeX * 16) + hitbox.Width);
                point.Y = Math.Max(point.Y, (int)sprite.Y + (hitbox.RelativeY * 16) + hitbox.Height);
            }

            foreach (var texture in textures)
            {
                point.X = Math.Max(point.X, (int)sprite.X + (texture.RelativeX * 16) + texture.Width);
                point.Y = Math.Max(point.Y, (int)sprite.Y + (texture.RelativeY * 16) + texture.Height);
            }

            return point;
        }
    }
}
