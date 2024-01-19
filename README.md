### Основной сервис

* .NET 8
* [MartenDB](https://martendb.io/documents/) -- документоориентированная СУБД поверх PostgreSQL.
* [Telegram.Bot](https://telegrambots.github.io/book/) -- .NET клиент Telegram Bot API

## Разработка

Запуск dev окружения

```shell
docker-compose -p pocker-dev -f .\manifests\docker-compose.yml --env-file .\manifests\.env.dev up -d
```
