using RetroWar.Models.Sprites;
using RetroWar.Models.Vehicles.Vehicles.EnemyVehicle;
using RetroWar.Services.Interfaces.Actions;
using RetroWar.Services.Interfaces.AI;
using RetroWar.Services.Interfaces.Updaters;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Updaters
{
    public class EnemyUpdater : ISpriteUpdater
    {
        private readonly IBehaviorProcessor behivorProcessorComposite;
        private readonly ISequenceService sequenceService;

        public EnemyUpdater
            (
            IBehaviorProcessor behivorProcessorComposite,
            ISequenceService sequenceService
            )
        {
            this.behivorProcessorComposite = behivorProcessorComposite;
            this.sequenceService = sequenceService;
        }

        public bool UpdateSprite(Sprite sprite, float deltaTime, Dictionary<string, string> processedSprites)
        {
            EnemyVehicle enemyVehicle = null;

            if (sprite is EnemyVehicle)
            {
                enemyVehicle = (EnemyVehicle)sprite;
            }
            else
            {
                return false;
            }

            sequenceService.UpdateActionSequence(enemyVehicle, deltaTime * 1000);

            behivorProcessorComposite.ProcessBehavior(enemyVehicle, deltaTime);

            processedSprites.Add(sprite.SpriteId, "processed");

            return true;
        }
    }
}
