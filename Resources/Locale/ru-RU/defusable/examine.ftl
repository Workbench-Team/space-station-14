defusable-examine-defused = { CAPITALIZE($name) } [color=lime]обезврежена[/color].
defusable-examine-live = { CAPITALIZE($name) } [color=red]тикает[/color] и на таймере [color=red]{ $time }[/color] секунд.
defusable-examine-live-display-off = { CAPITALIZE($name) } [color=red]тикает[/color], и, похоже, таймер выключен.
defusable-examine-inactive = { CAPITALIZE($name) } [color=lime]не активна[/color], но всё ещё может быть взведена.
defusable-examine-bolts =
    Болты { $down ->
        [true] [color=red]опущены[/color]
       *[false] [color=green]подняты[/color]
    }.
