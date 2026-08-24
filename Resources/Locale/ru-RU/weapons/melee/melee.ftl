melee-inject-failed-hardsuit = Ваше { $weapon } не может проникать через костюмы!
melee-balloon-pop = { CAPITALIZE(THE($balloon)) } popped!
melee-weapon-dealt-no-damage = { CAPITALIZE(THE($weapon)) } is not damaging { THE($target) }!
melee-self-weapon-dealt-no-damage = You are not damaging { THE($target) }!
# MeleeBatteryHitsLeftSystem
examine-battery-hits-left = It has enough charge for [color={ $color }]{ $count }[/color] hits.
# BatteryComponent
melee-battery-examine =
    Заряда хватит на [color={ $color }]{ $count }[/color] { $count ->
        [one] удар
        [few] удара
       *[other] ударов
    }.
