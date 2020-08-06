using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Definitions;

namespace Keyspace.NoMoreFreeEnergy
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class NoMoreFreeEnergy_Session : MySessionComponentBase
    {
        //public static NoMoreFreeEnergy_Session Instance;

        private const float HYDROGEN_ENERGY_DENSITY_MULTIPLIER = 1.0f;
        private const float HYDROGEN_ENGINE_EFFICIENCY_MULTIPLIER = 10.0f;
        private const float OXYGEN_GENERATOR_EXTRA_SPEED_DIVISOR = 6.0f;
        private const float OXYGEN_GENERATOR_SPEED_MULTIPLIER = 1.0f / (OXYGEN_GENERATOR_EXTRA_SPEED_DIVISOR * HYDROGEN_ENGINE_EFFICIENCY_MULTIPLIER);
        private const float BATTERY_MAX_POWER_INPUT_MULTIPLIER = 0.25f;

        public override void LoadData()
        {
            //Instance = this;

            MyDefinitionId hydrogenId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Hydrogen");
            var definition = MyDefinitionManager.Static.GetDefinition(hydrogenId);
            var gasDefinition = (MyGasProperties)definition;
            gasDefinition.EnergyDensity *= HYDROGEN_ENERGY_DENSITY_MULTIPLIER;
            // TODO: Modify character jetpack to have lower capacity?.. (So that flight duration is kept same.)

            RebalanceBattery(new MyDefinitionId(typeof(MyObjectBuilder_BatteryBlock), "LargeBlockBatteryBlock"));
            RebalanceBattery(new MyDefinitionId(typeof(MyObjectBuilder_BatteryBlock), "SmallBlockBatteryBlock"));
            RebalanceBattery(new MyDefinitionId(typeof(MyObjectBuilder_BatteryBlock), "SmallBlockSmallBatteryBlock"));

            RebalanceHydrogenEngine(new MyDefinitionId(typeof(MyObjectBuilder_HydrogenEngine), "LargeHydrogenEngine"));
            RebalanceHydrogenEngine(new MyDefinitionId(typeof(MyObjectBuilder_HydrogenEngine), "SmallHydrogenEngine"));

            RebalanceOxygenGenerator(new MyDefinitionId(typeof(MyObjectBuilder_OxygenGenerator), ""));
            RebalanceOxygenGenerator(new MyDefinitionId(typeof(MyObjectBuilder_OxygenGenerator), "OxygenGeneratorSmall"));
        }

        internal void RebalanceBattery(MyDefinitionId definitionId)
        {
            var definition = MyDefinitionManager.Static.GetDefinition(definitionId) as MyBatteryBlockDefinition;
            definition.RequiredPowerInput *= BATTERY_MAX_POWER_INPUT_MULTIPLIER;
        }

        internal void RebalanceHydrogenEngine(MyDefinitionId definitionId)
        {
            var definition = MyDefinitionManager.Static.GetDefinition(definitionId) as MyHydrogenEngineDefinition;
            definition.FuelProductionToCapacityMultiplier *= HYDROGEN_ENGINE_EFFICIENCY_MULTIPLIER;
        }

        internal void RebalanceOxygenGenerator(MyDefinitionId definitionId)
        {
            var definition = MyDefinitionManager.Static.GetDefinition(definitionId) as MyOxygenGeneratorDefinition;
            definition.IceConsumptionPerSecond *= OXYGEN_GENERATOR_SPEED_MULTIPLIER;
        }

        //protected override void UnloadData()
        //{
        //    Instance = null;
        //}
    }
}
