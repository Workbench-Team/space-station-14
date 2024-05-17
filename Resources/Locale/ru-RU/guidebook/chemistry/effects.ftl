-create-3rd-person =
    { $chance ->
        [1] Создаёт
       *[other] создать
    }
-cause-3rd-person =
    { $chance ->
        [1] Вызывает
       *[other] вызвать
    }
reagent-effect-guidebook-emp-reaction-effect =
    { $chance ->
        [1] Вызывает
       *[other] вызвать
    } электромагнитный импульс
-satiate-3rd-person =
    { $chance ->
        [1] Утоляет
       *[other] утолить
    }
reagent-effect-guidebook-create-entity-reaction-effect =
    { $chance ->
        [1] Создаёт
       *[other] создать
    } { $amount ->
        [1] { INDEFINITE($entname) }
       *[other] { $amount } { $entname }
    }
reagent-effect-guidebook-explosion-reaction-effect =
    { $chance ->
        [1] Вызывает
       *[other] вызвать
    } взрыв
reagent-effect-guidebook-foam-area-reaction-effect =
    { $chance ->
        [1] Создаёт
       *[other] создать
    } большое количество пены
reagent-effect-guidebook-smoke-area-reaction-effect =
    { $chance ->
        [1] Создаёт
       *[other] создают
    } большое количество дыма
reagent-effect-guidebook-foam-area-reaction-effect =
    { $chance ->
        [1] Создаёт
       *[other] создать
    } большое количество дыма
reagent-effect-guidebook-satiate-thirst =
    { $chance ->
        [1] Утоляет
       *[other] утолить
    } { $relative ->
        [1] жажду
       *[other] жажду на { NATURALFIXED($relative, 3) } за единицу
    }
reagent-effect-guidebook-satiate-hunger =
    { $chance ->
        [1] Утоляет
       *[other] утолить
    } { $relative ->
        [1] голод
       *[other] голод на { NATURALFIXED($relative, 3) } за единицу
    }
reagent-effect-guidebook-health-change =
    { $chance ->
        [1]
            { $healsordeals ->
                [heals] Лечит
                [deals] Наносит
               *[both] Изменяет здоровье на
            }
       *[other]
            { $healsordeals ->
                [heals] вылечить
                [deals] нанести
               *[both] изменить здоровье на
            }
    } { $changes }
reagent-effect-guidebook-status-effect =
    { $type ->
        [add]
            { $chance ->
                [1] Вызывает
               *[other] вызвать
            } { LOC($key) } в течение { NATURALFIXED($time, 3) } { MANY("секунд", $time) }, суммируется
       *[set]
            { $chance ->
                [1] Вызывает
               *[other] вызвать
            } { LOC($key) } в течение { NATURALFIXED($time, 3) } { MANY("секунд", $time) }, не суммируется
        [remove]
            { $chance ->
                [1] Удаляет
               *[other] удалить
            } { NATURALFIXED($time, 3) } { MANY("секунд", $time) } { LOC($key) }
    }
reagent-effect-guidebook-activate-artifact =
    { $chance ->
        [1] Пытается
       *[other] попытаться
    } активировать артефакт
reagent-effect-guidebook-set-solution-temperature-effect =
    { $chance ->
        [1] Задаёт
       *[other] задать
    } температуру раствора до { NATURALFIXED($temperature, 2) }К
reagent-effect-guidebook-adjust-solution-temperature-effect =
    { $chance ->
        [1]
            { $deltasign ->
                [1] Добавляет
               *[-1] Удаляет
            }
       *[other]
            { $deltasign ->
                [1] добавить
               *[-1] удалить
            }
    } тепло из раствора, пока его температура не достигнет { $deltasign ->
        [1] не более { NATURALFIXED($maxtemp, 2) }К
       *[-1] не менее { NATURALFIXED($mintemp, 2) }k
    }
reagent-effect-guidebook-adjust-reagent-reagent =
    { $chance ->
        [1]
            { $deltasign ->
                [1] Добавляет
               *[-1] Удаляет
            }
       *[other]
            { $deltasign ->
                [1] добавить
               *[-1] удалить
            }
    } { NATURALFIXED($amount, 2) } ед. { $reagent } { $deltasign ->
        [1] к раствору
       *[-1] из раствора
    }
reagent-effect-guidebook-adjust-reagent-group =
    { $chance ->
        [1]
            { $deltasign ->
                [1] Добавляет
               *[-1] Удаляет
            }
       *[other]
            { $deltasign ->
                [1] добавить
               *[-1] удалить
            }
    } { NATURALFIXED($amount, 2) } ед. реагентов группы { $group } { $deltasign ->
        [1] к раствору
       *[-1] из раствора
    }
reagent-effect-guidebook-adjust-temperature =
    { $chance ->
        [1]
            { $deltasign ->
                [1] Согревает
               *[-1] Охлаждает
            }
       *[other]
            { $deltasign ->
                [1] согреть
               *[-1] охладить
            }
    } на { POWERJOULES($amount) } тело в котором находится
