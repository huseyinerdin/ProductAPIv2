# Product APIv2

This is a scalable **.NET 8 Web API** application developed with **Onion Architecture** and **CQRS Pattern** principles.  
It contains two main services:

- **Auth Service** → User registration and login (with JWT Authentication)  
- **Product Service** → Product CRUD operations with Redis Cache integration  

## 🚀 Technologies Used
- .NET 8 Web API
- Entity Framework Core
- Redis
- CQRS Pattern
- PostgreSQL
- Swagger
- Layered Architecture (API – Application – Domain – Infrastructure)
- Global Exception Middleware
- Serilog for Logging

## 📂 Project Structure
```
📂 ProductAPI
├── ProductAPI.API            🟦 Controllers, Program.cs, Middleware
├── ProductAPI.Application    🟩 DTOs, Services, Commands, Queries, Mapping Profiles
├── ProductAPI.Domain         🟨 Entities
└── ProductAPI.Infrastructure 🟫 DbContext, Repositories, Helpers
```
## ⚙️ Setup
1. Clone the repository:
```bash
git clone <repo-url>
cd ProductAPIv2
```

2. Configure PostgreSQL connection in ./ProductAPI.API/appsettings.json:
```bash
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=ProductAPIv2Db;Username=postgres;Password=p1234"
}
```

3. Install Redis on your machine or use Docker:
```bash
docker run -d --name my-redis -p 6379:6379 redis
```

4. Configure Redis connection in ./ProductAPI.API/appsettings.json:
```bash
  "Redis": {
    "ConnectionString": "localhost:6379"
  }
```
3. Run the project:
```bash
dotnet run --project ./ProductAPI.API
```
## Swagger UI
```bash
http://localhost:5010/swagger
```
## 📌 API Endpoints
```plaintext
POST /api/auth/register → Register a new user (returns JWT token)

POST /api/auth/login → User login (returns JWT token)

POST /api/products → Add a new product

GET /api/products → Get all products

GET /api/products/{id} → Get product by ID

DELETE /api/products/{id} → Delete a product
```

## 📝 License
This project is licensed under the MIT License.