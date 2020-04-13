using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Services.Interfaces.Helpers.Collision;
using RetroWar.Services.Interfaces.Helpers.Model;

namespace RetroWar.Services.Implementations.Helpers.Collision
{
    public class ResolverHelper : IResolverHelper
    {
        private readonly IFaceHelper faceHelper;

        public ResolverHelper(IFaceHelper faceHelper)
        {
            this.faceHelper = faceHelper;
        }

        public bool TryResolveByFace(Sprite normal, Tile tile, Face? face)
        {
            if (face == null)
            {
                return false;
            }

            if (TileFaceFree(tile, face.Value))
            {
                Resolve(normal, tile, face.Value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TileFaceFree(Tile tile, Face face)
        {
            switch (face)
            {
                case Face.Top:
                    return !tile.HasTileAbove;
                case Face.Bottom:
                    return !tile.HasTileBelow;
                case Face.Left:
                    return !tile.HasTileToLeft;
                case Face.Right:
                    return !tile.HasTileToRight;
            }

            return false;
        }

        public void Resolve(Sprite normal, Sprite based, Face basedFace)
        {
            var difference = faceHelper.GetFaceDifference(based, basedFace, normal);

            if (basedFace == Face.Top || basedFace == Face.Left)
            {
                difference = difference * -1;
            }

            // move the vehicle (normal) face adjecent to the tile (based)
            if (basedFace == Face.Top || basedFace == Face.Bottom)
            {
                normal.OldY = normal.Y;
                normal.Y += difference;
            }
            else
            {
                normal.OldX = normal.X;
                normal.X += difference;
            }
        }
    }
}
