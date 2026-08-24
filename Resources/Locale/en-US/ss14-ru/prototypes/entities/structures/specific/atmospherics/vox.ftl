ent-AirSensorVoxBase = { ent-AirSensorBase }
    .suffix = Vox Atmosphere
    .desc = { ent-AirSensorBase.desc }
ent-AirSensorVox = { ent-AirSensorVoxBase }
    .desc = { ent-AirSensorVoxBase.desc }
ent-{'values': ['GasVentPumpVox', 'GasVentPumpVoxAlt1', 'GasVentPumpVoxAlt2']} = { ent-{'sequences': [['AirSensorVoxBase', 'GasVentPump'], ['AirSensorVoxBase', 'GasVentPumpAlt1'], ['AirSensorVoxBase', 'GasVentPumpAlt2']]} }

  .desc = { ent-{'sequences': [['AirSensorVoxBase', 'GasVentPump'], ['AirSensorVoxBase', 'GasVentPumpAlt1'], ['AirSensorVoxBase', 'GasVentPumpAlt2']]}.desc }
ent-{'values': ['GasVentScrubberVox', 'GasVentScrubberVoxAlt1', 'GasVentScrubberVoxAlt2']} = { ent-{'sequences': [['AirSensorVoxBase', 'GasVentScrubber'], ['AirSensorVoxBase', 'GasVentScrubberAlt1'], ['AirSensorVoxBase', 'GasVentScrubberAlt2']]} }

  .desc = { ent-{'sequences': [['AirSensorVoxBase', 'GasVentScrubber'], ['AirSensorVoxBase', 'GasVentScrubberAlt1'], ['AirSensorVoxBase', 'GasVentScrubberAlt2']]}.desc }
ent-AirAlarmVox = { ent-AirAlarm }
    .suffix = Vox Atmosphere, auto mode disabled
    .desc = { ent-AirAlarm.desc }
