# Project Name

> Brief description of your project here.

---

## 📋 Table of Contents

- [Setup Project](#setup-project)
- [Tools & Technologies](#tools--technologies)
- [Framework Versions](#framework-versions)
- [Data Storage](#data-storage)
- [Sample Endpoints & Payloads](#sample-endpoints--payloads)

---

## 🚀 Setup Project

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code

### Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/your-repo.git
   cd your-repo
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Access the app**

   - MVC: `https://localhost:{PORT}`
   - API + Swagger UI: `https://localhost:{PORT}/swagger`

---

## 🛠️ Tools & Technologies

| Category         | Tool / Library                      |
|------------------|-------------------------------------|
| Language         | C# 12                               |
| MVC Framework    | ASP.NET Core MVC 8                  |
| API Framework    | ASP.NET Core Web API 8              |
| Data Storage     | In-memory local variables (no DB)   |
| API Docs         | Swagger / Swashbuckle               |
| Unit Testing     | xUnit                               |
| IDE              | Visual Studio 2022                  |
| CI/CD            | GitHub Actions                      |
| Version Control  | Git + GitHub                        |

---

## 📦 Framework Versions

| Framework / Package              | Version   |
|----------------------------------|-----------|
| .NET                             | 8.0       |
| ASP.NET Core                     | 8.0       |
| Swashbuckle.AspNetCore           | 6.x       |
| xUnit                            | 2.x       |

---

## 🗄️ Data Storage

Neither the MVC nor the API project uses a database. Both store data in **local in-memory variables**, meaning all data resets when the application restarts.

Example:
```csharp
private static List<Product> _products = new List<Product>
{
    new Product { Id = 1, Name = "Laptop", Price = 15000000 },
    new Product { Id = 2, Name = "Mouse",  Price = 250000 }
};
```

---

## 📡 Sample Endpoints & Payloads

Base URL: `https://localhost:{PORT}/api`

---

### `GET /api/products`

Returns a list of all products.

**Response `200 OK`:**
```json
[
  {
    "id": 1,
    "name": "Laptop",
    "price": 15000000
  },
  {
    "id": 2,
    "name": "Mouse",
    "price": 250000
  }
]
```

---

### `GET /api/products/{id}`

Returns a single product by ID.

**Response `200 OK`:**
```json
{
  "id": 1,
  "name": "Laptop",
  "price": 15000000
}
```

**Response `404 Not Found`:**
```json
{
  "message": "Product not found."
}
```

---

### `POST /api/products`

Creates a new product.

**Request Body:**
```json
{
  "name": "Keyboard",
  "price": 500000
}
```

**Response `201 Created`:**
```json
{
  "id": 3,
  "name": "Keyboard",
  "price": 500000
}
```

---

### `PUT /api/products/{id}`

Updates an existing product.

**Request Body:**
```json
{
  "name": "Keyboard Pro",
  "price": 750000
}
```

**Response `200 OK`:**
```json
{
  "id": 3,
  "name": "Keyboard Pro",
  "price": 750000
}
```

---

### `DELETE /api/products/{id}`

Deletes a product by ID.

**Response `204 No Content`**

---

## ⚙️ CI/CD

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
