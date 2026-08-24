ent-GasPipeSansLayersIndestructible = gas pipe
    .desc = Holds gas.
ent-GasPipeSansLayers = gas pipe
    .desc = Holds gas.
ent-BaseGasPipeLayers = { "" }
    .desc = { "" }
ent-GasPipeBase = труба
    .desc = Содержит газ.
ent-{'values': ['GasPipeHalf', 'GasPipeHalfAlt1', 'GasPipeHalfAlt2']} = { ent-GasPipeBase }

  .suffix = Half
  .desc = { ent-GasPipeBase.desc }
ent-{'values': ['GasPipeStraight', 'GasPipeStraightAlt1', 'GasPipeStraightAlt2']} = { ent-GasPipeBase }

  .suffix = Straight
  .desc = { ent-GasPipeBase.desc }
ent-{'values': ['GasPipeBend', 'GasPipeBendAlt1', 'GasPipeBendAlt2']} = { ent-GasPipeBase }

  .suffix = Bend
  .desc = { ent-GasPipeBase.desc }
ent-{'values': ['GasPipeTJunction', 'GasPipeTJunctionAlt1', 'GasPipeTJunctionAlt2']} = { ent-GasPipeBase }

  .suffix = TJunction
  .desc = { ent-GasPipeBase.desc }
ent-{'values': ['GasPipeFourway', 'GasPipeFourwayAlt1', 'GasPipeFourwayAlt2']} = { ent-GasPipeBase }

  .suffix = Fourway
  .desc = { ent-GasPipeBase.desc }
ent-GasPipeHalf = { ent-GasPipeBase }
    .suffix = Половинная
    .desc = { ent-GasPipeBase.desc }
ent-GasPipeStraight = { ent-GasPipeBase }
    .suffix = Прямая
    .desc = { ent-GasPipeBase.desc }
ent-GasPipeBend = { ent-GasPipeBase }
    .suffix = Угловая
    .desc = { ent-GasPipeBase.desc }
ent-GasPipeTJunction = { ent-GasPipeBase }
    .suffix = Т-образная
    .desc = { ent-GasPipeBase.desc }
ent-GasPipeFourway = { ent-GasPipeBase }
    .suffix = Четверная
    .desc = { ent-GasPipeBase.desc }
ent-GasPipeBroken = сломанная труба
    .desc = Раньше в ней содержался газ.
ent-GasPipeManifold = gas pipe manifold
    .desc = Allows gas pipes of different configurations to be connected together.
