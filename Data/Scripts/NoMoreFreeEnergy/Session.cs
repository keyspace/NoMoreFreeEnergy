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

        // Gas definition is used by hydrogen thrusters; i.e. not needed for mod purposes, at least not yet.
        private const float HYDROGEN_ENERGY_DENSITY_MULTIPLIER = 1.0f;
        // Hydrogen engines produce more power from the same amount of gas.
        private const float HYDROGEN_ENGINE_EFFICIENCY_MULTIPLIER = 10.0f;
        // Oxygen generators churn less ice in the same amount of time, and consume more power.
        private const float OXYGEN_GENERATOR_SPEED_MULTIPLIER = 1 / HYDROGEN_ENGINE_EFFICIENCY_MULTIPLIER;
        private const float OXYGEN_GENERATOR_POWER_CONSUMPTION_MULTIPLIER = 6.0f;
        
        public override void LoadData()
        {
            //Instance = this;

            MyDefinitionId hydrogenId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Hydrogen");
            var definition = MyDefinitionManager.Static.GetDefinition(hydrogenId);
            var gasDefinition = (MyGasProperties)definition;
            gasDefinition.EnergyDensity *= HYDROGEN_ENERGY_DENSITY_MULTIPLIER;

            // TODO: Modify character jetpack?..
            
            RebalanceHydrogenEngine(new MyDefinitionId(typeof(MyObjectBuilder_HydrogenEngine), "LargeHydrogenEngine"));
            RebalanceHydrogenEngine(new MyDefinitionId(typeof(MyObjectBuilder_HydrogenEngine), "SmallHydrogenEngine"));
            
            RebalanceOxygenGenerator(new MyDefinitionId(typeof(MyObjectBuilder_OxygenGenerator), ""));
            RebalanceOxygenGenerator(new MyDefinitionId(typeof(MyObjectBuilder_OxygenGenerator), "OxygenGeneratorSmall"));
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
            definition.OperationalPowerConsumption *= OXYGEN_GENERATOR_POWER_CONSUMPTION_MULTIPLIER;
        }

        //protected override void UnloadData()
        //{
        //    Instance = null;
        //}
    }
}
