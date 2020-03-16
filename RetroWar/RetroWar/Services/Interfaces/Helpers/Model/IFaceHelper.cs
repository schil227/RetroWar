using RetroWar.Models.Sprites;

namespace RetroWar.Services.Interfaces.Helpers.Model
{
    public interface IFaceHelper
    {
        float GetFaceAxis(Sprite sprite, Face face);
        float GetFaceDifference(Sprite normal, Face normalFace, Sprite based);
    }
}
