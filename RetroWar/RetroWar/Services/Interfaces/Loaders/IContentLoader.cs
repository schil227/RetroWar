﻿using Microsoft.Xna.Framework.Content;
using RetroWar.Models.Repositories;

namespace RetroWar.Services.Interfaces.Loaders
{
    public interface IContentLoader
    {
        ContentDatabase LoadAllData(
            ContentManager Content,
            string spriteFileName,
            string actionDataFileName,
            string textureFileName,
            string tileFileName
            );
    }
}
