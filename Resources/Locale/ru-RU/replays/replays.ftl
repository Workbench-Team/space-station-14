# Loading Screen

replay-loading = Загрузка ({ $cur }/{ $total })
replay-loading-reading = Чтение файлов
replay-loading-processing = Обработка файлов
replay-loading-spawning = Спавн энтити
replay-loading-initializing = Инициализация энтити
replay-loading-starting = Запуск энтити
replay-loading-failed =
    Не удалось загрузить повтор:
    { $reason }
replay-loading-retry = Попытка загрузить с большей допустимостью исключений МОЖЕТ ВЫЗВАТЬ БАГИ!
# Main Menu
replay-menu-subtext = Клиент повтора
replay-menu-load = Загрузить выбранный повтор
replay-menu-select = Выбрать повтор
replay-menu-open = Открыть папку повторов
replay-menu-none = Ни одного повтора не найдено.
# Main Menu Info Box
replay-info-title = Информация о повторе
replay-info-none-selected = Повтор не выбран
replay-info-invalid = [color=red]Выбран недействительный повтор[/color]
replay-info-info =
    { "[" }color=gray]Выбран:[/color]  { $name } ({ $file })
    { "[" }color=gray]Время:[/color]   { $time }
    { "[" }color=gray]ID Раунда:[/color]   { $roundId }
    { "[" }color=gray]Продолжительность:[/color]   { $duration }
    { "[" }color=gray]ID Ветки:[/color]   { $forkId }
    { "[" }color=gray]Версия:[/color]   { $version }
    { "[" }color=gray]Версия движка:[/color]   { $engVersion }
    { "[" }color=gray]Тип Хэша:[/color]   { $hash }
    { "[" }color=gray]Скомп Хэш:[/color]   { $compHash }
# Replay selection window
replay-menu-select-title = Выбрать повтор
# Replay related verbs
replay-verb-spectate = Наблюдать
# command
cmd-replay-spectate-help = наблюдать_за_повтором [опциональное энтити]
cmd-replay-spectate-desc = Прикрепляет или открепляет локального игрока к заданному uid энтити.
cmd-replay-spectate-hint = Опциональный EntityUid
