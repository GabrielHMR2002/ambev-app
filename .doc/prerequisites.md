## Prerequisites

Before running the project, make sure the following requirements are met:

---

### 1. **Visual Studio 2022 or later**

* Required to run and debug the application locally.
* Recommended workloads:

  * ASP.NET and web development
  * .NET desktop development
* Ensure Visual Studio is updated and configured to use the **.NET 8 SDK**.
* **Download:** [Visual Studio 2022](https://visualstudio.microsoft.com/en-us/downloads/)

---

### 2. **PostgreSQL**

* Replacing SQL Server, the project now uses **PostgreSQL**.
* Can be installed locally or accessed via network.
* Recommended configuration:

  * Standard username/password authentication
  * Create a dedicated database for the project
* **Download:** [PostgreSQL](https://www.postgresql.org/download/)

---

### 3. **pgAdmin (optional but recommended)**

* Management tool for PostgreSQL.
* Allows easy creation, inspection, and management of databases.
* **Download:** [pgAdmin](https://www.pgadmin.org/download/)

---

### 4. **Docker Desktop**

* Required to run the RabbitMQ container.
* Make sure Docker Desktop is installed, running, and properly configured before executing any Docker commands.
* RabbitMQ will use the following ports:

  * **5672** - AMQP
  * **15672** - Management UI
* **Download:** [Docker Desktop](https://www.docker.com/products/docker-desktop/)

---

### 5. **.NET 8 SDK**

* Required to build and run the application.
* Verify installation by running:

  ```bash
  dotnet --version
  ```
* **Download:** [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

---

### 6. **Git**

* Required to clone the project repository.
* Verify installation by running:

  ```bash
  git --version
  ```
* **Download:** [Git](https://git-scm.com/downloads)

---

### 7. **Browser**

* A modern browser is recommended to access:

  * Swagger UI: `https://localhost:7001/index.html`
  * RabbitMQ Management UI: `http://localhost:15672`
  * pgAdmin: PostgreSQL GUI management tool

---

### 8. **Network and Ports**

* Ensure the following ports are free and not blocked by firewall:

  * `7001` - API
  * `5672` - RabbitMQ (AMQP)
  * `15672` - RabbitMQ UI
  * `5432` - PostgreSQL (default port)

---

### 9. **Operating System**

* Tested on **Windows 10/11**.
* Should work on Linux/Mac with Docker and .NET 8 SDK installed, with minor adjustments.


