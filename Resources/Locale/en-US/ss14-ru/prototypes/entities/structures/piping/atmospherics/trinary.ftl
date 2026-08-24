ent-GasTrinaryBase = { ent-GasPipeBase }
    .desc = { ent-GasPipeBase.desc }
ent-{'values': ['GasFilter', 'GasFilterAlt1', 'GasFilterAlt2']} = gas filter

  .desc = Very useful for filtering gases.
ent-{'values': ['GasFilterFlipped', 'GasFilterFlippedAlt1', 'GasFilterFlippedAlt2']} = gas filter

  .suffix = Flipped
  .desc = { ent-GasFilter.desc }
ent-{'values': ['GasMixer', 'GasMixerAlt1', 'GasMixerAlt2']} = gas mixer

  .desc = Very useful for mixing gases.
ent-{'values': ['GasMixerFlipped', 'GasMixerFlippedAlt1', 'GasMixerFlippedAlt2']} = gas mixer

  .suffix = Flipped
  .desc = { ent-GasMixer.desc }
ent-{'values': ['PressureControlledValve', 'PressureControlledValveAlt1', 'PressureControlledValveAlt2']} = pneumatic valve

  .desc = A bidirectional valve controlled by pressure. Opens if the output pipe is lower than the pressure of the control pipe by 101.325 kPa.