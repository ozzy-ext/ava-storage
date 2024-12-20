# MyLab Ava Storage

Веб сервис, позволяющий хранить и поставлять аватарки субъектов требуемого размера.

Ознакомьтесь с последними изменениями в [журнале изменений](/CHANGELOG.md).

Спецификация API : [![Admin API specification](https://img.shields.io/badge/OAS3-v2%20(actual)-green)](./doc/ava-api-v1.yml)

`Docker` образ: [![Docker image](https://img.shields.io/static/v1?label=docker&style=flat&logo=docker&message=image&color=blue)](https://github.com/mylab-search-fx/indexer/pkgs/container/indexer)

## О сервисе

`AvaStorage` имеет веб-api интерфейс, через который можно как сохранить, так и получить аватарку. 

Предполагается, что аватарка передаётся в сервис с произвольными размерами. `AvaStorage` сам обрежет изображение до квадратного и подгонит под запрашиваемый размер. При этом всегда можно получить оригинальное изображение.

## Метрики

Предоставляет [базовые HTTP метрики](https://github.com/mylab-monitoring/http-metrics?tab=readme-ov-file#%D0%BC%D0%B5%D1%82%D1%80%D0%B8%D0%BA%D0%B8).

## Конфигурация

Конфигурация, относящаяся к настройкам, специфичным для логики работы сервиса находятся в узле `AvaStorage` и имеет следующее содержание:

* `MaxSize` - максимальный размер сохраняемого и запрашиваемого изображения. По умолчанию - **512**;
* `LocalStoragePath` - локальный путь к хранилищу изображений. По умолчанию - **/var/lib/ava-storage**.

## Развёртывание

