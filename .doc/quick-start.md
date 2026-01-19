# 🚀 Quick Start with Docker

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running
- [Git](https://git-scm.com/) installed

## ⚡ 3 Steps to Run

### 1️⃣ Clone the repository

```bash
git clone https://github.com/GabrielHMR2002/ambev-app.git
cd ambev-app/template/backend
```

### 2️⃣ Start the entire application

```bash
docker-compose up -d --build
```

⏳ **Wait ~2 minutes** while:
- PostgreSQL starts
- RabbitMQ starts
- API compiles
- Application starts

### 2.5️⃣ Apply database migrations

Entity Framework Core Migrations

Navigate to the WebApi Project

Open your terminal or PowerShell and go to the **WebApi** folder:

```bash
cd src/Ambev.DeveloperEvaluation.WebApi
```

> **Note:** All commands below should be run from this directory.

---

### Restore NuGet Packages

Before updating the database, make sure all packages for both **WebApi** and **ORM** are installed:

```bash
dotnet restore
```
---

### Apply All Migrations

To update the database with all pending migrations, run:

```bash
dotnet ef database update --project ../Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj --startup-project ./
```

### 3️⃣ Access the application

Open your browser at:

**🔹 Swagger (API):** http://localhost:8080/swagger

**🔹 RabbitMQ (Management UI):** http://localhost:15672
- Username: `developer`
- Password: `ev@luAt10n`

---

## 📊 Check if it's running

```bash
docker-compose ps
```

You should see 3 containers running:
- ✅ `ambev_api`
- ✅ `ambev_postgres`
- ✅ `ambev_rabbitmq`

---

## 🛠️ Useful Commands

### View API logs
```bash
docker-compose logs -f api
```

### View all services logs
```bash
docker-compose logs -f
```

### Stop everything
```bash
docker-compose down
```

### Stop and clean data (start from scratch)
```bash
docker-compose down -v
docker-compose up -d --build
```

### Full rebuild (after code changes)
```bash
docker-compose up -d --build
```

---

## 🐛 Troubleshooting

### Swagger won't open?

Wait 30 seconds and try again. If still not working:

```bash
# Check logs
docker-compose logs api

# Restart
docker-compose restart api
```

### Port already in use?

Change ports in `docker-compose.yml`:

```yaml
ports:
  - "8081:8080"  # Change 8080 to 8081
```

### Container won't start?

```bash
# Clean everything and start from scratch
docker-compose down -v
docker system prune -f
docker-compose up -d --build
```

---

## 💾 Database

**Connect to PostgreSQL:**

```
Host: localhost
Port: 5432
Database: developer_evaluation
Username: developer
Password: ev@luAt10n
```

Or via terminal:

```bash
docker-compose exec postgres psql -U developer -d developer_evaluation
```

---

## 🔄 Development

To develop locally without Docker for the API:

See [How to run API](./how-to-run-api.md)

Access: https://localhost:7181/swagger

---

## ✅ Checklist

- [ ] Docker Desktop is running
- [ ] Executed `docker-compose up -d --build`
- [ ] Waited ~2 minutes
- [ ] Accessed http://localhost:8080/swagger
- [ ] Saw the API documentation
