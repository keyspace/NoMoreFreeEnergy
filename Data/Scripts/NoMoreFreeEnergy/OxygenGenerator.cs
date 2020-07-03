using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using VRage.Game.Components;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Keyspace.NoMoreFreeEnergy
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_OxygenGenerator), false)]
    public class OxygenGenerator : MyGameLogicComponent
    {
        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            // only used once, so no need to store - just use vars here
            var block = (MyCubeBlock)Entity;
            var definition = (MyProductionBlockDefinition)block.BlockDefinition as MyOxygenGeneratorDefinition;

            // TODO: configurable!
            definition.IceConsumptionPerSecond /= 10.0f;  // same as HydrogenEngine's FuelProductionToCapacityMultiplier
            definition.OperationalPowerConsumption *= 6.0f;

            MyLog.Default.WriteLineAndConsole($"DEBUG OG IceConsumptionPerSecond: {definition.IceConsumptionPerSecond}");
            MyLog.Default.WriteLineAndConsole($"DEBUG OG OperationalPowerConsumption: {definition.OperationalPowerConsumption}");
        }
    }
}