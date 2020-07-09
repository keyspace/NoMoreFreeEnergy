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

        private const float SCALING_FACTOR = 10.0f;
        private const float OG_POWER_CONSUMPTION_MULTIPLIER = 6.0f;
        private const float HYDROGEN_ENERGY_DENSITY_MULTIPLIER = 1.0f;

        public override void LoadData()
        {
            //Instance = this;

            // Gas definition is used by hydrogen thrusters; i.e. not needed for mod purposes, at least not yet.
            MyDefinitionId hydrogenId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Hydrogen");
            var definition = MyDefinitionManager.Static.GetDefinition(hydrogenId);
            var gasDefinition = (MyGasProperties)definition;
            gasDefinition.EnergyDensity *= HYDROGEN_ENERGY_DENSITY_MULTIPLIER;

            // DEBUG
            MyLog.Default.WriteLineAndConsole($"DEBUG H2 EnergyDensity: {gasDefinition.EnergyDensity}");

            // TODO: Modify character jetpack?..

            // Hydrogen engines produce X times as much power from the same amount of gas.
            MyDefinitionId lheId = new MyDefinitionId(typeof(MyObjectBuilder_HydrogenEngine), "LargeHydrogenEngine");
            var lheDefinition = MyDefinitionManager.Static.GetDefinition(lheId) as MyHydrogenEngineDefinition;
            lheDefinition.FuelProductionToCapacityMultiplier *= SCALING_FACTOR;

            // DEBUG
            MyLog.Default.WriteLineAndConsole($"DEBUG HE FuelProductionToCapacityMultiplier: {lheDefinition.FuelProductionToCapacityMultiplier}");

            // TODO: refactor for less code duplication.
            MyDefinitionId sheId = new MyDefinitionId(typeof(MyObjectBuilder_HydrogenEngine), "SmallHydrogenEngine");
            var sheDefinition = MyDefinitionManager.Static.GetDefinition(sheId) as MyHydrogenEngineDefinition;
            sheDefinition.FuelProductionToCapacityMultiplier *= SCALING_FACTOR;

            // DEBUG
            MyLog.Default.WriteLineAndConsole($"DEBUG HE FuelProductionToCapacityMultiplier: {sheDefinition.FuelProductionToCapacityMultiplier}");

            // Oxygen generators churn X times less ice in the same amount of time, and consume Y times more power.
            MyDefinitionId logId = new MyDefinitionId(typeof(MyObjectBuilder_OxygenGenerator), "");
            var logDefinition = MyDefinitionManager.Static.GetDefinition(logId) as MyOxygenGeneratorDefinition;
            logDefinition.IceConsumptionPerSecond /= SCALING_FACTOR;
            logDefinition.OperationalPowerConsumption *= OG_POWER_CONSUMPTION_MULTIPLIER;
            // Re-populate produced gases using the same scaling factor as IceConsumpionPerSecond.
            List<MyGasGeneratorResourceInfo> logProducedGases = new List<MyGasGeneratorResourceInfo>(logDefinition.ProducedGases.Count);
            logDefinition.ProducedGases.ForEach(producedGasInfo => logProducedGases.Add(
                new MyGasGeneratorResourceInfo
                {
                    Id = producedGasInfo.Id,
                    IceToGasRatio = producedGasInfo.IceToGasRatio / SCALING_FACTOR
                }
                ));
            logDefinition.ProducedGases.Clear();
            logDefinition.ProducedGases = logProducedGases;

            // DEBUG
            MyLog.Default.WriteLineAndConsole($"DEBUG OG Id.TypeId: {logDefinition.Id.TypeId}");
            MyLog.Default.WriteLineAndConsole($"DEBUG OG Id.SubtypeName: {logDefinition.Id.SubtypeName}");
            MyLog.Default.WriteLineAndConsole($"DEBUG OG IceConsumptionPerSecond: {logDefinition.IceConsumptionPerSecond}");
            MyLog.Default.WriteLineAndConsole($"DEBUG OG OperationalPowerConsumption: {logDefinition.OperationalPowerConsumption}");
            foreach (var producedGas in logDefinition.ProducedGases)
            {
                MyLog.Default.WriteLineAndConsole($"DEBUG OG logDefinition2.ProducedGases[?].Id.SubtypeName: {producedGas.Id.SubtypeName}");
                MyLog.Default.WriteLineAndConsole($"DEBUG OG logDefinition2.ProducedGases[?].IceToGasRatio: {producedGas.IceToGasRatio}");
            }

            // TODO: refactor for less code duplication.
            MyDefinitionId sogId = new MyDefinitionId(typeof(MyObjectBuilder_OxygenGenerator), "OxygenGeneratorSmall");
            var sogDefinition = MyDefinitionManager.Static.GetDefinition(sogId) as MyOxygenGeneratorDefinition;
            sogDefinition.IceConsumptionPerSecond /= SCALING_FACTOR;
            sogDefinition.OperationalPowerConsumption *= OG_POWER_CONSUMPTION_MULTIPLIER;
            // Re-populate produced gases using the same scaling factor as IceConsumpionPerSecond.
            List<MyGasGeneratorResourceInfo> sogProducedGases = new List<MyGasGeneratorResourceInfo>(sogDefinition.ProducedGases.Count);
            sogDefinition.ProducedGases.ForEach(producedGasInfo => sogProducedGases.Add(
                new MyGasGeneratorResourceInfo
                {
                    Id = producedGasInfo.Id,
                    IceToGasRatio = producedGasInfo.IceToGasRatio / SCALING_FACTOR
                }
                ));
            sogDefinition.ProducedGases.Clear();
            sogDefinition.ProducedGases = sogProducedGases;

            // DEBUG
            MyLog.Default.WriteLineAndConsole($"DEBUG OG Id.TypeId: {sogDefinition.Id.TypeId}");
            MyLog.Default.WriteLineAndConsole($"DEBUG OG Id.SubtypeName: {sogDefinition.Id.SubtypeName}");
            MyLog.Default.WriteLineAndConsole($"DEBUG OG IceConsumptionPerSecond: {sogDefinition.IceConsumptionPerSecond}");
            MyLog.Default.WriteLineAndConsole($"DEBUG OG OperationalPowerConsumption: {sogDefinition.OperationalPowerConsumption}");
            foreach (var producedGas in sogDefinition.ProducedGases)
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
