## Survivor

roles-antag-survivor-name = Выживший
# It's a Halo reference
roles-antag-survivor-objective = Текущая цель: Выжить
survivor-role-greeting =
    Вы - выживший.
    Прежде всего, вам нужно вернуться на Центком живым.
    Соберите столько огнестрельного оружия, сколько необходимо, чтобы гарантировать своё выживание.
    Никому не доверяйте.
survivor-round-end-dead-count =
    { $deadCount ->
        [one] [color=red]{ $deadCount }[/color] выживший умер.
       *[other] [color=red]{ $deadCount }[/color] выживших умерло.
    }
survivor-round-end-alive-count =
    { $aliveCount ->
        [one] [color=yellow]{ $aliveCount }[/color] выживший остался на станции.
       *[other] [color=yellow]{ $aliveCount }[/color] выживших осталось на станции.
    }
survivor-round-end-alive-on-shuttle-count =
    { $aliveCount ->
        [one] [color=green]{ $aliveCount }[/color] выжившему удалось выбрался живым.
       *[other] [color=green]{ $aliveCount }[/color] Выжившим удалось выбраться живыми.
    }

## Wizard

objective-issuer-swf = [color=turquoise]Федерация Космических Волшебников[/color]
wizard-title = Волшебник
wizard-description = На станции появился волшебник! Никогда не знаешь, что они могут сделать.
roles-antag-wizard-name = Волшебник
roles-antag-wizard-objective = Преподайте им урок, который они никогда не забудут.
wizard-role-greeting =
    ВЫ ВОЛШЕБНИК!
    Между Федерацией Космических Волшебников и Нанотрезен возникли трения.
    Итак, Федерация Космических Волшебников выбрала вас для визита на станцию.
    Продемонстрируйте им свои способности.ы
    Что делать - решать вам, но помните, что космические волшебники хотят, чтобы вы выбрались живыми.
wizard-round-end-name = волшебник

## TODO: Wizard Apprentice (Coming sometime post-wizard release)

