ent-GasPipeSansLayersIndestructible = gas pipe
    .desc = Holds gas.
ent-GasPipeSansLayers = { ent-GasPipeSansLayersIndestructible }
    .desc = { ent-GasPipeSansLayersIndestructible.desc }
ent-BaseGasPipeLayers = { "" }
    .desc = { "" }
ent-GasPipeBase = { ent-GasPipeSansLayers }
    .desc = { ent-GasPipeSansLayers.desc }
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
ent-GasPipeBroken = broken pipe
    .desc = It used to hold gas.
ent-GasPipeManifold = gas pipe manifold
    .desc = Allows gas pipes of different configurations to be connected together.
