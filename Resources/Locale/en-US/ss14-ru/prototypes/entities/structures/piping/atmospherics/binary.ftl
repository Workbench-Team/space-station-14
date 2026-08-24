ent-GasBinaryBase = { ent-GasPipeBase }
    .desc = { ent-GasPipeBase.desc }
ent-{'values': ['GasPressurePump', 'GasPressurePumpAlt1', 'GasPressurePumpAlt2']} = gas pump

  .desc = A pump that moves gas by pressure.
ent-{'values': ['GasVolumePump', 'GasVolumePumpAlt1', 'GasVolumePumpAlt2']} = volumetric gas pump

  .desc = A pump that moves gas by volume.
ent-{'values': ['GasPressureRegulator', 'GasPressureRegulatorAlt1', 'GasPressureRegulatorAlt2']} = inlet pressure regulator

  .desc = A valve that releases gas when the inlet pressure exceeds a certain threshold.
ent-{'values': ['GasPassiveGate', 'GasPassiveGateAlt1', 'GasPassiveGateAlt2']} = passive gate

  .desc = A one-way air valve that does not require power.
ent-{'values': ['GasValve', 'GasValveAlt1', 'GasValveAlt2']} = manual valve

  .desc = A pipe with a valve that can be used to disable the flow of gas through it.
ent-{'values': ['SignalControlledValve', 'SignalControlledValveAlt1', 'SignalControlledValveAlt2']} = signal valve

  .desc = A pipe with a valve that can be controlled with signals.
ent-{'values': ['GasPort', 'GasPortAlt1', 'GasPortAlt2']} = connector port

  .desc = For connecting portable devices related to atmospherics control.
ent-{'values': ['GasDualPortVentPump', 'GasDualPortVentPumpAlt1', 'GasDualPortVentPumpAlt2']} = dual-port air vent

  .desc = Has a valve and a pump attached to it. There are two ports, one is an input for releasing air, the other is an output when siphoning.
ent-{'values': ['GasRecycler', 'GasRecyclerAlt1', 'GasRecyclerAlt2']} = gas recycler

  .desc = Recycles carbon dioxide and nitrous oxide. Heater and compressor not included.
ent-HeatExchanger = radiator
    .desc = Transfers heat between the pipe and its surroundings.
ent-HeatExchangerBend = { ent-HeatExchanger }
    .suffix = Bend
    .desc = { ent-HeatExchanger.desc }
