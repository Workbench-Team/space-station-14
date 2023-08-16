ent-BaseDefibrillator = defibrillator
    .desc = CLEAR! Zzzzat!
    .suffix = { "" }
ent-Defibrillator = { ent-['BaseDefibrillator', 'PowerCellSlotMediumItem'] }

  .desc = { ent-['BaseDefibrillator', 'PowerCellSlotMediumItem'].desc }
  .suffix = { "" }
ent-DefibrillatorEmpty = { ent-Defibrillator }
    .suffix = Empty
    .desc = { ent-Defibrillator.desc }
ent-DefibrillatorOneHandedUnpowered = { ent-BaseDefibrillator }
    .suffix = One-Handed, Unpowered
    .desc = { ent-BaseDefibrillator.desc }
