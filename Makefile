# -----------------------------
# CONFIG
# -----------------------------

COMPOSE=docker compose -f docker-compose.dev.yaml
PROFILE=--profile migrate

# -----------------------------
# MAIN COMMANDS
# -----------------------------

## 🚀 Запуск API (dev)
run:
	dotnet watch --project src/API/API.csproj run --urls=http://localhost:8080	

## Запустить конейнеры
up:	
	$(COMPOSE) up -d

## 🛑 Остановить всё
stop:
	$(COMPOSE) down

## 🔄 Пересобрать всё без кэша
rebuild:
	$(COMPOSE) build --no-cache

## 🧹 Полная очистка (контейнеры + volume)
clean:
	$(COMPOSE) down -v --remove-orphans

# -----------------------------
# MIGRATIONS
# -----------------------------

## 🗄 Запустить миграции (с пересборкой)
migrate:
	dotnet run --project src/Migrator/Migrator.csproj

## 🗄 Запустить миграции без пересборки
migrate-fast:
	$(COMPOSE) $(PROFILE) up --abort-on-container-exit migrator

## 🗄 Пересобрать только мигратор
build-migrator:
	$(COMPOSE) build migrator

# -----------------------------
# DEBUG
# -----------------------------

## 🔍 Логи мигратора
logs-migrator:
	$(COMPOSE) logs migrator

## 🔍 Зайти в контейнер API
bash-api:
	$(COMPOSE) exec api sh
