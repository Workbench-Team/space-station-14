comp-kitchen-spike-begin-hook-self = Вы начинаете насаживать себя на { $hook }!
comp-kitchen-spike-begin-hook-self-other = { CAPITALIZE($victim) } начинает насаживать себя на { $hook }!
comp-kitchen-spike-begin-hook-other-self = Вы начинаете насаживать { CAPITALIZE($victim) } на { $hook }!
comp-kitchen-spike-begin-hook-other = { CAPITALIZE($user) } начинает насаживать { CAPITALIZE($victim) } на { $hook }!
comp-kitchen-spike-hook-self = Вы бросились на { $hook }!
comp-kitchen-spike-hook-self-other =
    { CAPITALIZE($victim) } { GENDER($victim) ->
        [male] бросился
        [female] бросилась
        [epicene] бросились
       *[neuter] бросилось
    } на { $hook }!
comp-kitchen-spike-hook-other-self = Вы бросили { CAPITALIZE($victim) } на { $hook }!
comp-kitchen-spike-hook-other =
    { CAPITALIZE($user) } { GENDER($user) ->
        [male] бросил
        [female] бросила
        [epicene] бросили
       *[neuter] бросило
    } { CAPITALIZE($victim) } на { $hook }!
comp-kitchen-spike-begin-unhook-self = Вы начинаете срываться с { $hook }!
comp-kitchen-spike-begin-unhook-self-other = { CAPITALIZE($victim) } начинает срываться с { $hook }!
comp-kitchen-spike-begin-unhook-other-self = Вы начинаете срывать { CAPITALIZE($victim) } с { $hook }!
comp-kitchen-spike-begin-unhook-other = { CAPITALIZE($user) } начинает срывать { CAPITALIZE($victim) } с { $hook }!
comp-kitchen-spike-unhook-self = Вы сорвали себя с { $hook }!
comp-kitchen-spike-unhook-self-other =
    { CAPITALIZE($victim) } { GENDER($victim) ->
        [male] сорвал
        [female] сорвала
        [epicene] сорвали
       *[neuter] сорвало
    } себя с { $hook }!
comp-kitchen-spike-unhook-other-self = Вы сорвали { CAPITALIZE($victim) } с { $hook }!
comp-kitchen-spike-unhook-other =
    { CAPITALIZE($user) } { GENDER($user) ->
        [male] сорвал
        [female] сорвала
        [epicene] сорвали
       *[neuter] сорвало
    } { CAPITALIZE($victim) } с { $hook }!
comp-kitchen-spike-begin-butcher-self = Вы начинаете разделывать { $victim }!
comp-kitchen-spike-begin-butcher = { CAPITALIZE($user) } начинает разделывать { $victim }!
comp-kitchen-spike-butcher-self = Вы разделали { $victim }!
comp-kitchen-spike-butcher =
    { CAPITALIZE($user) } { GENDER($user) ->
        [male] разделал
        [female] разделала
        [epicene] разделали
       *[neuter] разделало
    } { $victim }!
comp-kitchen-spike-unhook-verb = Отцепить
comp-kitchen-spike-hooked = [color=red]{ CAPITALIZE($victim) } на этом мясном крюке![/color]
comp-kitchen-spike-meat-name = { $name } ({ $victim })
comp-kitchen-spike-victim-examine = [color=orange]{ CAPITALIZE(SUBJECT($target)) } выглядит довольно { GENDER($target) ->
        [male] худощавым
        [female] худощавой
        [epicene] худощавыми
       *[neuter] худощавым
    }.[/color]
