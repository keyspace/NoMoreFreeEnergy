using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;

namespace Keyspace.NoMoreFreeEnergy
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class NoMoreFreeEnergy_Session : MySessionComponentBase
    {
        //public static NoMoreFreeEnergy_Session Instance;

        //public override void LoadData()
        //{
        //    Instance = this;
        //}

        public override void BeforeStart()
        {
            // Gas definition is used by hydrogen thrusters; i.e. not needed for mod purposes, at least not yet.
            MyDefinitionId hydrogenId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Hydrogen");
            var definition = MyDefinitionManager.Static.GetDefinition(hydrogenId);
            var gasDefinition = (MyGasProperties)definition;
            gasDefinition.EnergyDensity *= 1.0f;

            // DEBUG
            var definition2 = MyDefinitionManager.Static.GetDefinition(hydrogenId);
            var gasDefinition2 = (MyGasProperties)definition2;
            MyLog.Default.WriteLineAndConsole($"DEBUG H2 EnergyDensity: {gasDefinition2.EnergyDensity}");

            // TODO: Modify character jetpack.

            // Hydrogen engines produce X times as much poswer from the same amount of gas.
            MyDefinitionId lheId = new MyDefinitionId(typeof(MyObjectBuilder_HydrogenEngine), "LargeHydrogenEngine");
            var lheDefinition = MyDefinitionManager.Static.GetDefinition(lheId) as MyHydrogenEngineDefinition;
            lheDefinition.FuelProductionToCapacityMultiplier *= 10.0f;  // FIXME: make single-const, same as OxygenGenerator's IceConsumptionPerSecond
            MyDefinitionId sheId = new MyDefinitionId(typeof(MyObjectBuilder_HydrogenEngine), "SmallHydrogenEngine");
            var sheDefinition = MyDefinitionManager.Static.GetDefinition(sheId) as MyHydrogenEngineDefinition;
            sheDefinition.FuelProductionToCapacityMultiplier *= 10.0f;  // FIXME: make single-const, same as OxygenGenerator's IceConsumptionPerSecond

            
        }

        //protected override void UnloadData()
        //{
        //    Instance = null;
        //}
    }
}
