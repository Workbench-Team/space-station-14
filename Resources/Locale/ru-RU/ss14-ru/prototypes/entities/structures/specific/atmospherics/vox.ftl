ent-AirSensorVoxBase = { ent-AirSensorBase }
    .suffix = Атмосфера воксов
    .desc = { ent-AirSensorBase.desc }
ent-AirSensorVox = { ent-AirSensor }
    .desc = { ent-AirSensor.desc }
ent-{'values': ['GasVentPumpVox', 'GasVentPumpVoxAlt1', 'GasVentPumpVoxAlt2']} = { ent-{'sequences': [['AirSensorVoxBase', 'GasVentPump'], ['AirSensorVoxBase', 'GasVentPumpAlt1'], ['AirSensorVoxBase', 'GasVentPumpAlt2']]} }

  .desc = { ent-{'sequences': [['AirSensorVoxBase', 'GasVentPump'], ['AirSensorVoxBase', 'GasVentPumpAlt1'], ['AirSensorVoxBase', 'GasVentPumpAlt2']]}.desc }
ent-{'values': ['GasVentScrubberVox', 'GasVentScrubberVoxAlt1', 'GasVentScrubberVoxAlt2']} = { ent-{'sequences': [['AirSensorVoxBase', 'GasVentScrubber'], ['AirSensorVoxBase', 'GasVentScrubberAlt1'], ['AirSensorVoxBase', 'GasVentScrubberAlt2']]} }

  .desc = { ent-{'sequences': [['AirSensorVoxBase', 'GasVentScrubber'], ['AirSensorVoxBase', 'GasVentScrubberAlt1'], ['AirSensorVoxBase', 'GasVentScrubberAlt2']]}.desc }
ent-GasVentPumpVox = { ent-GasVentPump }
    .desc = { ent-GasVentPump.desc }
ent-GasVentScrubberVox = { ent-GasVentScrubber }
    .desc = { ent-GasVentScrubber.desc }
ent-AirAlarmVox = { ent-AirAlarm }
    .suffix = Атмосфера воксов, авторежим выключен
    .desc = { ent-AirAlarm.desc }
