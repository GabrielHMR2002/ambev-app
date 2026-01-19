# Project Documentation

## 1. Clone the Repository

```bash
git clone https://github.com/GabrielHMR2002/ambev-app.git
cd ambev-app
```

Open the project in your IDE:

```bash
code .
```

or open it with **Visual Studio** **(recommended)**.

---

## 2. PostgreSQL Database Setup

Before running the application and migrations, create the PostgreSQL database:

1. Open **pgAdmin** or connect to PostgreSQL.
2. Create the database:

<img width="959" height="200" alt="image" src="https://github.com/user-attachments/assets/bc9ca456-4da0-4be4-b660-354ff01e6b8c" />

<img width="959" height="473" alt="image" src="https://github.com/user-attachments/assets/9143fc5d-fdb2-48c4-84d3-e82bb8c84bf1" />

> Make sure the database name matches the one defined in the connection string.

---

## 3. Configure Connection String

Open the `appsettings.json` file located at:

```
template/backend/src/Ambev.DeveloperEvaluation.WebApi/appsettings.json
```

Update the PostgreSQL connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DeveloperEvaluationAmbev;User Id=USER_HERE;Password=PASSWORD_HERE;TrustServerCertificate=True"
  }
}
```

> Ensure PostgreSQL is running and credentials match your database configuration.

---

Absolutely! Here’s a polished **English version** of your documentation section, including the restore step and clarifications:

---

## 4. Entity Framework Core Migrations

### 4.1. Navigate to the WebApi Project

Open your terminal or PowerShell and go to the **WebApi** folder:

```bash
cd template/backend/src/Ambev.DeveloperEvaluation.WebApi
```

> **Note:** All commands below should be run from this directory.

---

### 4.2. Restore NuGet Packages

Before updating the database, make sure all packages for both **WebApi** and **ORM** are installed:

```bash
dotnet restore
```

---

### 4.3. Apply All Migrations

To update the database with all pending migrations, run:

```bash
dotnet ef database update --project ../Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj --startup-project ./
```

> **Parameter explanation:**
>
> * `--project` → path to the project containing the **DbContext** (ORM).
> * `--startup-project` → path to the project used to run the application (WebApi).

<img width="959" height="461" alt="image" src="https://github.com/user-attachments/assets/5dcbe66e-c571-47ed-86c9-e275ca10e2c5" />
---

## 5. RabbitMQ Setup

### a) Run RabbitMQ with Docker

```bash
docker run -d --name ambev-rabbitmq --restart unless-stopped -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=guest -e RABBITMQ_DEFAULT_PASS=guest rabbitmq:3.12-management-alpine
```

### b) RabbitMQ Credentials

* **Username:** `guest`
* **Password:** `guest`

Management UI: [http://localhost:15672](http://localhost:15672)

---

## 6. Run the Web API

Make sure RabbitMQ is running before starting the application.

### a) Restore Dependencies

```bash
dotnet restore
```

### b) Build the Solution

```bash
dotnet build
```

> Run these commands from the WebApi directory:
> `template/backend/src/Ambev.DeveloperEvaluation.WebApi`

### c) Open Solution in Visual Studio

1. Open `Ambev.DeveloperEvaluation.sln`.
2. Set **Ambev.DeveloperEvaluation.WebApi** as the startup project.
3. Press **F5** or click **Run**.

### d) Access Swagger

Swagger UI will be available at:

```
https://localhost:7181/swagger/index.html
```

