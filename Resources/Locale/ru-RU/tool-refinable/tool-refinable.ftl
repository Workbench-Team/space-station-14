refined-butchered-success = Вы разрезаете { $target } используя { $tool }.
refined-butchered-success-others = { $user } разрезал { $target } используя { $tool }.
refined-slice-verb-message-default = Разрезать острым предметом.
refined-slice-verb-target-isnt-dead = Цель должна быть мертва.
refined-slice-verb-message-tool = Вам понадобится другой инструмент для разрезания { $target }.

refined-screwed-success = Вы разбираете { $target } с помощью { $tool }.
refined-screwed-success-others = { CAPITALIZE($user) } { GENDER($user) ->
    [male] разобрал
    [female] разобрала
    [epicene] разобраи
    *[neuter] разобрало
} { $target } с помощью { $tool }.
refined-screwed-verb-message-default = Открутить и разобрать объект.
refined-screwed-verb-message-tool = Вам нужен другой инструмент, чтобы открутить { $target }.

refined-slice-verb-name = Разрезать
refined-screw-verb-name = Закручивать
refined-butchered-verb-name = Разделать

tool-refinable-component-hint = { CAPITALIZE(SUBJECT($target)) } может быть обработан [color=cyan]{ $quality }[/color].
tool-refinable-component-hint-slicing = { CAPITALIZE(SUBJECT($target)) } может быть разрезан используя инструмент с возможностью [color=cyan]{ $quality }[/color].
tool-refinable-component-hint-butchering = { CAPITALIZE(SUBJECT($target)) } может быть разделан используя инструмент с возможностью [color=cyan]{ $quality }[/color].
tool-refinable-component-hint-screwing = { CAPITALIZE(SUBJECT($target)) } может быть разобран используя инструмент с возможностью [color=cyan]{ $quality }[/color].
