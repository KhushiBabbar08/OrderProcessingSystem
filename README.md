# Order Processing System 🚀

A scalable and production-ready **Order Processing System** built using **ASP.NET Core Web API**, demonstrating modern backend engineering practices including **Redis caching**, **Kafka event-driven communication**, and **Docker containerization**.

---

## 📌 Project Overview

This project simulates a real-world enterprise order management backend where customers can create and manage orders while the system efficiently processes requests using distributed architecture concepts.

The application is designed to showcase:

- Clean backend architecture
- Scalable API design
- Event-driven communication
- Distributed caching
- Containerized deployment
- Enterprise-grade development practices

---

# 🛠️ Tech Stack

| Technology | Purpose |
|---|---|
| ASP.NET Core Web API | Backend API |
| Entity Framework Core | ORM |
| SQL Server | Primary Database |
| Redis | Distributed Caching |
| Apache Kafka | Event Streaming |
| Docker | Containerization |
| Swagger | API Documentation |

---

# ✨ Features

## ✅ Order Management
- Create Orders
- Get Order Details
- Update Order Status
- Delete Orders

## ✅ Database Integration
- SQL Server Integration
- Entity Framework Core
- Code First Approach
- Database Migrations

## ✅ Redis Caching
- Faster Order Retrieval
- Reduced Database Load
- Distributed Cache Handling

## ✅ Kafka Messaging
- Publish Order Events
- Event-Driven Architecture
- Asynchronous Processing

## ✅ Docker Support
- Dockerized API
- Environment Independent Deployment
- Easy Local Setup

---

# 🏗️ Architecture

```text
OrderProcessingSystem
│
├── Controllers
├── Services
├── Repositories
├── Models
├── Data
├── Kafka
├── Redis
├── Docker
└── Configurations
```

---

# 🔥 System Workflow

```text
Client Request
      ↓
API Controller
      ↓
Business Service
      ↓
Repository Layer
      ↓
SQL Server Database
      ↓
Redis Cache Update
      ↓
Kafka Event Publish
```

---

# ⚡ Getting Started

## 1️⃣ Clone Repository

```bash
git clone https://github.com/KhushiBabbar08/OrderProcessingSystem.git
```

---

## 2️⃣ Navigate to Project

```bash
cd OrderProcessingSystem
```

---

## 3️⃣ Update Connection String

Update `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=OrderProcessingDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

---

## 4️⃣ Run Database Migration

```bash
dotnet ef database update
```

---

## 5️⃣ Run Application

```bash
dotnet run
```

---

# 🐳 Docker Support

Build Docker Image:

```bash
docker build -t order-processing-system .
```

Run Container:

```bash
docker run -p 8080:8080 order-processing-system
```

---

# 📡 Kafka Integration

Kafka is used for:

- Order Created Events
- Order Updated Events
- Event Streaming
- Asynchronous Processing

Example Event:

```json
{
  "orderId": 101,
  "status": "Created",
  "createdAt": "2026-05-23T18:00:00"
}
```

---

# ⚡ Redis Caching

Redis is implemented to:

- Improve API performance
- Reduce repeated database hits
- Cache frequently accessed orders

---

# 📘 API Documentation

Swagger UI available at:

```text
https://localhost:xxxx/swagger
```

---

# 🚀 Future Enhancements

- JWT Authentication
- Role-Based Authorization
- Payment Gateway Integration
- Inventory Service
- Email Notifications
- Kubernetes Deployment
- CI/CD Pipeline
- Microservices Migration

---

# 🧠 Learning Goals

This project is built for hands-on learning of:

- Distributed Systems
- Backend Scalability
- Event-Driven Architecture
- High Performance APIs
- Docker & Containerization
- Redis & Kafka Integration

---

# 👨‍💻 Author

### Khushi Babbar

Backend Engineer | .NET Developer | Cloud & Distributed Systems Enthusiast

GitHub:  
https://github.com/KhushiBabbar08

---

# ⭐ Support

If you found this project useful:

- Star the repository ⭐
- Fork the project 🍴
- Share feedback 🚀

---

# 📄 License

This project is licensed under the MIT License.