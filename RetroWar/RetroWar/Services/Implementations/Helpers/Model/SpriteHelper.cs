using Microsoft.Xna.Framework;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.HitBoxes;
using RetroWar.Models.Sprites.Textures;
using RetroWar.Services.Interfaces.Helpers.Model;
using System;
using System.Linq;
using Action = RetroWar.Models.Sprites.Actions.Action;

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
                .ActionTextureSet.ElementAt(0).TextureData.ToArray();
            // check out this sick hack.   ^^^
        }

        public int GetCurrentEvent(Sprite sprite)
        {
            return sprite.ActionDataSet.First(a => a.Action == sprite.CurrentAction)
                .Events.ElementAt(sprite.CurrentSequence);
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

        public void UpdateActionSequence(Sprite sprite, float deltaTimeTick)
        {
            var currentAction = sprite.ActionDataSet.First(a => a.Action == sprite.CurrentAction);
            sprite.TickAccumulation += deltaTimeTick;

            var currentTickDuration = currentAction.SequenceDurations.ElementAt(sprite.CurrentSequence);

            while (sprite.TickAccumulation > currentTickDuration)
            {
                IncrementSequence(sprite);
                sprite.TickAccumulation = sprite.TickAccumulation - currentTickDuration;
            }
        }

        public void IncrementSequence(Sprite sprite)
        {
            sprite.CurrentSequence++;
            var currentAction = sprite.ActionDataSet.First(a => a.Action == sprite.CurrentAction);

            if (sprite.CurrentSequence >= currentAction.TotalSequences)
            {
                sprite.CurrentSequence = 0;

                if (!currentAction.IsContinuous)
                {
                    SetAction(sprite, Action.Idle);
                }
            }

            var eventId = GetCurrentEvent(sprite);

            if (eventId != 0)
            {
                //Trigger action events here
            }
        }

        public void SetAction(Sprite sprite, Action action)
        {
            sprite.CurrentSequence = 0;
            sprite.TickAccumulation = 0;
            sprite.CurrentAction = action;
        }
    }
}
