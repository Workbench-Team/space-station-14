ent-GasUnaryBase = { ent-GasPipeBase }
    .desc = { ent-GasPipeBase.desc }
ent-{'values': ['GasVentPump', 'GasVentPumpAlt1', 'GasVentPumpAlt2']} = air vent

  .desc = Has a valve and a pump attached to it.
ent-{'values': ['GasPassiveVent', 'GasPassiveVentAlt1', 'GasPassiveVentAlt2']} = passive vent

  .desc = It's an open vent.
ent-{'values': ['GasVentScrubber', 'GasVentScrubberAlt1', 'GasVentScrubberAlt2']} = air scrubber

  .desc = Has a valve and pump attached to it.
ent-{'values': ['GasOutletInjector', 'GasOutletInjectorAlt1', 'GasOutletInjectorAlt2']} = air injector

  .desc = Has a valve and pump attached to it.
ent-BaseGasThermoMachine = thermomachine
    .desc = { ent-BaseMachinePowered.desc }
ent-{'values': ['GasThermoMachineFreezer', 'GasThermoMachineFreezerAlt1', 'GasThermoMachineFreezerAlt2']} = freezer

  .desc = Cools gas in connected pipes.
ent-{'values': ['GasThermoMachineFreezerEnabled', 'GasThermoMachineFreezerEnabledAlt1', 'GasThermoMachineFreezerEnabledAlt2']} = { ent-{'values': ['GasThermoMachineFreezer', 'GasThermoMachineFreezerAlt1', 'GasThermoMachineFreezerAlt2']} }

  .suffix = Enabled
  .desc = { ent-{'values': ['GasThermoMachineFreezer', 'GasThermoMachineFreezerAlt1', 'GasThermoMachineFreezerAlt2']}.desc }
ent-{'values': ['GasThermoMachineHeater', 'GasThermoMachineHeaterAlt1', 'GasThermoMachineHeaterAlt2']} = heater

  .desc = Heats gas in connected pipes.
ent-{'values': ['GasThermoMachineHeaterEnabled', 'GasThermoMachineHeaterEnabledAlt1', 'GasThermoMachineHeaterEnabledAlt2']} = { ent-{'values': ['GasThermoMachineHeater', 'GasThermoMachineHeaterAlt1', 'GasThermoMachineHeaterAlt2']} }

  .suffix = Enabled
  .desc = { ent-{'values': ['GasThermoMachineHeater', 'GasThermoMachineHeaterAlt1', 'GasThermoMachineHeaterAlt2']}.desc }
ent-{'values': ['GasThermoMachineHellfireFreezer', 'GasThermoMachineHellfireFreezerAlt1', 'GasThermoMachineHellfireFreezerAlt2']} = hellfire freezer

  .desc = An advanced machine that cools gas in connected pipes. Has the side effect of chilling the surrounding area. Cold as Hell!
ent-{'values': ['GasThermoMachineHellfireHeater', 'GasThermoMachineHellfireHeaterAlt1', 'GasThermoMachineHellfireHeaterAlt2']} = hellfire heater

  .desc = An advanced machine that heats gas in connected pipes. Has the side effect of leaking heat into the surrounding area. Hot as Hell!
ent-{'values': ['BaseGasCondenser', 'BaseGasCondenserAlt1', 'BaseGasCondenserAlt2']} = condenser

  .desc = Condenses gases into liquids. Now we just need some plumbing.