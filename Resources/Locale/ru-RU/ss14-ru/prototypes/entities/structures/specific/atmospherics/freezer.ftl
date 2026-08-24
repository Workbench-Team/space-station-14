ent-AirSensorFreezerBase = { ent-AirSensorBase }
    .suffix = Атмосфера морозильной камеры
    .desc = { ent-AirSensorBase.desc }
ent-AirSensorFreezer = { ent-AirSensorFreezerBase }
    .desc = { ent-AirSensorFreezerBase.desc }
ent-{'values': ['GasVentPumpFreezer', 'GasVentPumpFreezerAlt1', 'GasVentPumpFreezerAlt2']} = { ent-{'sequences': [['AirSensorFreezerBase', 'GasVentPump'], ['AirSensorFreezerBase', 'GasVentPumpAlt1'], ['AirSensorFreezerBase', 'GasVentPumpAlt2']]} }

  .desc = { ent-{'sequences': [['AirSensorFreezerBase', 'GasVentPump'], ['AirSensorFreezerBase', 'GasVentPumpAlt1'], ['AirSensorFreezerBase', 'GasVentPumpAlt2']]}.desc }
ent-{'values': ['GasVentScrubberFreezer', 'GasVentScrubberFreezerAlt1', 'GasVentScrubberFreezerAlt2']} = { ent-{'sequences': [['AirSensorFreezerBase', 'GasVentScrubber'], ['AirSensorFreezerBase', 'GasVentScrubberAlt1'], ['AirSensorFreezerBase', 'GasVentScrubberAlt2']]} }

  .desc = { ent-{'sequences': [['AirSensorFreezerBase', 'GasVentScrubber'], ['AirSensorFreezerBase', 'GasVentScrubberAlt1'], ['AirSensorFreezerBase', 'GasVentScrubberAlt2']]}.desc }
ent-GasVentPumpFreezer = { ent-AirSensorFreezerBase }
    .desc = { ent-AirSensorFreezerBase.desc }
ent-GasVentScrubberFreezer = { ent-AirSensorFreezerBase }
    .desc = { ent-AirSensorFreezerBase.desc }
ent-AirAlarmFreezer = { ent-AirAlarm }
    .suffix = Атмосфера морозильной камеры, режим авто отключён
    .desc = { ent-AirAlarm.desc }
