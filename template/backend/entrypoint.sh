#!/bin/sh
set -e

echo "==================================================="
echo "Waiting for PostgreSQL to be ready..."
echo "==================================================="

until nc -z postgres 5432; do
  echo "PostgreSQL is not ready yet - waiting..."
  sleep 2
done

echo "PostgreSQL is ready!"

echo "==================================================="
echo "Waiting for RabbitMQ to be ready..."
echo "==================================================="

until nc -z rabbitmq 5672; do
  echo "RabbitMQ is not ready yet - waiting..."
  sleep 2
done

echo "RabbitMQ is ready!"

echo "==================================================="
echo "Starting application..."
echo "NOTE: Run migrations manually with:"
echo "docker-compose exec api dotnet ef database update --project /src/src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj --startup-project /src/src/Ambev.DeveloperEvaluation.WebApi"
echo "==================================================="

exec dotnet Ambev.DeveloperEvaluation.WebApi.dll