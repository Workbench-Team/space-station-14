
<h1 align="center"> [GitLab](https://gitlab.workbench.network/Workbench-Team/space-station-14) | [GitHub](https://github.com/Workbench-Team/space-station-14) </h1>
=======
<div class="header" align="center">  
<img alt="Space Station 14" width="880" height="300" src="https://raw.githubusercontent.com/space-wizards/asset-dump/de329a7898bb716b9d5ba9a0cd07f38e61f1ed05/github-logo.svg">  
</div>

Space Station 14 это ремейк SS13, который работает на собственном движке [Robust Toolbox](https://github.com/space-wizards/RobustToolbox), написанном на C#.

Это репозиторий форка Space Station 14 от Workbench Team который используется на нашем основном сервере Starshine, и его также можете использовать и Вы для получения эксклюзивных фишек форка. Если Вам нужен только русский перевод для игры без наших фишек, перейдите в [master-ru ветку](https://gitlab.workbench.network/Workbench-Team/space-station-14/-/tree/master-ru).

Вы можете использовать эту ветку как для создания собственного сервера, так и для контрибьюта в работу нашего сервера.

## Ссылки

<div class="header" align="center">  

[Сайт игры](https://spacestation14.com/) | [Workbench Discord](https://discord.gg/Dxqz5gy) | [Workbench Revolt](https://rvlt.gg/wcYASVKF) | [Steam игры](https://store.steampowered.com/app/1255460/Space_Station_14/) | [Скачать лаунчер](https://spacestation14.com/about/nightlies/)  

</div>

## Документация

На официальном сайте с [документацией](https://docs.spacestation14.com/) имеется вся необходимая информация о контенте SS14, движке, дизайне игры и многом другом.
Помимо этого, ознакомьтесь с этими ресурсами для получения информации о лицензии и авторстве:
- [Общая атрибуция Robust](https://docs.spacestation14.com/en/specifications/robust-generic-attribution.html)  
- [Robust Station Image](https://docs.spacestation14.com/en/specifications/robust-station-image.html)

Также имеется множество ресурсов для новых контрибьюторов проекта.

## Вклад

Если Вы хотите предложить добавление нового контента или редактирование существующего, мы с радостью ждём Ваши изменения на нашем [основном репозитории GitLab](https://git.arumoon.ru/Workbench-Team/space-station-14/-/tree/arumoon-server) (рекомендуется) или [зеркале GitHub](https://github.com/Workbench-Team/space-station-14/tree/arumoon-server). Если Вам нужна помощь, посмотрите текущие [обсуждения в GitLab](https://git.arumoon.ru/Workbench-Team/space-station-14/-/issues) или лучше перейдите на Discord или Revolt сервер Workbench Team для более удобной коммуникации.

## Готовая сборка

Статус сборки: [![pipeline status](https://gitlab.workbench.network/Workbench-Team/space-station-14/badges/arumoon-server/pipeline.svg)](https://gitlab.workbench.network/Workbench-Team/space-station-14/-/commits/arumoon-server)

Готовые билды сборки Вы можете скачать из [CDN хостинга (быстро)](https://ss14.lolicon.monster/builds/arumoon-server-builds.html) или [артефактов CI/CD (медленно)](https://gitlab.workbench.network/Workbench-Team/space-station-14/-/pipelines?page=1&scope=all&ref=arumoon-server&status=success)

## Самостоятельная сборка

1. Склонируйте этот репозиторий:
```shell
git clone https://github.com/Workbench-Team/space-station-14.git
```
2. Переключитесь на ветку `arumoon-server`

3. Зайдите в папку проекта и запустите скрипт `RUN_THIS.py` для инициализации субмодулей и скачивания движка:
```shell
cd space-station-14
python RUN_THIS.py
```
4. Соберите решение:
Создайте сборку через Visual Studio или `dotnet build` в терминале.

[Более детальная инструкция для сборки.](https://docs.spacestation14.com/en/general-development/setup.html)

## Лицензия

Весь код репозитория лицензирован под [MIT](https://gitlab.workbench.network/Workbench-Team/space-station-14/blob/master-ru/LICENSE.TXT).

Большинство ассетов лицензированы под [CC-BY-SA 3.0](https://creativecommons.org/licenses/by-sa/3.0/) если не имеют иное. Ассеты имеют свою лицензию и авторские права в файле метаданных. [Пример](https://gitlab.workbench.network/Workbench-Team/space-station-14/blob/master-ru/Resources/Textures/Objects/Tools/crowbar.rsi/meta.json).

> [!NOTE]
> Обратите внимание, что некоторые ассеты лицензированы на некоммерческой основе [CC-BY-NC-SA 3.0](https://creativecommons.org/licenses/by-nc-sa/3.0/) или аналогичной некоммерческой лицензией, и их необходимо удалить, если Вы хотите использовать этот проект в коммерческих целях.
