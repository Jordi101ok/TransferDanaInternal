# TransferDanaInternal

> consists of three independent projects: an ASP.NET Core MVC app with Unit Test, a REST API, and a SQL Server database with stored procedures

---

## 📋 Table of Contents

- [Projects Overview](#projects-overview)
- [Setup Project](#setup-project)
- [Tools & Technologies](#tools--technologies)
- [Framework Versions](#framework-versions)
- [Database Schema](#database-schema)
- [Stored Procedures](#stored-procedures)
- [Sample Endpoints & Payloads](#sample-endpoints--payloads)
- [Unit Tests](#unit-tests)
- [CI/CD](#cicd)

---

## 🗂️ Projects Overview

| Project | Type | Description |
|---|---|---|
| `TransferDanaInternal` | ASP.NET Core MVC | Fund transfer UI with login, register, and transfer form |
| `RESTAPITransaksi&Rekening` | ASP.NET Core Web API | REST API for managing accounts and transactions |
| `SQL Database` | SQL Server | Schema, tables, stored procedures, and seed data |
| `TestProject` | xUnit | Unit tests for the MVC TransferController |

> These projects are **independent** — they share similar domain concepts (accounts, transactions) but do not call or depend on each other.

---

## 🚀 Setup Project

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code
- [SQL Server](https://www.microsoft.com/en-us/sql-server) or SQL Server Express

### MVC App

```bash
cd TransferDanaInternal
dotnet restore
dotnet run
```
Access at: `https://localhost:{PORT}`

### REST API

```bash
cd RESTAPITransaksi&Rekening
dotnet restore
dotnet run
```
Access Swagger at: `https://localhost:{PORT}/swagger`

### SQL Database

- Open SQL Server Management Studio (SSMS)
- Connect to your SQL Server instance
- Run the SQL script to create the schema, tables, stored procedures, and seed data

---

## 🛠️ Tools & Technologies

| Category         | Tool / Library                        |
|------------------|---------------------------------------|
| Language         | C# 12                                 |
| MVC Framework    | ASP.NET Core MVC 8                    |
| API Framework    | ASP.NET Core Web API 8                |
| Data Storage     | In-memory local variables (MVC & API) |
| Database         | SQL Server (standalone DB project)    |
| API Docs         | Swagger / Swashbuckle                 |
| Unit Testing     | xUnit                                 |
| IDE              | Visual Studio 2022                    |
| CI/CD            | GitHub Actions                        |
| Version Control  | Git + GitHub                          |

---

## 📦 Framework Versions

| Framework / Package              | Version   |
|----------------------------------|-----------|
| .NET                             | 8.0       |
| ASP.NET Core                     | 8.0       |
| Swashbuckle.AspNetCore           | 6.x       |
| xUnit                            | 2.x       |
| SQL Server                       | 2019+     |

---

## 🗄️ Database Schema

Schema name: `Transfer`

### Table: `Users`

| Column     | Type           | Constraints           |
|------------|----------------|-----------------------|
| `UserId`   | INT            | PK, Identity(1,1)     |
| `FullName` | NVARCHAR(100)  | NOT NULL              |
| `Email`    | NVARCHAR(100)  | NOT NULL, UNIQUE      |
| `Password` | NVARCHAR(255)  | NOT NULL              |

**Indexes:**
| Index | Column | Type |
|---|---|---|
| `PK_Users` | `UserId` | Primary Key (clustered) |
| `UQ_Users_Email` | `Email` | Unique (auto-created by UNIQUE constraint) |

---

### Table: `Accounts`

| Column          | Type           | Constraints                        |
|-----------------|----------------|------------------------------------|
| `AccountNumber` | INT            | PK, Identity(1,1)                  |
| `AccountName`   | NVARCHAR(100)  | NOT NULL                           |
| `UserId`        | INT            | NOT NULL, FK → Users(UserId)       |
| `Balance`       | DECIMAL(18,2)  | NOT NULL, DEFAULT 0, CHECK >= 0    |

**Indexes:**
| Index | Column | Type |
|---|---|---|
| `PK_Accounts` | `AccountNumber` | Primary Key (clustered) |
| `FK_Accounts_Users` | `UserId` | Foreign Key (non-clustered, auto-created) |

---

### Table: `Transactions`

| Column              | Type           | Constraints                                      |
|---------------------|----------------|--------------------------------------------------|
| `TransactionId`     | INT            | PK, Identity(1,1)                                |
| `AccountNumberFrom` | INT            | NOT NULL, FK → Accounts(AccountNumber)           |
| `AccountNameFrom`   | NVARCHAR(100)  | NOT NULL                                         |
| `AccountNumberTo`   | INT            | NOT NULL, FK → Accounts(AccountNumber)           |
| `AccountNameTo`     | NVARCHAR(100)  | NOT NULL                                         |
| `Amount`            | DECIMAL(18,2)  | NOT NULL, CHECK > 0                              |
| `Status`            | NVARCHAR(20)   | NOT NULL, DEFAULT 'SUCCESS', CHECK IN ('SUCCESS', 'FAILED', 'PENDING') |
| `CreatedAt`         | DATETIME       | NOT NULL, DEFAULT GETDATE()                      |

**Indexes:**
| Index | Column | Type |
|---|---|---|
| `PK_Transactions` | `TransactionId` | Primary Key (clustered) |
| `FK_Transactions_AccountFrom` | `AccountNumberFrom` | Foreign Key (non-clustered, auto-created) |
| `FK_Transactions_AccountTo` | `AccountNumberTo` | Foreign Key (non-clustered, auto-created) |

---

### Relationships

```
Users (1) ──── (many) Accounts
Accounts (1) ──── (many) Transactions (as sender)
Accounts (1) ──── (many) Transactions (as receiver)
```

### Seed Data

**Users:**
| UserId | FullName        | Email               |
|--------|-----------------|---------------------|
| 1      | John Doe        | john@email.com      |
| 2      | Jane Smith      | jane@email.com      |
| 3      | Bob Johnson     | bob@email.com       |
| 4      | Alice Brown     | alice@email.com     |
| 5      | Charlie Wilson  | charlie@email.com   |

**Accounts:**
| AccountNumber | AccountName          | Balance  |
|---------------|----------------------|----------|
| 1             | Johns Savings        | 5000.00  |
| 2             | Janes Checking       | 3200.00  |
| 3             | Bobs Wallet          | 1500.00  |
| 4             | Alices Main Account  | 8750.00  |
| 5             | Charlies Fund        | 250.00   |

---

## ⚙️ Stored Procedures

### `sp_GetAccountBalance`
Returns the balance and details of a given account.

```sql
EXEC [Transfer].sp_GetAccountBalance @AccountNumber = 1;
```

**Returns:** `AccountNumber`, `AccountName`, `FullName`, `Balance`

**Errors:** Account not found → raises error

---

### `sp_TransferFund`
Transfers an amount between two accounts and records the result in Transactions.

```sql
EXEC [Transfer].sp_TransferFund
    @FromAccountNumber = 1,
    @ToAccountNumber   = 2,
    @Amount            = 500.00;
```

**Validations:**
- Amount must be greater than 0
- Sender account must exist
- Receiver account must exist
- Sender must have sufficient balance

**On success:** deducts from sender, adds to receiver, inserts `SUCCESS` transaction.

**On failure:** rolls back balance changes, inserts `FAILED` transaction.

---

### `sp_GetLast5Transactions`
Returns the 5 most recent transactions for a given account (as sender or receiver).

```sql
EXEC [Transfer].sp_GetLast5Transactions @AccountNumber = 1;
```

**Returns:** `TransactionId`, `AccountNumberFrom`, `AccountNameFrom`, `AccountNumberTo`, `AccountNameTo`, `Amount`, `Status`, `CreatedAt`

**Errors:** Account not found → raises error

---

## 📡 Sample Endpoints & Payloads

### MVC Routes

| Method | Route              | Description                         |
|--------|--------------------|-------------------------------------|
| GET    | `/Login`           | Show login page                     |
| POST   | `/Login`           | Authenticate user                   |
| GET    | `/Login/Register`  | Show register page                  |
| POST   | `/Login/Register`  | Register a new user                 |
| GET    | `/Transfer`        | Show transfer form (requires login) |
| POST   | `/Transfer`        | Submit a transfer                   |
| GET    | `/Transfer/Logout` | Logout and clear session            |

**Transfer validation rules:**
- Source and destination account must be selected
- Amount must be greater than 0
- Amount must not exceed source account balance
- Description is required and must not exceed 40 characters

---

### REST API — Accounts

Base URL: `https://localhost:{PORT}/api`

#### `POST /api/accounts`

**Request Body:**
```json
{
  "name": "Jordan",
  "number": "123456"
}
```

**Response `201 Created`:**
```json
{
  "name": "Jordan",
  "number": "123456"
}
```

**Response `400 Bad Request`** — name or number is missing.

**Response `409 Conflict`** — account number already exists.

---

#### `GET /api/accounts/{number}`

**Response `200 OK`:**
```json
{
  "name": "Jordan",
  "number": "123456"
}
```

**Response `404 Not Found`** — account number does not exist.

---

### REST API — Transactions

#### `POST /api/transactions`

**Request Body:**
```json
{
  "accountNumberFrom": "123456",
  "accountNumberTo": "789012",
  "amount": 500000,
  "time": "2026-03-18T10:00:00"
}
```

**Response `201 Created`:**
```json
{
  "transactionId": "a3f1c2d4-...",
  "accountNumberFrom": "123456",
  "accountNumberTo": "789012",
  "amount": 500000,
  "time": "2026-03-18T10:00:00"
}
```

---

#### `GET /api/transactions?accountnumberfrom={number}`

**Response `200 OK`:**
```json
{
  "transactionId": "a3f1c2d4-...",
  "accountNumberFrom": "123456",
  "accountNumberTo": "789012",
  "amount": 500000,
  "time": "2026-03-18T10:00:00"
}
```

**Response `400 Bad Request`** — `accountnumberfrom` query param is missing.

**Response `404 Not Found`** — no transactions found for that account.

---

## 🧪 Unit Tests

Tests cover the MVC `TransferController` using xUnit:

| Test | Description |
|------|-------------|
| `Transfer_ValidRequest_ReturnsCorrectFormModel` | Valid transfer returns correct result view with form data |
| `Transfer_ValidRequest_SourceBalanceDecreases` | Valid transfer correctly deducts balance from source account |
| `Transfer_ZeroAmount_ReturnsBadRequest` | Amount = 0 returns 400 Bad Request |
| `Transfer_NegativeAmount_ReturnsBadRequest` | Negative amount returns 400 Bad Request |
| `Transfer_AmountExceedsBalance_ReturnsBadRequest` | Amount > balance returns 400 Bad Request |

---

## 🔄 CI/CD

This project uses **GitHub Actions** for automated build and test on every push.

Pipeline file: `.github/workflows/ci.yml`

**Pipeline steps:**
1. Checkout code
2. Setup .NET 8
3. Restore dependencies
4. Build (Release)
5. Run xUnit tests + collect coverage

---

## 📄 License

This project is for educational purposes.
