using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Tiles;

namespace RetroWar.Services.Interfaces.Helpers.Collision
{
    public interface IResolverHelper
    {
        bool TryResolveByFace(Sprite normal, Tile tile, Face? face);
        bool TileFaceFree(Tile tile, Face face);
        void Resolve(Sprite normal, Sprite based, Face basedFace);
        Direction DirectionToResolveIn(Face basedFace, bool isNormalSprite);
    }
}
