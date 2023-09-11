objectives-round-end-result =
    { $count ->
        [one] Был один { $agent }.
       *[other] Было { $count } { MAKEPLURAL($agent) }.
    }
objectives-player-user-named = [color=White]{ $name }[/color] ([color=gray]{ $user }[/color])
objectives-player-user = [color=gray]{ $user }[/color]
objectives-player-named = [color=White]{ $name }[/color]
objectives-no-objectives = { $title } был(а) { $agent }.
objectives-with-objectives = { $title } был(а) { $agent }, у которого(-ой) были следующие цели:
objectives-condition-success = { $condition } | [color={ $markupColor }]Успех![/color]
objectives-condition-fail = { $condition } | [color={ $markupColor }]Провал![/color] ({ $progress }%)
