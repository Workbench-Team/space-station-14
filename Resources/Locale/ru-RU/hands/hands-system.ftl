# Examine text after when they're holding something (in-hand)
comp-hands-examine = { CAPITALIZE(SUBJECT($user)) } удерживает { $items }.
comp-hands-examine-empty = { CAPITALIZE(SUBJECT($user)) } ничего не держит в руках.
comp-hands-examine-wrapper = [color=paleturquoise]{ $item }[/color]
hands-system-blocked-by = Заблокировано
