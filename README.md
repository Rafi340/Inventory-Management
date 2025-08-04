# Inventory Management System

An enterprise-grade **Inventory Management System** built using modern technologies for scalability and efficiency.

### Pattern Overview
| Architectural Pattern  | Implementation Details                          | Benefits |
|-----------------------|-----------------------------------------------|----------|
| **Repository Pattern** | Abstraction layer between DAL and business logic | Persistence ignorance, testability |
| **Mediator Pattern**  | CQRS implementation via MediatR library        | Decoupled components, single responsibility |
| **CQRS**              | Separate command and query models              | Scalability, optimized reads/writes |
| **Unit of Work**      | Transaction management across repositories     | Atomic operations, data consistency |

## ✨ Key Features

### 🔐 Security & Access
- **Role-based User Management** - Create and manage user accounts with granular permissions
- **Authentication** - Secure login system
- 
### Identity Framework Integration
- **Extended User Management**: Custom `ApplicationUser` with additional profile properties
- **Role Management**: Custom `ApplicationRole` with role descriptions
### Claim-Based Authorization
- **Custom Claim Types**: Granular permissions (e.g., `Inventory.View`, `Sales.Approve`)
- **Claim Seeding**: Automatic assignment of default claims to roles

### 📦 Inventory Core
- **Product Catalog** - Full CRUD operations for inventory items
- **Stock Tracking** - Real-time inventory levels
- **Barcode Support** - Product identification system

### 💰 Sales & Finance
- **Sales Processing** - Complete transaction recording
- **Customer Management** - Maintain customer profiles
- **Balance Transfers** - Inter-account fund movements

### 🚀 Advanced Features
- **Cloud Image Storage** - AWS S3 integration for product images
- **Email Notifications** - AWS SQS-powered messaging
- **Background Workers** - Automated processing tasks

## 🛠 Technology Stack

### Backend
| Component          | Technology               |
|--------------------|--------------------------|
| Framework          | ASP.NET Core MVC (.NET 9)|
| ORM                | Entity Framework Core 8  |
| Background Workers | .NET Worker Service      |

### Frontend
| Component          | Technology               |
|--------------------|--------------------------|
| UI Framework       | Vuexy theme              |
| Templating         | Razor Pages              |

### Infrastructure
| Service            | Technology               |
|--------------------|--------------------------|
| Cloud Storage      | AWS S3                   |
| Messaging          | AWS SQS                  |
| Containerization   | Docker + Compose         |
| Database           | SQL Server 2022          |

### Testing
| Type               | Tools                    |
|--------------------|--------------------------|
| Unit Testing       | NUnit + Moq              |
| Assertions         | Shouldly                 |

## 🚀 Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or use Docker)
- AWS Account with:
  - S3 Bucket
  - SQS Queue
  - IAM Credentials

### 🐳 Docker Setup

```bash
# 1. Clone the repository
git clone https://github.com/Rafi340/Inventory-Management.git
cd Inventory-Management

# 2. Configure environment
cp .env.example .env
# Edit the .env file with your AWS credentials

# 3. Run database migrations
dotnet ef database update --project Inventory.Web --context ApplicationDbContext

# 4. Start the application
docker-compose up --build
