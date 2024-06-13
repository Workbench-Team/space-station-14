whitelist-not-whitelisted = Вас нет в белом списке.
# proper handling for having a min/max or not
whitelist-playercount-invalid =
    { $min ->
        [0] Белый список для этого сервера применяется только при количестве игроков меньше { $max }.
       *[other]
            Белый список для этого сервера применяется только при количестве игроков больше { $min } { $max ->
                [2147483647] -> так что, возможно, Вы сможете присоединиться позже.
               *[other] -> и меньше { $max }, так что ,возможно, Вы сможете присоединиться позже.
            }
    }
whitelist-not-whitelisted-rp = Вас нет в белом списке. Чтобы попасть в белый список, посетите наш Discord.
whitelist-no-reserve-slot = У вас отсутствует резервный слот. Вы можете его приобрести через донат-магазин перейдя в Discord.
cmd-whitelistadd-desc = Добавляет игрока с указанным именем в белый список.
cmd-whitelistadd-help = Использование: whitelistadd <username>
cmd-whitelistadd-existing = { $username } уже в белом списке!
cmd-whitelistadd-added = { $username } добавлен в белый список.
cmd-whitelistadd-not-found = Пользователь '{ $username }' не найден.
cmd-whitelistadd-arg-player = [player]
cmd-whitelistremove-desc = Удаляет игрока с указанным именем из белого списка сервера.
cmd-whitelistremove-help = Использование: whitelistremove <username>
cmd-whitelistremove-existing = { $username } не в белом списке!
cmd-whitelistremove-removed = Пользователь { $username } удалён из белого списка.
cmd-whitelistremove-not-found = Пользователь '{ $username }' не найден.
cmd-whitelistremove-arg-player = [player]
cmd-kicknonwhitelisted-desc = Кикает с сервера всех пользователей не из белого списка.
cmd-kicknonwhitelisted-help = Использование: kicknonwhitelisted
ban-banned-permanent = Этот бан можно только обжаловать.
ban-banned-permanent-appeal = Этот бан можно только обжаловать. Вы можете подать обжалование на { $link }
ban-expires = Вы получили бан на { $duration } минут, и он истечёт { $time } по UTC (для московского времени добавьте 3 часа).
ban-banned-1 = Вам, или другому пользователю этого компьютера или соединения, запрещено здесь играть.
ban-banned-2 = Причина бана: "{ $reason }"
ban-banned-3 = Попытки обойти этот бан через создание новых аккаунтов будут залогированы.
discord-expires-at = до { $date }
discord-permanent = навсегда
discord-ban-msg = Игрок { $username } забанен { $expires } по причине: { $reason }
discord-jobban-msg = Игроку { $username } заблокирована роль { $role } { $expires } по причине: { $reason }
discord-departmentban-msg = Игроку { $username } заблокирован департамент { $department } { $expires } по причине: { $reason }
soft-player-cap-full = Сервер заполнен!
panic-bunker-account-denied = Этот сервер находится в режиме бункера паники, который часто включается в качестве меры предосторожности против набегаторов. Новые подключения от учётных записей, не соответствующих определённым требованиям, временно не принимаются. Повторите попытку позже
panic-bunker-account-denied-reason = Этот сервер находится в режиме бункера паники, который часто включается в качестве меры предосторожности против набегаторов. Новые подключения учётных записей, не соответствующих определённым требованиям, временно не принимаются. Повторите попытку позже. Причина: "{ $reason }"
panic-bunker-account-reason-account = Ваша учётная запись Space Station 14 слишком новая. Она должна быть старше { $minutes } минут
panic-bunker-account-reason-overall =
    Ваше общее игровое время на сервере должно быть больше { $minutes } { $minutes ->
        [one] минуты
       *[other] минут
    }.
baby-jail-account-denied = Этот сервер - сервер для новичков, предназначенный для новых игроков и тех, кто хочет им помочь. Новые подключения слишком старых или не внесенных в белый список аккаунтов не принимаются. Загляните на другие серверы и посмотрите все, что может предложить Space Station 14. Веселитесь!
baby-jail-account-denied-reason = Этот сервер - сервер для новичков, предназначенный для новых игроков и тех, кто хочет им помочь. Новые подключения слишком старых или не внесенных в белый список аккаунтов не принимаются. Загляните на другие серверы и посмотрите все, что может предложить Space Station 14. Веселитесь! Причина: "{ $reason }"
baby-jail-account-reason-account = Ваша учетная запись Space Station 14 слишком старая. Она должен быть моложе { $minutes } минут
baby-jail-account-reason-overall = Ваше общее игровое время на сервере должно быть меньше { $minutes } { $minutes ->
        [one] минуты
       *[other] минут
    }.
