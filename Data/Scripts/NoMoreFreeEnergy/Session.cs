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
        
        // Hydrogen engines produce more power from the same amount of gas. This makes hydrogen engines more
        // useful, and allows them to compete with batteries.
        // MAGICNUM 10.0f: semi-arbitrary, vanilla is 0.005f, this seemed a big-enough improvement.
        private const float HYDROGEN_ENGINE_EFFICIENCY_MULTIPLIER = 10.0f;

        // O2/H2 generators churn less ice in the same amount of time, and in effect consume more power per
        // unit of gas produced.
        // MAGICNUM 6.0f: picked empirically; seems the lowest this can go is 5.0f or so - otherwise hydrogen
        // can be stockpiled even without exploits.
        private const float OXYGEN_GENERATOR_EXTRA_SPEED_DIVISOR = 6.0f;
        // Apply both multipliers as divisors to get the final value we'll be using.
        private const float OXYGEN_GENERATOR_SPEED_MULTIPLIER = 1.0f / (OXYGEN_GENERATOR_EXTRA_SPEED_DIVISOR * HYDROGEN_ENGINE_EFFICIENCY_MULTIPLIER);

        // Earlier versions of this mod also increased O2/H2 gen's power consumption (by same multiplier, 6.0f,
        // from 100 kW to 600 kW) instead of further reducing its production speed as above.
        //
        // However, this showed to be easily exploitable by not providing enough power to
        // the generator. The rates of ice->gas and power->gas would get out of whack.
        //
        // This could be side-stepped for the H2 engine (e.g. small-grid vanilla power of 500 kW) by giving it a
        // max power output multiplier, so it is above the gen's consumption (e.g. from 500 kW to 750 kW).
        //
        // But the exploit would still be possible on small-grid by powering an O2/H2 generator using a small
        // battery (vanilla output of 200 kW), and increasing _that_ would start to be too much creep.
        //
        // To clarify, the exploit is still possible, just not as apparent.

        // TODO: Reduce batteries' max input and increase their power loss factor, so hydrogen becomes a
        // more competitive storage/source of power by way of tanks re-filling faster and not having transmission
        // losses.

        public override void LoadData()
        {
            //Instance = this;

            MyDefinitionId hydrogenId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Hydrogen");
            var definition = MyDefinitionManager.Static.GetDefinition(hydrogenId);
            var gasDefinition = (MyGasProperties)definition;
            gasDefinition.EnergyDensity *= HYDROGEN_ENERGY_DENSITY_MULTIPLIER;
            // TODO: Modify character jetpack to have lower capacity?.. (So that flight duration is kept same.)

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
        }

        //protected override void UnloadData()
        //{
        //    Instance = null;
        //}
    }
}
