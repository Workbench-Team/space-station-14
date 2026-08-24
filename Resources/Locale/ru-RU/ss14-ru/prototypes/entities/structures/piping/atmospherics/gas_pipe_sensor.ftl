ent-{'values': ['GasPipeSensor', 'GasPipeSensorAlt1', 'GasPipeSensorAlt2']} = gas pipe sensor

  .desc = Reports on the status of the gas in the attached pipe network.
ent-{'values': ['GasPipeSensorDistribution', 'GasPipeSensorDistributionAlt1', 'GasPipeSensorDistributionAlt2']} = { ent-{'values': ['GasPipeSensor', 'GasPipeSensorAlt1', 'GasPipeSensorAlt2']} }

  .suffix = Distribution
  .desc = { ent-{'values': ['GasPipeSensor', 'GasPipeSensorAlt1', 'GasPipeSensorAlt2']}.desc }
ent-{'values': ['GasPipeSensorWaste', 'GasPipeSensorWasteAlt1', 'GasPipeSensorWasteAlt2']} = { ent-{'values': ['GasPipeSensor', 'GasPipeSensorAlt1', 'GasPipeSensorAlt2']} }

  .suffix = Waste
  .desc = { ent-{'values': ['GasPipeSensor', 'GasPipeSensorAlt1', 'GasPipeSensorAlt2']}.desc }
ent-{'values': ['GasPipeSensorMixedAir', 'GasPipeSensorMixedAirAlt1', 'GasPipeSensorMixedAirAlt2']} = { ent-{'values': ['GasPipeSensor', 'GasPipeSensorAlt1', 'GasPipeSensorAlt2']} }

  .suffix = Mixed air
  .desc = { ent-{'values': ['GasPipeSensor', 'GasPipeSensorAlt1', 'GasPipeSensorAlt2']}.desc }
ent-{'values': ['GasPipeSensorTEGHot', 'GasPipeSensorTEGHotAlt1', 'GasPipeSensorTEGHotAlt2']} = { ent-{'values': ['GasPipeSensor', 'GasPipeSensorAlt1', 'GasPipeSensorAlt2']} }

  .suffix = TEG hot
  .desc = { ent-{'values': ['GasPipeSensor', 'GasPipeSensorAlt1', 'GasPipeSensorAlt2']}.desc }
ent-{'values': ['GasPipeSensorTEGCold', 'GasPipeSensorTEGColdAlt1', 'GasPipeSensorTEGColdAlt2']} = { ent-{'values': ['GasPipeSensor', 'GasPipeSensorAlt1', 'GasPipeSensorAlt2']} }

  .suffix = TEG cold
  .desc = { ent-{'values': ['GasPipeSensor', 'GasPipeSensorAlt1', 'GasPipeSensorAlt2']}.desc }ent-GasPipeSensor = датчик газовой трубы
    .desc = Сообщает о состоянии газа в присоединенной сети труб.
ent-GasPipeSensorDistribution = { ent-GasPipeSensor }
    .suffix = Распределительный
    .desc = { ent-GasPipeSensor.desc }
ent-GasPipeSensorWaste = { ent-GasPipeSensor }
    .suffix = Утилизационный
    .desc = { ent-GasPipeSensor.desc }
ent-GasPipeSensorMixedAir = { ent-GasPipeSensor }
    .suffix = Смешанный воздух
    .desc = { ent-GasPipeSensor.desc }
ent-GasPipeSensorTEGHot = { ent-GasPipeSensor }
    .suffix = ТЭГ горячий
    .desc = { ent-GasPipeSensor.desc }
ent-GasPipeSensorTEGCold = { ent-GasPipeSensor }
    .suffix = ТЭГ холодный
    .desc = { ent-GasPipeSensor.desc }
