# Refill Messages
comp-entity-provider-cannot-receive = { CAPITALIZE($refillTarget) } не может быть пополнен!
comp-entity-provider-cannot-transfer = { CAPITALIZE($provider) } не может быть использован для пополнения!
comp-entity-provider-full = { CAPITALIZE($provider) } уже полный!

# Ejection Messages
comp-entity-provider-no-ejected = Здесь нечего извлекать!

# Menu Messages
comp-entity-provider-select-entity = Выбрать { MAKEPLURAL($entity) }.
comp-entity-provider-eject-all-specified-entities = Извлечь всё { MAKEPLURAL($entity) }.
comp-entity-provider-select-new-active = Переключить выбранный хранимый объект.

# Examine Description

comp-entity-provider-no-stored-entities = Пуст.
comp-entity-provider-has-entities = Содержит следующее:
comp-entity-provider-entity-listing = {$amount ->
    [one] [color=yellow]{ $amount }[/color] [color=gray]{ $name }[/color]
    *[other] [color=yellow]{ $amount }[/color] [color=gray]{ MAKEPLURAL($name) }[/color]
}
