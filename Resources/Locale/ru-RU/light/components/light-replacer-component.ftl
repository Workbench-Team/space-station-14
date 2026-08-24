### Interaction Messages

# Shown when player tries to replace light, but there is no lights left
comp-light-replacer-missing-light = В { $light-replacer } не осталось лампочек.
# Shown when player inserts light bulb inside light replacer
comp-light-replacer-insert-light = Вы вставили { $bulb } в { $light-replacer }.
# Shown when a player attempts to replace a light with the same color & type as the active light.
comp-light-replacer-same-light = This fixture already holds { INDEFINITE($light) } { $light }!
# Radial Menu messages
comp-light-replacer-eject-specified-lights = Eject all { MAKEPLURAL($light) }.
comp-light-replacer-select-lights = Select { MAKEPLURAL($light) }.
comp-light-replacer-open-empty = { CAPITALIZE(THE($light-replacer)) } is completely empty!
# Label
comp-light-replacer-label =
    Tube: { $tube }
    Bulb: { $bulb }
# Shown when player tries to insert in light replacer brolen light bulb
comp-light-replacer-insert-broken-light = Вы не можете вставлять разбитые лампочки!
# Shown when player refill light from light box
comp-light-replacer-refill-from-storage = Вы пополнили { $light-replacer }.
comp-light-replacer-no-lights = It's empty.
comp-light-replacer-has-lights = It contains the following:
comp-light-replacer-light-listing =
    { $amount ->
        [one] [color=yellow]{ $amount }[/color] [color=gray]{ $name }[/color]
       *[other] [color=yellow]{ $amount }[/color] [color=gray]{ $name }s[/color]
    }
# Bulbs
comp-light-bulb-incandescent = incandescent
comp-light-bulb-dim = dim
comp-light-bulb-warm = warm
comp-light-bulb-service = service
# Tubes
comp-light-bulb-fluorescent = fluorescent
comp-light-bulb-exterior = exterior
comp-light-bulb-sodium = sodium
# Both
comp-light-bulb-old = old
comp-light-bulb-led = led
comp-light-bulb-cyan = cyan
comp-light-bulb-blue = blue
comp-light-bulb-yellow = yellow
comp-light-bulb-pink = pink
comp-light-bulb-orange = orange
comp-light-bulb-black = black
comp-light-bulb-red = red
comp-light-bulb-green = green
