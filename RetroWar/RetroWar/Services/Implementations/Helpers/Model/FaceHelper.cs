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

        public int GetFaceAxis(Sprite sprite, Face face)
        {
            var hitbox = spriteHelper.GetHitBox(sprite);

            switch (face)
            {
                case Face.Top:
                    //return sprite.X
                    break;
                case Face.Bottom:
                    break;
                case Face.Left:
                    break;
                case Face.Right:
                    break;
            }

            return 0;
        }

        public int GetFaceDifference(Sprite normal, Face normalFace, Sprite based)
        {
            throw new NotImplementedException();
        }
    }
}
