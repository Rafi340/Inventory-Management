# 🧾 Inventory Management System

An enterprise-grade **Inventory Management System** built using **ASP.NET Core MVC (.NET 9)** with full integration of **Entity Framework Core**, **AWS (S3 + SQS)**, **Worker Services**, and **Docker**. Designed for modern deployment and scalable backend processing.

---

## 🧩 Features

- 🔐 **User Management** — Create and manage user accounts and roles.
- 👥 **Customer Management** — Maintain detailed customer records.
- 📦 **Product Management** — Add, update, and remove inventory items.
- 🧾 **Sales Management** — Record and track sales transactions.
- 💸 **Balance Transfer** — Manage balance transfers between entities.
- 🖼️ **Image Upload** — Upload product images directly to **AWS S3**.
- 📨 **Email Notifications** — Triggered via **AWS SQS** messaging.
- ⚙️ **Background Processing** — Automated tasks via Worker Service:
  - Image resizing
  - S3 uploads
  - Email sending

---

## ⚙️ Technology Stack

| Layer              | Technology                     |
|-------------------|--------------------------------|
| Backend            | ASP.NET Core MVC (.NET 9)      |
| ORM                | Entity Framework Core          |
| Frontend           | jQuery                         |
| Messaging Queue    | AWS SQS                        |
| File Storage       | AWS S3                         |
| Background Worker  | ASP.NET Core Worker Service    |
| Testing            | NUnit, Shouldly, Moq           |
| Containerization   | Docker + Docker Compose        |
| Database           | SQL Server                     |

---

## 🧪 Testing

Automated unit tests are written using:

- ✅ [NUnit](https://nunit.org/)
- ✅ [Shouldly](https://shouldly.readthedocs.io/)
- ✅ [Moq](https://github.com/moq)

Run tests via:
```bash
dotnet test
