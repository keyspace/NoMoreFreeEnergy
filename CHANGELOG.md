# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).


## [Unreleased]

- Nothing!

[Unreleased]: https://github.com/keyspace/NoMoreFreeEnergy/compare/v0.3.1...HEAD


## [0.3.1] - 2021-04-21
### Changed

- Fixed `OxygenGeneratorPowerConsumptionMultiplier` being also erroneously
  applied in calculating `OxygenGeneratorSpeedMultiplier` when a config was
  not present on load. In effect, the block's overall utility was being nerfed
  twice. ([_1])
- Updated instructions with an explanation of what is a configuration constraint,
  and what is an unenforced recommendation.
- Added configuration example for "closer to realism".

[Unreleased]: https://github.com/keyspace/NoMoreFreeEnergy/compare/v0.3...v0.3.1
[_1]: https://github.com/keyspace/NoMoreFreeEnergy/issues/1


## [0.3] - 2021-01-14
### Added

- An `OxygenGeneratorPowerConsumptionMultiplier` setting (default: 2.0) as
  an additional control that doesn't impact the generator's speed.

### Changed

- Set the default O2/H2 generator ice-to-gases divisor from 12.0 back to 6.0,
  as the extra x2 is now handled by the new setting. If you feel that the
  O2/H2 generator is still too slow to produce gas, you can now lower
  `OxygenGeneratorExtraSpeedDivisor` while increasing
  `OxygenGeneratorPowerConsumptionMultiplier` by the same ratio.

[0.3]: https://github.com/keyspace/NoMoreFreeEnergy/compare/v0.2.1...v0.3


## [0.2.1] - 2021-01-11
### Changed

- Updated default hydrogen engine buff and O2/H2 generator nerf to match
  behaviour of pre-1.197 (the "Wasteland" major game update).
  Vanilla hydrogen engine efficiency (hydrogen amount -> watt amount) has
  been doubled; O2/H2 generator material efficiency (ice amount ->
  hydrogen amount) has also been doubled. This results in a 4-time
  buff to the ice-to-power resource path.
  To update existing configs, `HydrogenEngineEfficiencyMultiplier` should be
  halved, and `OxygenGeneratorExtraSpeedDivisor` doubled.

[0.2.1]: https://github.com/keyspace/NoMoreFreeEnergy/compare/v0.2...v0.2.1



## [0.2] - 2020-08-27
### Added

- Allow configuring hydrogen engines' max power output. The default is left
  as in vanilla, since it's not the mod's title task.

### Changed

- O2/H2 generator speed multiplier is no longer directly configurable; it
  is still configurable indirectly, through the "extra" divisor and hydrogen
  engine's efficiency multiplier. The setting will be silently removed from
  the config file on loading the save.
- Set battery recharge rate multiplier back to 1 (from 0.25) - again, since
  it's not the mod's title task.

[0.2]: https://github.com/keyspace/NoMoreFreeEnergy/compare/v0.1...v0.2


## [0.1] - 2020-08-07
### Changed

- Do save the config once after loading.
- Added mod ID to configuration docs.

[0.1]: https://github.com/keyspace/NoMoreFreeEnergy/compare/v0.0...v0.1


## [0.0] - 2020-08-07
### Added
- Initial submission to acquire mod Steam ID.
- O2/H2 generator production speed is reduced 60 times compared to vanilla.
- Hydrogen engine fuel efficiency is increased 10 times compared to vanilla.
- Battery recharge rate is reduced 4 times compared to vanilla.
- All used constants configurable per save; fuel efficiency for hydrogen
  thrusters is also included, although left at vanilla value.

[0.0]: https://github.com/keyspace/NoMoreFreeEnergy/releases/tag/v0.0
