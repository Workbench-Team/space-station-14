# ban
cmd-ban-desc = Банит кого-то
cmd-ban-help = Использование: ban <name или user ID> <причина> [длительность в минутах, укажите 0 или оставьте пустым для блокировки навсегда]
cmd-ban-player = Не удалось найти игрока с таким никнеймом.
cmd-ban-invalid-minutes = ${ minutes } не является допустимым количеством минут!
cmd-ban-invalid-severity = ${ severity } недопустимая тяжесть!
cmd-ban-invalid-arguments = Недопустимое количество аргументов
cmd-ban-hint = <name/user ID>
cmd-ban-hint-reason = <причина>
cmd-ban-hint-severity = [тяжесть]
cmd-ban-hint-duration = [срок]
cmd-ban-hint-duration-1 = Навсегда
cmd-ban-hint-duration-2 = 1 день
cmd-ban-hint-duration-3 = 3 дня
cmd-ban-hint-duration-4 = 1 неделя
cmd-ban-hint-duration-5 = 2 недели
# ban panel
cmd-banpanel-desc = Открывает панель бана
cmd-banpanel-help = Использование: banpanel [никнейм или ID пользователя]
cmd-banpanel-server = Это не может быть использовано в консоли сервера
cmd-banpanel-player-err = Указанный игрок не найден
cmd-ban-hint-duration-6 = 1 месяц
# listbans
cmd-banlist-desc = Выводит список активных блокировок игрока
cmd-banlist-help = Использование: banlist <никнейм или ID пользователя>
cmd-banlist-empty = Нет активных блокировок для { $user }
cmd-banlistF-hint = <никнейм/ID пользователя>
cmd-ban_exemption_update-desc = Установить исключение для типа бана для игрока.
cmd-ban_exemption_update-help =
    Использование: ban_exemption_update <игрок> <флаг> [<флаг> [...]]
    Укажите несколько флагов, чтобы дать игроку несколько флагов исключения из банов.
    Чтобы удалить все исключения, выполните эту команду, указав в качестве единственного флага "None".
cmd-ban_exemption_update-nargs = Ожидалось минимум 2 аргумента
cmd-ban_exemption_update-locate = Невозможно определить местонахождение игрока '{ $player }'.
cmd-ban_exemption_update-invalid-flag = Недопустимый флаг '{ $flag }'.
cmd-ban_exemption_update-success = Обновлены флаги исключения бана для '{ $player }' ({ $uid }).
cmd-ban_exemption_update-arg-player = <игрок>
cmd-ban_exemption_update-arg-flag = <флаг>
cmd-ban_exemption_get-desc = Показать исключения из банов для определённого игрока.
cmd-ban_exemption_get-help = Использование: ban_exemption_get <игрок>
cmd-ban_exemption_get-nargs = Ожидался всего 1 аргумент
cmd-ban_exemption_get-none = Пользователь не имеет никаких исключений из банов.
cmd-ban_exemption_get-show = Пользователь имеет следующие флаги исключения из банов: { $flags }.
# Ban panel
ban-panel-title = Панель бана
ban-panel-player = Игрок
ban-panel-ip = IP
ban-panel-hwid = HWID
ban-panel-reason = Причина
ban-panel-last-conn = Использовать IP и HWID с последнего подключения?
ban-panel-submit = Забанить
ban-panel-confirm = Вы уверены?
ban-panel-tabs-basic = Основн инфо
ban-panel-tabs-reason = Причина
ban-panel-tabs-players = Список игрков
ban-panel-tabs-role = Инфо о запрете роли
ban-panel-no-data = Для бана необходимо указать пользователя, IP или HWID.
ban-panel-invalid-ip = Не удалось разобрать IP-адрес. Пожалуйста, попробуйте еще раз
ban-panel-select = Выберите тип
ban-panel-server = Бан
ban-panel-role = Бан роли
ban-panel-minutes = Минут
ban-panel-hours = Часов
ban-panel-days = Дней
ban-panel-weeks = Недель
ban-panel-months = Месяцев
ban-panel-years = Лет
ban-panel-permanent = Навсегда
ban-panel-ip-hwid-tooltip = Оставьте пустым и установите флажок ниже, чтобы использовать данные с последнего подключения
ban-panel-severity = Тяжесть:
# Ban string
server-ban-string = { $admin } выдал бан { $severity } тяжести, который истекает { $expires } для [{ $name }, { $ip }, { $hwid }], по причине: { $reason }
ban-panel-erase = Стереть сообщения в чате и игрока из раунда
server-ban-string-never = никогда
server-ban-string-no-pii = { $admin } выдал бан { $severity } тяжести, который истекает { $expires } для { $name } по причине: { $reason }
cmd-ban_exemption_get-arg-player = <игрок>
