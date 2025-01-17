# MyLab Ava Storage

Веб сервис, позволяющий хранить и поставлять аватарки субъектов требуемого размера.

Ознакомьтесь с последними изменениями в [журнале изменений](/CHANGELOG.md).

Спецификация API : [![Admin API specification](https://img.shields.io/badge/OAS3-v2%20(actual)-green)](./doc/ava-api-v1.yml)

`Docker` образ: [![Docker image](https://img.shields.io/static/v1?label=docker&style=flat&logo=docker&message=image&color=blue)](https://github.com/mylab-search-fx/indexer/pkgs/container/indexer)

## О сервисе

`AvaStorage` имеет веб-api интерфейс, через который можно как сохранить, так и получить аватарку. 

Предполагается, что аватарка передаётся в сервис с произвольными размерами. `AvaStorage` сам обрежет изображение до квадратного и подгонит под запрашиваемый размер. При этом всегда можно получить оригинальное изображение.

## Хранилище

В качестве места хранения файлов изображений аватарок `AvaStorage` использует локальную файловую систему. По умолчанию корнем места хранения является **/var/lib/ava-storage**.

Структура папок:

* `personal` - для персональных аватарок;
* `subject-types` - для аватарок типов субъектов;
* `default` - аватарки по умолчанию.

### Директория `personal`

Содержит директории с персональными аватарками. Субъект и, соответственно, поддиректория определяется по идентификатору субъекта из запроса. 

Персональная поддиректория содержит файлы аватарок:

* `origin` - оригинальный файл.

### Директория `subject-types`

Содержит директории с аватарками типов субъектов. Тип субъекта и, соответственно, поддиректория определяются по типу субъекта из запроса.

Поддиректория типа субъекта содержит файлы аватарок:

* `default` - по умолчанию.

### Директория `default`

Содержит аватарки по умолчанию:

*  `default` - по умолчанию.

## Алгоритм

При обработке запроса получения аватарки выполняются следующие действия:

* попытка получить файл изображения аватарки;
* если не найден файл - ответ 404;
* если в запросе указан размер и если он не соответствует размеру изображения - изменение изображения под указанный размер.

### Поиск файла аватарки

* если в запросе указан размер, попытка получить персональную аватарку указанного размера;
* попытка получить оригинальную аватарку;
* если указан тип субъекта:
  * если указан размер, попытка получить аватарку типа субъекта указанного размера;
  * попытка получить аватарку по умолчанию типа субъекта;
* если указан размер, попытка получить аватарку по умолчанию указанного размера;
* попытка получить аватарку по умолчанию.

## Метрики

Предоставляет [базовые HTTP метрики](https://github.com/mylab-monitoring/http-metrics?tab=readme-ov-file#%D0%BC%D0%B5%D1%82%D1%80%D0%B8%D0%BA%D0%B8).

## Конфигурация

Конфигурация, относящаяся к настройкам, специфичным для логики работы сервиса находятся в узле `AvaStorage` и имеет следующее содержание:

* `MaxRequestedSize` - максимальный размер запрашиваемого изображения. По умолчанию - **512** **pix**;
* `MaxOriginalFileLength` - максимальный размер загружаемого файла. По умолчанию - **512 KiB**;
* `LocalStoragePath` - локальный путь к хранилищу изображений. По умолчанию - **/var/lib/ava-storage**.

## Развёртывание

