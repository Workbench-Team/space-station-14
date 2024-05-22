anomaly-component-contact-damage = Аномалия сдирает с Вас кожу!
anomaly-vessel-component-anomaly-assigned = Аномалия привязана к сосуду.
anomaly-vessel-component-not-assigned = Этот сосуд не привязан ни к одной аномалии. Попробуйте использовать сканер на нём.
anomaly-vessel-component-assigned = Этот сосуд уже привязан к аномалии.
anomaly-particles-delta = Дельта частицы
anomaly-particles-epsilon = Эпсилон частицы
anomaly-particles-zeta = Зетта частицы
anomaly-particles-omega = Омега частицы
anomaly-particles-sigma = Сигма частицы
anomaly-scanner-component-scan-complete = Сканирование завершено!
anomaly-scanner-ui-title = Сканер аномалий
anomaly-scanner-no-anomaly = Аномалии не просканированы
anomaly-scanner-severity-percentage = Опасность на: [color=gray]{ $percent }[/color]
anomaly-scanner-severity-percentage-unknown = Опасность на: [color=red]ОШИБКА[/color]
anomaly-scanner-stability-low = Текущая стабильность аномалии: [color=gold]Распадается[/color]
anomaly-scanner-stability-medium = Текущая стабильность аномалии: [color=forestgreen]Стабильно[/color]
anomaly-scanner-stability-high = Текущая стабильность аномалии: [color=crimson]Растёт[/color]
anomaly-scanner-stability-unknown = Текущая стабильность аномалии: [color=red]ОШИБКА[/color]
anomaly-scanner-point-output = Генерация очков: [color=gray]{ $point }[/color]
anomaly-scanner-point-output-unknown = Генерация очков: [color=red]ОШИБКА[/color]
anomaly-scanner-particle-readout = Анализ реакции частиц:
anomaly-scanner-particle-danger = - [color=crimson]Опасный тип:[/color] { $type }
anomaly-scanner-particle-unstable = - [color=plum]Нестабильный тип:[/color] { $type }
anomaly-scanner-particle-containment = - [color=goldenrod]Стабильный тип:[/color] { $type }
anomaly-scanner-particle-transformation = - [color=#6b75fa]Метаморфозный тип:[/color] { $type }
anomaly-scanner-particle-danger-unknown = - [color=crimson]Опасный тип:[/color] [color=red]ОШИБКА[/color]
anomaly-scanner-particle-unstable-unknown = - [color=plum]Нестабильный тип[/color] [color=red]ОШИБКА[/color]
anomaly-scanner-particle-containment-unknown = - [color=goldenrod]Стабильный тип[/color] [color=red]ОШИБКА[/color]
anomaly-scanner-particle-transformation-unknown = - [color=#6b75fa]Метаморфозный тип:[/color] [color=red]ОШИБКА[/color]
anomaly-scanner-pulse-timer = Время до следующего импульса: [color=gray]{ $time }[/color]
anomaly-gorilla-core-slot-name = Ядро аномалии
anomaly-gorilla-charge-none = Внутри нет [bold]ядра аномалии[/bold].
anomaly-gorilla-charge-limit =
    { $count ->
        [one] Остался
       *[other] Осталось
    } [color={ $count ->
        [3] green
        [2] yellow
        [1] orange
        [0] red
       *[other] purple
    }]{ $count } { $count ->
        [one] заряд
        [few] заряда
       *[other] зарядов
    }[/color].
anomaly-gorilla-charge-infinite = Осталось [color=gold]бесконечно зарядов[/color]. [italic]Пока что...[/italic]
anomaly-sync-connected = Аномалия успешно подключена
anomaly-sync-disconnected = Связь с аномалией потеряна!
anomaly-sync-no-anomaly = Нет аномалий в радиусе действия.
anomaly-sync-examine-connected = Он [color=darkgreen]прикреплён[/color] к аномалии.
anomaly-sync-examine-not-connected = Он [color=darkred]не прикреплён[/color] к аномалии.
anomaly-sync-connect-verb-text = Прикрепить аномалию
anomaly-sync-connect-verb-message = Прикрепить ближайшую аномалию к { $machine }.
anomaly-generator-ui-title = Генератор аномалий
anomaly-generator-fuel-display = Топливо:
anomaly-generator-cooldown = Перезарядка: [color=gray]{ $time }[/color]
anomaly-generator-no-cooldown = Перезарядка: [color=gray]Завершено[/color]
anomaly-generator-yes-fire = Статус: [color=forestgreen]Готово[/color]
anomaly-generator-no-fire = Статус: [color=crimson]Не готово[/color]
anomaly-generator-generate = Генерировать аномалию
anomaly-generator-charges =
    { $charges ->
        [one] { $charges } заряд
        [few] { $charges } заряда
       *[other] { $charges } зарядов
    }
anomaly-generator-announcement = Аномалия была сгенерирована!
anomaly-command-pulse = Вызывает импульс целевой аномалии
anomaly-command-supercritical = Делает целевую аномалию сверхкритической
# Flavor text on the footer
anomaly-generator-flavor-left = Аномалия может возникнуть внутри оператора.
anomaly-generator-flavor-right = v1.1
anomaly-behavior-unknown = [color=red]ОШИБКА. Невозможно прочитать.[/color]
anomaly-behavior-title = анализ отклонений в поведении:
anomaly-behavior-point = [color=gold]Аномалия генерирует { $mod }% очков[/color]
anomaly-behavior-safe = [color=forestgreen]Аномалия чрезвычайно стабильна. Крайне редкие импульсы.[/color]
anomaly-behavior-slow = [color=forestgreen]Частота импульсов значительно меньше.[/color]
anomaly-behavior-light = [color=forestgreen]Мощность импульсов значительно снижается.[/color]
anomaly-behavior-balanced = Отклонений в поведении не обнаружено.
anomaly-behavior-delayed-force = Частота импульсов значительно снижается, но их сила увеличивается.
anomaly-behavior-rapid = Частота импульсов гораздо выше, но их сила ослаблена.
anomaly-behavior-reflect = Обнаружено защитное поле.
anomaly-behavior-nonsensivity = Была обнаружена низкая чувствительность к частицам.
anomaly-behavior-sensivity = Была обнаружена высокая чувствительность к частицам.
anomaly-behavior-secret = Обнаружены помехи. Некоторые данные не могут быть прочитаны.
anomaly-behavior-inconstancy = [color=crimson]Обнаружена неустойчивость. Типы частиц могут меняться с течением времени.[/color]
anomaly-behavior-fast = [color=crimson]Частота импульсов сильно увеличивается.[/color]
anomaly-behavior-strenght = [color=crimson]Мощность импульсов значительно увеличивается.[/color]
anomaly-behavior-moving = [color=crimson]Обнаружена нестабильность координат.[/color]
