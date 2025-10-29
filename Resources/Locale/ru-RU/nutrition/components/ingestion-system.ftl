### Interaction Messages


# System


## When trying to ingest without the required utensil... but you gotta hold it

ingestion-you-need-to-hold-utensil = Вам нужно держать в руках { $utensil }, чтобы съесть это!
ingestion-try-use-is-empty =
    { CAPITALIZE($entity) } { GENDER($entity) ->
        [male] пуст
        [female] пуста
        [epicene] пусты
       *[neuter] пусто
    }!
ingestion-try-use-wrong-utensil = Вы не можете { $verb } { $food } с помощью { $utensil }.
ingestion-remove-mask = Сначала вам нужно снять { $entity }.

## Failed Ingestion

ingestion-you-cannot-ingest-any-more = Вы не можете { $verb } больше!
ingestion-other-cannot-ingest-any-more =
    { CAPITALIZE(SUBJECT($target)) } не { GENDER($target) ->
        [epicene] могут
       *[other] может
    } { $verb } больше!
ingestion-cant-digest = Вы не можете переварить { $entity }!
ingestion-cant-digest-other = { CAPITALIZE(SUBJECT($target)) } не может переварить { $entity }!

## Action Verbs, not to be confused with Verbs

ingestion-verb-food = Есть
ingestion-verb-drink = Пить

# Edible Component

edible-nom = Ням. { $flavors }
edible-nom-other = Ням.
edible-slurp = Сёрб. { $flavors }
edible-slurp-other = Сёрб.
edible-swallow = Вы проглотили { $food }
edible-gulp = Глоть. { $flavors }
edible-gulp-other = Глоть.
edible-has-used-storage = Вы не можете { $verb } { $food } с предметом внутри.

## Nouns

edible-noun-edible = съедобное
edible-noun-food = еда
edible-noun-drink = напиток
edible-noun-pill = таблетка

## Verbs

edible-verb-edible = принимать
edible-verb-food = съесть
edible-verb-drink = выпить
edible-verb-pill = проглотить

## Force feeding

edible-force-feed = { CAPITALIZE($user) } пытается заставить вас { $verb } что-то!
edible-force-feed-success = { CAPITALIZE($user) } заставил вас { $verb } что-то! { $flavors }
edible-force-feed-success-user = Вы успешно накормили { $target }
