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
        /// (batteries) or complex but fast to recharge (hydrogen engines).
        /// </summary>
        public float BatteryMaxPowerInputMultiplier { get; set; }

        /// <summary>
        /// Energy density of hydrogen gas when used as fuel by thrusters (not by engines!);
        /// multiplier compared to vanilla.
        /// Provided for user convenience only, to make the game easier/harder.
        /// </summary>
        public float HydrogenGasEnergyDensityMultiplier { get; set; }

        /// <summary>
        /// Energy efficiency of hydrogen engines when converting hydrogen gas to electric power;
        /// multiplier compared to vanilla.
        /// Used directly by game and is also factored into the calculation of final
        /// OxygenGeneratorSpeedMultiplier.
        /// </summary>
        public float HydrogenEngineEfficiencyMultiplier { get; set; }

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
        /// Should not be configured directly - use the other settings.
        /// </summary>
        internal float OxygenGeneratorSpeedMultiplier { get; set; }

        /// <summary>
        /// Constructor contains defaults; these properties will remain as below if the config couldn't be loaded.
        /// </summary>
        public Config()
        {
            // Thrusters are as gas-hungry as in vanilla. It's a large consideration now whether you want to
            // get airborne.
            //
            // MAGICNUM 1.0f: not used by mod yet, so don't modify.
            HydrogenGasEnergyDensityMultiplier = 1.0f;

            // Hydrogen engines produce more power from the same amount of gas. This makes hydrogen engines more
            // useful, and allows them to compete with batteries.
            //
            // MAGICNUM 10.0f: semi-arbitrary, vanilla is 0.005f, this seemed a big-enough improvement.
            HydrogenEngineEfficiencyMultiplier = 10.0f;

            // O2/H2 generators churn less ice in the same amount of time, and in effect consume more power per
            // unit of gas produced.
            //
            // MAGICNUM 6.0f: picked empirically; seems the lowest this can go is 5.0f or so - otherwise hydrogen
            // can be stockpiled even without exploits.
            OxygenGeneratorExtraSpeedDivisor = 6.0f;

            // Apply HEEM and OGESD to calculate the final value we'll be using.
            //
            // NOTE: Earlier versions of this mod also increased O2/H2 gen's power consumption (by same multiplier, 6.0f,
            // from 100 kW to 600 kW) instead of further reducing its production speed as below.
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
            OxygenGeneratorSpeedMultiplier = 1.0f / (OxygenGeneratorExtraSpeedDivisor * HydrogenEngineEfficiencyMultiplier);

            // Reduce batteries' max input, so hydrogen becomes a more competitive storage/source of power
            // by way of tanks re-filling even faster in comparison.
            //
            // MAGICNUM 0.25f: arbitrary; four times slower seems slow enough.
            BatteryMaxPowerInputMultiplier = 0.25f;
        }
    }
}