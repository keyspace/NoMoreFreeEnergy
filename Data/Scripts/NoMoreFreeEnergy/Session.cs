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
            MyDefinitionId hydrogenId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Hydrogen");
            var definition = MyDefinitionManager.Static.GetDefinition(hydrogenId);
            var gasDefinition = (MyGasProperties)definition;

            // used in thrusters; i.e. not needed for mod purposes ATM
            gasDefinition.EnergyDensity *= 1.0f;

            var definition2 = MyDefinitionManager.Static.GetDefinition(hydrogenId);
            var gasDefinition2 = (MyGasProperties)definition2;
            MyLog.Default.WriteLineAndConsole($"DEBUG H2 EnergyDensity: {gasDefinition2.EnergyDensity}");
        }

        //protected override void UnloadData()
        //{
        //    Instance = null;
        //}
    }
}
