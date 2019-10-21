using Newtonsoft.Json;
using RetroWar.Exceptions.Implementations.Loaders;
using RetroWar.Models.Repositories.Sprites;
using RetroWar.Services.Interfaces.Helpers;
using RetroWar.Services.Interfaces.Loaders;
using System.Linq;

namespace RetroWar.Services.Implementations.Loaders
{
    internal class SpriteLoader : ISpriteLoader
    {
        private readonly IStreamHelper streamHelper;

        public SpriteLoader(IStreamHelper streamHelper)
        {
            this.streamHelper = streamHelper;
        }

        public SpriteDatabaseItem[] LoadSprites(string SpriteFileName)
        {
            var spriteLoaderJson = streamHelper.ReadFile(SpriteFileName);

            var actionData = JsonConvert.DeserializeObject<SpriteDatabaseItem[]>(spriteLoaderJson);

            var duplicateIds = actionData.GroupBy(a => a.SpriteId).Where(g => g.Count() > 1)?.Select(i => i.Key);

            if (duplicateIds?.Count() > 0)
            {
                throw new SpriteLoaderException($"Duplicate IDs found when loading Sprites. Ids: {string.Join(",", duplicateIds)}");
            }

            return actionData;
        }
    }
}
