# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).


## [Unreleased]

- Nothing!

[Unreleased]: https://github.com//Stamina/compare/v0.2...HEAD


## [0.2.1] - 2021-01-11
### Changed

- Updated HydrogenEngineEfficiencyMultiplier to match pre-1.197 (the major)
  game update. Since vanilla hydrogen engine efficiency has been increased
  twice, the mod-provided boost has been reduced twice (from 10 to 5 by
  default). For existing games, make the same change manually in the configuration
  file.


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


## [0.1] - 2020-08-07
### Changed

- Do save the config once after loading.
- Added mod ID to configuration docs.

[0.1]: https://github.com/keyspace/Stamina/compare/v0.0...v0.1


## [0.0] - 2020-08-07
### Added
- Initial submission to acquire mod Steam ID.
- O2/H2 generator production speed is reduced 60 times compared to vanilla.
- Hydrogen engine fuel efficiency is increased 10 times compared to vanilla.
- Battery recharge rate is reduced 4 times compared to vanilla.
- All used constants configurable per save; fuel efficiency for hydrogen
  thrusters is also included, although left at vanilla value.

[0.0]: https://github.com/keyspace/NoMoreFreeEnergy/releases/tag/v0.0
