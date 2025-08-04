# 📦 Inventory Management System

<div align="center">

![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)
![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)
![Version](https://img.shields.io/badge/version-1.0.0-blue.svg)

*An enterprise-grade inventory management solution built with modern .NET technologies*

</div>

---

## 🌟 Overview

A comprehensive **Inventory Management System** designed for scalability, security, and efficiency. Built with modern architectural patterns and cloud-native technologies to handle enterprise-level inventory operations with ease.

## 🏗️ Architecture & Design Patterns

Our system implements industry-standard architectural patterns for maintainability and scalability:

| **Pattern** | **Implementation** | **Benefits** |
|-------------|-------------------|--------------|
| 🏛️ **Repository Pattern** | Abstraction layer between DAL and business logic | Persistence ignorance, enhanced testability |
| 🔄 **Mediator Pattern** | CQRS implementation via MediatR library | Decoupled components, single responsibility principle |
| ⚡ **CQRS** | Separate command and query models | Optimized performance, horizontal scalability |
| 🔐 **Unit of Work** | Transaction management across repositories | Atomic operations, guaranteed data consistency |

---

## ✨ Core Features

### 🔐 **Security & Identity Management**
- **Advanced User Management** - Role-based access control with granular permissions
- **Secure Authentication** - Industry-standard login and session management
- **Extended Identity Framework** - Custom `ApplicationUser` and `ApplicationRole` entities
- **Claim-Based Authorization** - Fine-grained permissions (`Inventory.View`, `Sales.Approve`, etc.)
- **Automated Role Seeding** - Default claims assignment for streamlined setup

### 📦 **Inventory Operations**
- **Product Catalog Management** - Complete CRUD operations with rich metadata
- **Real-Time Stock Tracking** - Live inventory level monitoring
- **Barcode Integration** - Efficient product identification and scanning
- **Category Management** - Hierarchical product organization

### 💰 **Sales & Financial Management**
- **Transaction Processing** - Comprehensive sales recording and tracking
- **Customer Relationship Management** - Detailed customer profiles and history
- **Balance Transfer System** - Secure inter-account fund movements
- **Financial Reporting** - Sales analytics and performance metrics

### 🚀 **Advanced Capabilities**
- **☁️ Cloud Storage** - AWS S3 integration for scalable image storage
- **📧 Email Notifications** - AWS SQS-powered asynchronous messaging
- **⚙️ Background Processing** - Automated tasks and scheduled operations
- **📊 Real-Time Dashboard** - Live system metrics and KPIs

---

## 🛠️ Technology Stack

### **Backend Infrastructure**
```
Framework:          ASP.NET Core MVC (.NET 9)
ORM:               Entity Framework Core
Background Jobs:    .NET Worker Service

```

### **Frontend Experience**
```
UI Framework:       Vuexy Premium Theme
Templating:         Razor Pages
JavaScript:         Modern ES6+
Styling:           Bootstrap 5 + Custom CSS
```

### **Cloud & Infrastructure**
```
Cloud Storage:      Amazon S3
Message Queue:      Amazon SQS
Container Platform: Docker + Docker Compose
Database:          SQL Server 2022
```

### **Development & Testing**
```
Unit Testing:       NUnit Framework
Mocking:           Moq Library
Assertions:        Shouldly
Code Coverage:     Coverlet
```

---

## 🚀 Quick Start Guide

### **Prerequisites**

Ensure you have the following installed:

- ✅ [.NET 9 SDK](https://dotnet.microsoft.com/download) (Latest version)
- ✅ [Docker Desktop](https://www.docker.com/products/docker-desktop) (For containerization)
- ✅ [SQL Server 2022](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or Docker container)
- ✅ **AWS Account** with configured services:
  - 🪣 S3 Bucket for file storage
  - 📨 SQS Queue for messaging
  - 🔑 IAM User with appropriate permissions

### **🐳 Docker Deployment (Recommended)**

Get up and running in minutes with Docker:

```bash
# Clone the repository
git clone https://github.com/Rafi340/Inventory-Management.git
cd Inventory-Management

# Set up environment configuration
cp web.env 
cp worker.env
# ⚠️ Edit .env file with your AWS credentials and database settings

# Apply database migrations
dotnet ef database update --project Inventory.Web --context ApplicationDbContext

# Launch the application stack
docker-compose up --build -d

# Access the application
# 🌐 Web Interface: http://localhost:8080
```

---

## 🔧 Configuration

### **Environment Variables**

Create a `.env` file in the root directory For Web and Worker Service:

```env
# Database Configuration
ConnectionStrings__DefaultConnection=Server=localhost;Database=InventoryDB;Trusted_Connection=true;

# AWS Configuration
AWS__Region=us-east-1
AWS__AccessKey=your_access_key_here
AWS__SecretKey=your_secret_key_here
AWS__S3BucketName=your-inventory-bucket
AWS__SQSQueueUrl=your-sqs-queue-url


---

## 🧪 Testing

Run the comprehensive test suite:

```bash
# Run all tests
dotnet test

```

---

## 📈 Performance & Scalability
  
- **Async Operations**: Non-blocking I/O for better throughput
- **Database Optimization**: Indexed queries and efficient data models


<div align="center">
**⭐ If this project helped you, please consider giving it a star! ⭐**
</div>
