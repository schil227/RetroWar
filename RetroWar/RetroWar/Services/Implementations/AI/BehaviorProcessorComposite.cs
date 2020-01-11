using RetroWar.Models.Vehicles.Vehicles.EnemyVehicle;
using RetroWar.Services.Interfaces.AI;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.AI
{
    public class BehaviorProcessorComposite : IBehaviorProcessor
    {
        private readonly IEnumerable<IBehaviorProcessor> behaviorProcessors;

        public BehaviorProcessorComposite
            (
            IEnumerable<IBehaviorProcessor> behaviorProcessors
            )
        {
            this.behaviorProcessors = behaviorProcessors;
        }

        public bool ProcessBehavior(EnemyVehicle enemy, float deltaTime)
        {
            foreach (var processor in behaviorProcessors)
            {
                if (processor.ProcessBehavior(enemy, deltaTime))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