reagent-effect-guidebook-chem-cause-disease =
    { $chance ->
        [1] Вызывает
       *[other] вызвать
    } болезнь { $disease }
reagent-effect-guidebook-chem-cause-random-disease =
    { $chance ->
        [1] Вызывает
       *[other] вызвать
    } болезнь { $diseases }
reagent-effect-guidebook-jittering =
    { $chance ->
        [1] Вызывает
       *[other] вызвать
    } дрожь
reagent-effect-guidebook-chem-clean-bloodstream =
    { $chance ->
        [1] Очищает
       *[other] очистить
    } кровоток от других химических веществ
reagent-effect-guidebook-cure-disease =
    { $chance ->
        [1] Вылечивает
       *[other] вылечить
    } болезни
reagent-effect-guidebook-cure-eye-damage =
    { $chance ->
        [1]
            { $deltasign ->
                [1] Наносит
               *[-1] Лечит
            }
       *[other]
            { $deltasign ->
                [1] нанести
               *[-1] вылечить
            }
    } повреждения глаз
reagent-effect-guidebook-chem-vomit =
    { $chance ->
        [1] Вызывает
       *[other] вызвать
    } рвоту
reagent-effect-guidebook-create-gas =
    { $chance ->
        [1] Создаёт
       *[other] создать
    } { $moles } { $moles ->
        [1] моль
       *[other] молей
    } { $gas }
reagent-effect-guidebook-drunk =
    { $chance ->
        [1] Вызывает
       *[other] вызвать
    } опьянение
reagent-effect-guidebook-electrocute =
    { $chance ->
        [1] Бьёт током
       *[other] ударить током
    } существо на { NATURALFIXED($time, 3) } { MANY("секунд", $time) }
reagent-effect-guidebook-extinguish-reaction =
    { $chance ->
        [1] Тушит
       *[other] потушить
    } огонь
reagent-effect-guidebook-make-polymorph =
    { $chance ->
        [1] Превращает
       *[other] превратить
    } существо в { $entityname }
reagent-effect-guidebook-flammable-reaction =
    { $chance ->
        [1] Повышает
       *[other] повысить
    } воспламеняемость
reagent-effect-guidebook-ignite =
    { $chance ->
        [1] Воспламеняет
       *[other] воспламенить
    } существо
reagent-effect-guidebook-make-sentient =
    { $chance ->
        [1] Делает
       *[other] сделать
    } существо разумным
reagent-effect-guidebook-modify-bleed-amount =
    { $chance ->
        [1]
            { $deltasign ->
                [1] Вызывает
               *[-1] Уменьшает
            }
       *[other]
            { $deltasign ->
                [1] вызвать
               *[-1] уменьшить
            }
    } кровотечение
reagent-effect-guidebook-modify-blood-level =
    { $chance ->
        [1]
            { $deltasign ->
                [1] Увеличивает
               *[-1] Уменьшает
            }
       *[other]
            { $deltasign ->
                [1] увеличить
               *[-1] уменьшить
            }
    } уровень крови
reagent-effect-guidebook-paralyze =
    { $chance ->
        [1] Парализует
       *[other] парализовать
    } существо на { NATURALFIXED($time, 3) } { MANY("секунд", $time) }
reagent-effect-guidebook-cure-zombie-infection =
    { $chance ->
        [1] Излечивает
       *[other] излечить
    } действующую зомби-инфекцию
reagent-effect-guidebook-cause-zombie-infection =
    { $chance ->
        [1] Заражает
       *[other] заразить
    } зомби-инфекцией
reagent-effect-guidebook-innoculate-zombie-infection =
    { $chance ->
        [1] Излечивает
       *[other] излечить
    } действующую зомби-инфекцию и обеспечивает иммунитет к будущим инфекциям
reagent-effect-guidebook-movespeed-modifier =
    { $chance ->
        [1] Изменяет
       *[other] изменить
    } скорость передвижения на { NATURALFIXED($walkspeed, 3) }x на { NATURALFIXED($time, 3) } { MANY("секунд", $time) }
reagent-effect-guidebook-reset-narcolepsy =
    { $chance ->
        [1] Временно избавляет
       *[other] временно избавить
    } от приступов нарколепсии
reagent-effect-guidebook-reduce-rotting =
    { $chance ->
        [1] Регенерирует
       *[other] регенерируют
    } { NATURALFIXED($time, 3) } { MANY("second", $time) } гниения
reagent-effect-guidebook-wash-cream-pie-reaction =
    { $chance ->
        [1] Смывает
       *[other] смыть
    } кремовый пирог с лица
reagent-effect-guidebook-missing =
    { $chance ->
        [1] Вызывает
       *[other] вызвать
    } неизвестный эффект, так как никто еще не описал этот эффект
