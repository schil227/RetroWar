using RetroWar.Exceptions.Implementations.Helpers.Model;
using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Helpers.Model;
using System;

namespace RetroWar.Services.Implementations.Helpers.Model
{
    public class FaceHelper : IFaceHelper
    {
        private readonly ISpriteHelper spriteHelper;

        public FaceHelper(ISpriteHelper spriteHelper)
        {
            this.spriteHelper = spriteHelper;
        }

        public float GetFaceAxis(Sprite sprite, Face face)
        {
            var hitbox = spriteHelper.GetHitBox(sprite);

            switch (face)
            {
                case Face.Top:
                    return sprite.Y + hitbox.RelativeY;
                case Face.Bottom:
                    return sprite.Y + hitbox.RelativeY + hitbox.Height;
                case Face.Left:
                    return sprite.X + spriteHelper.GetHitboxXOffset(sprite, hitbox.RelativeX, hitbox.Width);
                case Face.Right:
                    return sprite.X + hitbox.Width + spriteHelper.GetHitboxXOffset(sprite, hitbox.RelativeX, hitbox.Width); ;
            }

            throw new FaceHelperException("Illegal face specified. What the hell are you doing?");
        }

        // Gets the difference between the normal and based. In the context of collision detection,
        // the based face is always the opposite of the normal (e.g. we want to move normal such that
        // it's top face is adjcent to based's bottom face)
        public float GetFaceDifference(Sprite normal, Face normalFace, Sprite based)
        {
            switch (normalFace)
            {
                case Face.Top:
                    return Math.Abs(GetFaceAxis(normal, normalFace) - GetFaceAxis(based, Face.Bottom));
                case Face.Bottom:
                    return Math.Abs(GetFaceAxis(normal, normalFace) - GetFaceAxis(based, Face.Top));
                case Face.Left:
                    return Math.Abs(GetFaceAxis(normal, normalFace) - GetFaceAxis(based, Face.Right));
                case Face.Right:
                    return Math.Abs(GetFaceAxis(normal, normalFace) - GetFaceAxis(based, Face.Left));
            }

            throw new FaceHelperException("Illegal face specified. What the hell are you doing?");
        }
    }
}
