discord-watchlist-connection-header =
    { $players ->
        [one] { $players } игрок из списка наблюдения подключился
       *[other] { $players } игроки из списка наблюдения подключились
    } к { $serverName }
discord-watchlist-connection-entry =
    - { $playerName } с сообщением "{ $message }"{ $expiry ->
        [0] { "" }
       *[other] { " " }(истекает <t:{ $expiry }:R>)
    }{ $otherWatchlists ->
        [0] { "" }
        [one] { " " }и { $otherWatchlists } другой заметкой наблюдения
       *[other] { " " }и { $otherWatchlists } других заметок наблюдения
    }
