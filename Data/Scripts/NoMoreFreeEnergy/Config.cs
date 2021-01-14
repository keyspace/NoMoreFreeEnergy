namespace Keyspace.NoMoreFreeEnergy
{
    /// <summary>
    /// Represents the configuration, loadable from a file.
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Maximum allowed battery input; multiplier compared to vanilla.
        /// Allows making the battery more "sluggish" to recharge, making the choice of using
        /// batteries-only vs. hydrogen-only more pronounced: either simple but slow to recharge
        /// (batteries) or complex but fast to refuel (hydrogen engines).
        /// </summary>
        public float BatteryMaxPowerInputMultiplier { get; set; }

        /// <summary>
        /// Energy efficiency of hydrogen engines when converting hydrogen gas to electric power;
        /// multiplier compared to vanilla.
        /// Used directly by game and is also factored into the calculation of final
        /// OxygenGeneratorSpeedMultiplier.
        /// </summary>
        public float HydrogenEngineEfficiencyMultiplier { get; set; }

        /// <summary>
        /// Maximum power that hydrogen engines are capable of producing;
        /// multiplier compared to vanilla.
        /// Allows making the engines a viable source of power for atmospheric thrusters - vanilla
        /// engines are less powerful that batteries (leave alone size comparisons).
        /// </summary>
        public float HydrogenEngineMaxPowerOutputMultiplier { get; set; }

        /// <summary>
        /// Energy density of hydrogen gas when used as fuel by thrusters (not by engines!);
        /// multiplier compared to vanilla.
        /// Provided for user convenience only, to make the game easier/harder.
        /// </summary>
        public float HydrogenGasEnergyDensityMultiplier { get; set; }

        /// <summary>
        /// Amount of power which O2/H2 generators consume in a given time period;
        /// used directly in-game.
        /// Provided as a secondary control to adjust the costs of running an O2/H2 generator
        /// without impacting its ice-to-gas conversion speed.
        /// </summary>
        public float OxygenGeneratorPowerConsumptionMultiplier { get; set; }

        /// <summary>
        /// Amount of ice which O2/H2 generators consume in a given time period;
        /// used indirectly in-game (hence "Extra").
        /// Essential to achieve the mod's balancing act: this is how much the generators must
        /// be slowed down compared to vanilla, irrespective of whether hydrogen engines were
        /// made more efficient or not.
        /// </summary>
        public float OxygenGeneratorExtraSpeedDivisor { get; set; }

        /// <summary>
        /// Amount of ice which O2/H2 generators consume in a given time period;
        /// multiplier compared to vanilla.
        /// Used directly by game, factoring in both the mod's essential
        /// re-balancing and the possible efficiency increase of hydrogen engines.
        /// Can not be configured directly - use the other settings.
        /// </summary>
        internal float OxygenGeneratorSpeedMultiplier {
            get {
                return 1.0f / (OxygenGeneratorExtraSpeedDivisor * HydrogenEngineEfficiencyMultiplier);
            }
            set {
                return;
            }
        }

        /// <summary>
        /// Constructor contains defaults; these properties will remain as below if the config couldn't be loaded.
        /// </summary>
        public Config()
        {
            // Initial release used to reduce batteries' max input, so hydrogen would become a
            // more competitive way of storage and source of power (by way of tanks re-filling even faster
            // in comparison); however, it's been highlighted that this strays from the core mission of
            // the mod, so instead...
            //
            // MAGICNUM 1.0f: leave it up to the player. Used to be 0.25f.
            BatteryMaxPowerInputMultiplier = 1.0f;

            // Hydrogen engines produce more power from the same amount of gas. This makes them more
            // useful, and allows them to compete with batteries.
            //
            // MAGICNUM 5.0f: semi-arbitrary, vanilla is 0.01f, this seemed a big-enough improvement.
            HydrogenEngineEfficiencyMultiplier = 5.0f;

            // Hydrogen engines could produce more power in a given amount of time. This could make them more
            // useful, and could allow them to compete with batteries even in powering atmospheric flying
            // vehicles, but that drifts too much from the core mission of the mod, so...
            //
            // MAGICNUM 1.0f: leave it up to the player.
            HydrogenEngineMaxPowerOutputMultiplier = 1.0f;

            // Thrusters are as gas-hungry as in vanilla. It's a large consideration now whether you want to
            // get airborne using hydrogen or atmospherics.
            //
            // MAGICNUM 1.0f: leave it up to the player.
            HydrogenGasEnergyDensityMultiplier = 1.0f;

            // The "Wasteland" update halved the generator's ice-to-gases consumption rate, while
            // leaving power consumption at the same level. Since the mod was already doing this
            // at an even greter scale, a second control is now required so that the generator's
            // speed is not affected too much.
            //
            // MAGICNUM 2.0f: Keen made ice-to-gases two times more efficient; restore pre-update
            // balance by increasing power consumption the same amount.
            OxygenGeneratorPowerConsumptionMultiplier = 2.0f;

            // O2/H2 generators churn less ice in the same amount of time, and in effect consume more power per
            // unit of gas produced.
            //
            // MAGICNUM 6.0f: picked empirically; seems the lowest that
            //     OxygenGeneratorExtraSpeedDivisor * OxygenGeneratorPowerConsumptionMultiplier
            // can go is 10.0f or so - otherwise hydrogen can be stockpiled even without exploits.
            OxygenGeneratorExtraSpeedDivisor = 6.0f;

            // Apply HEEM and OGESD to calculate the final value we'll be using.
            //
            // NOTE: Earlier versions of this mod avoided increasing O2/H2 gen's power consumption
            // from 100 kW to 600 kW) instead of reducing its ice-to-gases conversion rate. This was
            // due to an exploit of running a generator on an under-powered grid: due to Keen's check
            // order and short cycle period, it was possible to get full output while consuming only
            // a fraction of the input.
            //
            // This may still be possible, but seems less pronounced since "Sparks of the Future" /
            // "Server Optimisations" updates (v1.195/v1.196 IIRC), where the cycle period is increased,
            // and (seemingly) power input requirements must be met _before_ a block will cycle.
            OxygenGeneratorSpeedMultiplier = 1.0f / (OxygenGeneratorExtraSpeedDivisor * OxygenGeneratorPowerConsumptionMultiplier * HydrogenEngineEfficiencyMultiplier);
        }
    }
}