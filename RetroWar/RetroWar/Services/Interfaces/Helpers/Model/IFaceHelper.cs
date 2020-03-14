using RetroWar.Models.Sprites;

namespace RetroWar.Services.Interfaces.Helpers.Model
{
    public interface IFaceHelper
    {
        int GetFaceAxis(Sprite sprite, Face face);
        int GetFaceDifference(Sprite normal, Face normalFace, Sprite based);
    }
}
