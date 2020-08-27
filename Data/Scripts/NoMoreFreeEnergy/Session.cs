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

        private Config Config;

        public override void LoadData()
        {
            //Instance = this;

            Config = StorageFile.Load<Config>("config.xml");
            // Save immediately instead of in SaveData(), so it's only done once.
            StorageFile.Save("config.xml", Config);

            RebalanceBattery(new MyDefinitionId(typeof(MyObjectBuilder_BatteryBlock), "LargeBlockBatteryBlock"));
            RebalanceBattery(new MyDefinitionId(typeof(MyObjectBuilder_BatteryBlock), "SmallBlockBatteryBlock"));
            RebalanceBattery(new MyDefinitionId(typeof(MyObjectBuilder_BatteryBlock), "SmallBlockSmallBatteryBlock"));

            RebalanceHydrogenEngine(new MyDefinitionId(typeof(MyObjectBuilder_HydrogenEngine), "LargeHydrogenEngine"));
            RebalanceHydrogenEngine(new MyDefinitionId(typeof(MyObjectBuilder_HydrogenEngine), "SmallHydrogenEngine"));
            
            RebalanceHydrogenGas(new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Hydrogen"));
            // TODO: Modify character jetpack to have lower capacity?.. (So that flight duration is kept same.)

            RebalanceOxygenGenerator(new MyDefinitionId(typeof(MyObjectBuilder_OxygenGenerator), ""));
            RebalanceOxygenGenerator(new MyDefinitionId(typeof(MyObjectBuilder_OxygenGenerator), "OxygenGeneratorSmall"));
        }

        private void RebalanceBattery(MyDefinitionId definitionId)
        {
            var definition = MyDefinitionManager.Static.GetDefinition(definitionId) as MyBatteryBlockDefinition;
            definition.RequiredPowerInput *= Config.BatteryMaxPowerInputMultiplier;
        }

        private void RebalanceHydrogenEngine(MyDefinitionId definitionId)
        {
            var definition = MyDefinitionManager.Static.GetDefinition(definitionId) as MyHydrogenEngineDefinition;
            definition.FuelProductionToCapacityMultiplier *= Config.HydrogenEngineEfficiencyMultiplier;
            definition.MaxPowerOutput *= Config.HydrogenEngineMaxPowerOutputMultiplier;
        }

        private void RebalanceHydrogenGas(MyDefinitionId definitionId)
        {
            var definition = MyDefinitionManager.Static.GetDefinition(definitionId) as MyGasProperties;
            definition.EnergyDensity *= Config.HydrogenGasEnergyDensityMultiplier;
        }

        private void RebalanceOxygenGenerator(MyDefinitionId definitionId)
        {
            var definition = MyDefinitionManager.Static.GetDefinition(definitionId) as MyOxygenGeneratorDefinition;
            definition.IceConsumptionPerSecond *= Config.OxygenGeneratorSpeedMultiplier;
        }

        //protected override void UnloadData()
        //{
        //    Instance = null;
        //}
    }
}
