objectives-round-end-result =
    { $count ->
        [one] Был один { $agent }.
       *[other] Было { $count } { $agent }.
    }
objectives-player-user-named = [color=White]{ $name }[/color] ([color=gray]{ $user }[/color])
objectives-player-user = [color=gray]{ $user }[/color]
objectives-player-named = [color=White]{ $name }[/color]
objectives-no-objectives = { $title } был(а) { $agent }.
objectives-with-objectives = { $title } был(а) { $agent }, у которого(-ой) были следующие цели:
objectives-objective-success = { $objective } | [color={ $markupColor }]Успех![/color]
objectives-objective-fail = { $objective } | [color={ $markupColor }]Провал![/color] ({ $progress }%)
