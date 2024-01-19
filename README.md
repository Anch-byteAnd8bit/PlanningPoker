### Основной сервис

* .NET 8
* [MartenDB](https://martendb.io/documents/) -- документоориентированная СУБД поверх PostgreSQL.
* [Telegram.Bot](https://telegrambots.github.io/book/) -- .NET клиент Telegram Bot API

## Разработка

Запуск dev окружения

docker-compose -p pocker-dev -f .\docker-compose.yml --env-file .\.env.dev up -d
