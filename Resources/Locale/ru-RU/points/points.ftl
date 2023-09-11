point-scoreboard-winner = Победителем стал(а) [color=lime]{ $player }![/color]
point-scoreboard-header = [bold]Табло[/bold]
point-scoreboard-list =
    { $place }. [bold][color=cyan]{ $name }[/color][/bold] набрал(а) [color=yellow]{ $points ->
        [one] { $points } очка
       *[other] { $points } очков
    }.[/color]
