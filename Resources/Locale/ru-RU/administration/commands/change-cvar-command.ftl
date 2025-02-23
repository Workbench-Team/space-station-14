cmd-changecvar-no-arguments = Вы должны указать квар.
cmd-changecvar-cvar-not-registered = Квар { $cvar } не зарегистрирован.
cmd-changecvar-cvar-not-allowed = Вы не можете изменить этот квар.
cmd-changecvar-value-out-of-range = Значение выходит за пределы диапазона. Диапазон составляет от { $min } до { $max }.
cmd-changecvar-desc = Изменяет значение квара.
cmd-changecvar-help = Использование: changecvar <cvar | ? | search> <значение>
cmd-changecvar-available-cvars = Перечисление доступных кваров:
cmd-changecvar-no-cvars = Не найдено ни одного квара, который можно было бы изменить.
cmd-changecvar-success = CVar { $cvar } изменён с "{ $old }" на "{ $value }".
cmd-changecvar-search-no-arguments = Вы должны указать искомый запрос.
cmd-changecvar-search-no-matches = Не найдено ни одного квара, соответствующего запросу.
cmd-changecvar-search-matches =
    { $count ->
        [one] Найден
       *[other] Найдено
    } { $count } { $count ->
        [one] квар
        [few] квара
       *[other] кваров
    }, соответствующих поисковому запросу:
cmd-changecvar-arg-name = <название | ? | search>
