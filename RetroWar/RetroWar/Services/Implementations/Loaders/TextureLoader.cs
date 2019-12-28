using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using RetroWar.Models.Repositories.Textures;
using RetroWar.Services.Interfaces.Helpers;
using RetroWar.Services.Interfaces.Loaders;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Loaders
{
    public class TextureLoader : ITextureLoader
    {
        private readonly IStreamReader streamReader;

        public TextureLoader(IStreamReader streamReader)
        {
            this.streamReader = streamReader;
        }

        public IEnumerable<TextureDatabaseItem> LoadTextures(string texturesFileName, ContentManager Content)
        {
            var textureJsonData = streamReader.ReadFile(texturesFileName);

            var texturesToLoad = JsonConvert.DeserializeObject<IEnumerable<string>>(textureJsonData);

            var textureDatabaseItems = new List<TextureDatabaseItem>();

            foreach (var textureName in texturesToLoad)
            {
                textureDatabaseItems.Add(
                    new TextureDatabaseItem
                    {
                        TextureId = textureName,
                        Texture = Content.Load<Texture2D>(textureName)
                    });
            }

            return textureDatabaseItems;
        }
    }
}
