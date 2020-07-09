using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using static Sandbox.Definitions.MyOxygenGeneratorDefinition;

namespace Keyspace.NoMoreFreeEnergy
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class NoMoreFreeEnergy_Session : MySessionComponentBase
    {
        //public static NoMoreFreeEnergy_Session Instance;

        public override void LoadData()
        {
            //Instance = this;

            // Gas definition is used by hydrogen thrusters; i.e. not needed for mod purposes, at least not yet.
            MyDefinitionId hydrogenId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Hydrogen");
            var definition = MyDefinitionManager.Static.GetDefinition(hydrogenId);
            var gasDefinition = (MyGasProperties)definition;
            gasDefinition.EnergyDensity *= 1.0f;

            // DEBUG
            var definition2 = MyDefinitionManager.Static.GetDefinition(hydrogenId);
            var gasDefinition2 = (MyGasProperties)definition2;
            MyLog.Default.WriteLineAndConsole($"DEBUG H2 EnergyDensity: {gasDefinition2.EnergyDensity}");

            // TODO: Modify character jetpack?..

            // Hydrogen engines produce X times as much power from the same amount of gas.
            MyDefinitionId lheId = new MyDefinitionId(typeof(MyObjectBuilder_HydrogenEngine), "LargeHydrogenEngine");
            var lheDefinition = MyDefinitionManager.Static.GetDefinition(lheId) as MyHydrogenEngineDefinition;
            lheDefinition.FuelProductionToCapacityMultiplier *= 10.0f;  // FIXME: make single-const, same as OxygenGenerator's IceConsumptionPerSecond
            
            // TODO: refactor for less code duplication.
            MyDefinitionId sheId = new MyDefinitionId(typeof(MyObjectBuilder_HydrogenEngine), "SmallHydrogenEngine");
            var sheDefinition = MyDefinitionManager.Static.GetDefinition(sheId) as MyHydrogenEngineDefinition;
            sheDefinition.FuelProductionToCapacityMultiplier *= 10.0f;  // FIXME: make single-const, same as OxygenGenerator's IceConsumptionPerSecond

            // Oxygen generators churn X times less ice in the same amount of time, and consume Y times more power.
            MyDefinitionId logId = new MyDefinitionId(typeof(MyObjectBuilder_OxygenGenerator), "");
            var logDefinition = MyDefinitionManager.Static.GetDefinition(logId) as MyOxygenGeneratorDefinition;
            logDefinition.IceConsumptionPerSecond /= 10.0f;  // FIXME: make single-const, same as HydrogenEngine's FuelProductionToCapacityMultiplier
            logDefinition.OperationalPowerConsumption *= 6.0f;
            // Re-populate produced gases using the same scaling factor as IceConsumpionPerSecond.
            List<MyGasGeneratorResourceInfo> logProducedGases = new List<MyGasGeneratorResourceInfo>(logDefinition.ProducedGases.Count);
            logDefinition.ProducedGases.ForEach(producedGasInfo => logProducedGases.Add(
                new MyGasGeneratorResourceInfo
                {
                    Id = producedGasInfo.Id,
                    IceToGasRatio = producedGasInfo.IceToGasRatio / 10.0f  // FIXME: make single-const, same as above
                }
                ));
            logDefinition.ProducedGases.Clear();
            logDefinition.ProducedGases = logProducedGases;

            // DEBUG
            var logDefinition2 = MyDefinitionManager.Static.GetDefinition(logId) as MyOxygenGeneratorDefinition;
            MyLog.Default.WriteLineAndConsole($"DEBUG OG Id.TypeId: {logDefinition2.Id.TypeId}");
            MyLog.Default.WriteLineAndConsole($"DEBUG OG Id.SubtypeName: {logDefinition2.Id.SubtypeName}");
            MyLog.Default.WriteLineAndConsole($"DEBUG OG IceConsumptionPerSecond: {logDefinition2.IceConsumptionPerSecond}");
            MyLog.Default.WriteLineAndConsole($"DEBUG OG OperationalPowerConsumption: {logDefinition2.OperationalPowerConsumption}");
            foreach (var producedGas in logDefinition2.ProducedGases)
            {
                MyLog.Default.WriteLineAndConsole($"DEBUG OG logDefinition2.ProducedGases[?].Id.SubtypeName: {producedGas.Id.SubtypeName}");
                MyLog.Default.WriteLineAndConsole($"DEBUG OG logDefinition2.ProducedGases[?].IceToGasRatio: {producedGas.IceToGasRatio}");
            }

            // TODO: refactor for less code duplication.
            MyDefinitionId sogId = new MyDefinitionId(typeof(MyObjectBuilder_OxygenGenerator), "OxygenGeneratorSmall");
            var sogDefinition = MyDefinitionManager.Static.GetDefinition(sogId) as MyOxygenGeneratorDefinition;
            sogDefinition.IceConsumptionPerSecond /= 10.0f;  // FIXME: make single-const, same as HydrogenEngine's FuelProductionToCapacityMultiplier
            sogDefinition.OperationalPowerConsumption *= 6.0f;
            // Re-populate produced gases using the same scaling factor as IceConsumpionPerSecond.
            List<MyGasGeneratorResourceInfo> sogProducedGases = new List<MyGasGeneratorResourceInfo>(sogDefinition.ProducedGases.Count);
            sogDefinition.ProducedGases.ForEach(producedGasInfo => sogProducedGases.Add(
                new MyGasGeneratorResourceInfo
                {
                    Id = producedGasInfo.Id,
                    IceToGasRatio = producedGasInfo.IceToGasRatio / 10.0f  // FIXME: make single-const, same as above
                }
                ));
            sogDefinition.ProducedGases.Clear();
            sogDefinition.ProducedGases = sogProducedGases;

            // DEBUG
            var sogDefinition2 = MyDefinitionManager.Static.GetDefinition(logId) as MyOxygenGeneratorDefinition;
            MyLog.Default.WriteLineAndConsole($"DEBUG OG Id.TypeId: {sogDefinition2.Id.TypeId}");
            MyLog.Default.WriteLineAndConsole($"DEBUG OG Id.SubtypeName: {sogDefinition2.Id.SubtypeName}");
            MyLog.Default.WriteLineAndConsole($"DEBUG OG IceConsumptionPerSecond: {sogDefinition2.IceConsumptionPerSecond}");
            MyLog.Default.WriteLineAndConsole($"DEBUG OG OperationalPowerConsumption: {sogDefinition2.OperationalPowerConsumption}");
            foreach (var producedGas in sogDefinition2.ProducedGases)
            {
                MyLog.Default.WriteLineAndConsole($"DEBUG OG sogDefinition2.ProducedGases[?].Id.SubtypeName: {producedGas.Id.SubtypeName}");
                MyLog.Default.WriteLineAndConsole($"DEBUG OG sogDefinition2.ProducedGases[?].IceToGasRatio: {producedGas.IceToGasRatio}");
            }
        }

        //protected override void UnloadData()
        //{
        //    Instance = null;
        //}
    }
}
