using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.ObjectBuilders;
using VRage.Utils;
using static Sandbox.Definitions.MyOxygenGeneratorDefinition;

namespace Keyspace.NoMoreFreeEnergy
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_OxygenGenerator), false)]
    public class OxygenGenerator : MyGameLogicComponent
    {
        // FIXME: looks like there's only one var istance for both subtypes; threaded loading means there's a race condition
        static bool initDone = false;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            MyLog.Default.WriteLineAndConsole("DEBUG In OG Init()");
            if (initDone)
            {
                MyLog.Default.WriteLineAndConsole($"DEBUG Init already done! Skipping re-init of: (FIXME: <size/subtype> oxygen generator)");
                return;
            }
            
            // only used once, so no need to store - just use vars here
            var block = (MyCubeBlock)Entity;
            var definition = (MyProductionBlockDefinition)block.BlockDefinition as MyOxygenGeneratorDefinition;

            // TODO: configurable!
            definition.IceConsumptionPerSecond /= 10.0f;  // FIXME: make single-const, same as HydrogenEngine's FuelProductionToCapacityMultiplier
            definition.OperationalPowerConsumption *= 6.0f;

            // Re-populate produced gases using the same scaling factor as IceConsumpionPerSecond.
            List<MyGasGeneratorResourceInfo> producedGases = new List<MyGasGeneratorResourceInfo>(definition.ProducedGases.Count);
            definition.ProducedGases.ForEach(producedGasInfo => producedGases.Add(
                new MyGasGeneratorResourceInfo
                {
                    Id = producedGasInfo.Id,
                    IceToGasRatio = producedGasInfo.IceToGasRatio / 10.0f  // FIXME: make single-const, same as above
                }
                ));
            definition.ProducedGases.Clear();
            definition.ProducedGases = producedGases;

            initDone = true;

            MyLog.Default.WriteLineAndConsole($"DEBUG OG Id.TypeId: {definition.Id.TypeId}");
            MyLog.Default.WriteLineAndConsole($"DEBUG OG Id.SubtypeName: {definition.Id.SubtypeName}");
            MyLog.Default.WriteLineAndConsole($"DEBUG OG IceConsumptionPerSecond: {definition.IceConsumptionPerSecond}");
            MyLog.Default.WriteLineAndConsole($"DEBUG OG OperationalPowerConsumption: {definition.OperationalPowerConsumption}");
            foreach (var producedGas in definition.ProducedGases)
            {
                MyLog.Default.WriteLineAndConsole($"DEBUG OG definition.ProducedGases[?].Id.SubtypeName: {producedGas.Id.SubtypeName}");
                MyLog.Default.WriteLineAndConsole($"DEBUG OG definition.ProducedGases[?].IceToGasRatio: {producedGas.IceToGasRatio}");
            }
        }
    }
}