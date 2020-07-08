using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using VRage.Game.Components;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Keyspace.NoMoreFreeEnergy
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_HydrogenEngine), false)]
    public class HydrogenEngine : MyGameLogicComponent
    {
        // FIXME: Has same issue as OG, Init() is called on each block built, and the scaling
        // is applied on each invocation. Move to Session()?..

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            MyLog.Default.WriteLineAndConsole("DEBUG In HE Init()");

            // only used once, so no need to store - just use vars here
            var block = (MyCubeBlock)Entity;
            var definition = (MyPowerProducerDefinition)block.BlockDefinition as MyHydrogenEngineDefinition;

            // TODO: configurable!
            definition.FuelProductionToCapacityMultiplier *= 10.0f;  // FIXME: make single-const, same as OxygenGenerator's IceConsumptionPerSecond

            MyLog.Default.WriteLineAndConsole($"DEBUG HE FuelProductionToCapacityMultiplier: {definition.FuelProductionToCapacityMultiplier}");
        }
    }
}