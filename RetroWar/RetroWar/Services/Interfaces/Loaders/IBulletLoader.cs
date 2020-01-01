using RetroWar.Models.Repositories.Bullets;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.Loaders
{
    public interface IBulletLoader
    {
        IEnumerable<BulletDatabaseItem> LoadBullets(string tileFileName);
    }
}
