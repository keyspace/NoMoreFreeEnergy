using Sandbox.Definitions;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Keyspace.NoMoreFreeEnergy
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_GasProperties), false, "Hydrogen")]
    public class Hydrogen : MyGameLogicComponent
    {
        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            // only used once, so no need to store - just use vars here
            var properties = (MyGasProperties)Entity;

            properties.EnergyDensity *= 4.0f;

            MyLog.Default.WriteLineAndConsole($"DEBUG H2 EnergyDensity: {properties.EnergyDensity}");
        }
    }
}