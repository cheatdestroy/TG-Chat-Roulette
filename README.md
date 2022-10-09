# Telegram Bot Chat Roulette
> Чат бот рулетка - представляет из себя Telegram бота, который является посредником в общении между пользователями, сохраняя при этом анонимность каждого.
Бот соединяет двух пользователей из общего пула с предпочтительными критериями для каждого из них.

## Возможности (в разработке)
- [x] Заполнение данных о себе:
	- пол
	- возраст
- [x] Выбор предпочтительных критериев собеседника:
	- пол собеседника
	- возраст
- [x] Выбор предпочтительного общения:
	- тестовый чат
	- голосовые сообщения
- [ ] Поиск собеседника по предпочтительным критериям
- [ ] Статистика общения
- [ ] Добавление собеседника в черный список

## Требования
1. [.NET Core 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
2. [SQL Server 2019](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
3. Домен с SSL сертификатом или [Ngrok](https://ngrok.com) <sup>([зачем нужен SSL сертификат?](https://core.telegram.org/bots/faq#i-39m-having-problems-with-webhooks))</sup>

## Установка
1. Указать в appsettings.json
	 - ```BotName``` : Название бота
	 - ```Url``` : Адрес расположения метода контроллера для вебкуха (https://ваш_домен/api/webhook/update)
	 - ```Token``` : Токен бота <sup>([подробнее](https://core.telegram.org/bots#3-how-do-i-create-a-bot))</sup>
	 - ```ConnectingString``` : Строка подключения к SQL Server
	 - Дополнительные настройки
	 	- ```MaximumAge``` : Максимально допустимый возраст
	 	- ```MinimumAge``` : Минимально допустимый возраст
	 	- ```DefaultLimit``` : Лимит получаемых данных за один запрос
	 	- ```DefaultOffset``` : Кол-во пропускаемых данных в запросе
2. [Настройка и публикация на веб-сервер](https://docs.microsoft.com/en-us/visualstudio/deployment/quickstart-deploy-aspnet-web-app?view=vs-2022&tabs=web-server#get-started)
