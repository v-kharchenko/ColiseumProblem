# Nsu.ColiseumProblem
Проект, выполненный в рамках задания курса по C#.

## Постановка задачи
Илон Маск и Марк Цукерберг собираютяс устроить дуэль в Римском Колизее. Однако, боги позволят им это сделать, только если они выиграют в следующую игру.
Боги перемешивают колоду из 36 карт и отдают каждому игроку по половине колоды. Каждый игрок, посмотрев на свою колоду, должен назвать номер карты из колоды соперника. После того как оба назовут номера, эти карты открываются. Если их цвета совпадают - игроки победили.
Илон и Марк могут обсуждать стратегию выбора только до начала игры.

В рамках задачи требуется придумать и реализовать эффективную стратегию, провести эксперимент многократно и посчитать количество успешных выполнений.

## Структура пакетов
### Contracts
Содержит контракты карты, стратегии, перемешивателя колод. Также там содержится CardMaster - класс, создающий колоду и разделяющий её пополам. Любые реализации процесса игры основываются на работе с этими классами

### Strategy
Содержит реализацию интерфейса ICardPickStrategy из контрактов. Эта стратегия статистически успешна в 65,8% случаев.

### FisherYatesDeckShuffler
Содержит реализацию перемешивателя колоды методом Фишера-Йетса.

### Sandbox
Содержит класс проведения эксперимента ColiseumSandbox и класс игроков Player.

### Host
Реализует проведение множества экспериментов через Hosted Service.

### Database
Реализует работу с базой данных SQLite через EntityFramework. Также содержит две реализации перемешивателя карт, которые при проведении эксперимента работают с базой данных:
- DeckSavingShuffler - делегирует перемешивание классу FisherYatesDeckShuffler, полученный порядок карт сохраняет под соответствующим номером эксперимента в базу и возвращает колоду.
- DeckLoadingShuffler - вместо перемешивания карт загружает порядок из базы данных и возвращает колоду.

### Tests
Реализует юнит-тестирование нескольких элементов программы:
- создателя колоды (результат создания содержит 18 черных и 18 красных карт);
- sandbox (однократное перемешивание колоды, при определенном порядке карт - ожидаемый результат);
- стратегии (возвращает тот номер, который обусловлен порядком карт в колоде игрока, соответственно реализуемой стратегии);
- базы данных (после сохранения и загрузки порядок карт эксперимента сохраняется);

### Web
Реализует проведение экспериментов через взаимодействие между богами и игроками по HTTP.

#### Web.Player
Реализует игрока в виде Web API, который отвечает на HTTP-запросы. В ответ на запрос POST с колодой игрока возвращает номер карты в виде HTTP ответа.

#### Web.Host
Реализует богов через Hosted Service, который отправляет карты web-игрокам и получает номера карт по HTTP. Сетевые взаимодействия с игроками идут через PlayerClient.

### MassTransit
Реализует приложение через микросервисы, взаимодействующии друг с другом через MassTransit над RabbitMQ:
1. Игроки слушают очереди, соответствующие их именам (например, rabbitmq://localhost:).
2. Боги отправляют колоды игрокам через MassTransit Send в соответствующие очереди.
3. Игрок, получив колоду, выбирает номер карты и публикует его ерез MassTransit Publish.
4. Игрок, получив номер карты, определяет цвет этой карты в своей колоде и готовится отвечать на HTTP запрос.
5. Боги запрашивают у игроков определившиеся цвета по HTTP.
Если боги запросили цвет карты раньше, чем игрок его определил, то игрок задерживает ответ до получения этой информации.

#### MassTransit.Entities
Содержит сообщения, которыми обмениваются игроки и боги через MassTransit:
- PickCard - команда для определения карты по колоде.
- CardPicked - событие выбора карты, для того чтобы соперник определил по номеру цвет.

#### MassTransit.God
Реализует богов через Hosted Service, который отправляет карты через MassTransit Send и получает номера карт по HTTP. Сетевые взаимодействия с игроками идут через PlayerClient.

#### MassTransit.Player
Реализует игроков как Web API, также содержащие MassTransit-получателей:
- PickCardConsumer - ждёт команды PickCard.
- CardPickConsumer - ждёт команды CardPicked.
Содержит класс DeckManager, синхронизирующий получение колоды и получение номера карты с определением цвета карты. Таким образом,
```c#
await _deckManager.GetColorAsync();
```
приводит к задержке функции, пока _deckManager не получит колоду и номер карты.
