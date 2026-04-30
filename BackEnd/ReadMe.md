# ProductManagement V2

Production-style ASP.NET Core Web API for product lifecycle management, user/role administration, claims-based authorization, and dashboard analytics.

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Quick Start](#quick-start)
- [Authentication](#authentication)
- [Default Admin Account](#default-admin-account)
- [Roles and Permissions](#roles-and-permissions)
- [API Endpoints](#api-endpoints)
- [Authorization Policies](#authorization-policies)
- [Database Features](#database-features)
- [Validation](#validation)
- [JWT Usage](#jwt-usage)
- [Future Improvements](#future-improvements)

---

## Overview

`ProductManagement V2` demonstrates enterprise backend architecture using:

- Clean structure
- CQRS with MediatR
- ASP.NET Core Identity
- JWT authentication + refresh tokens
- FluentValidation pipeline

It includes product workflow management, status history, role/claim authorization, and operational dashboard statistics.

---

## Features

- Product management (`CRUD` + soft delete)
- Product status workflow
- Product status history tracking
- User management
- Role and claim-based authorization
- JWT authentication
- Refresh token support
- Statistics dashboard
- Admin seeder account
- Swagger API documentation
- Validation pipeline
- Audit tracking

---

## Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT Bearer Authentication
- MediatR
- FluentValidation
- AutoMapper
- Swagger / Swashbuckle

---

## Architecture

### Patterns Used

- CQRS (commands and queries separation)
- Feature-based folder structure
- Thin controllers
- MediatR handlers
- Result pattern
- `ApiResponse` wrapper
- Global validation pipeline
- Soft delete pattern
- Audit interceptor

---

## Quick Start

```bash
git clone <repo-url>
cd "ProductManagement V2"
dotnet restore
dotnet ef database update
dotnet run
```

Swagger URL:

```text
https://localhost:xxxx/swagger
```

---

## Authentication

### Login

`POST /api/auth/login`

Request:

```json
{
  "email": "admin@system.com",
  "password": "Admin@123"
}
```

Response:

```json
{
  "success": true,
  "data": {
    "id": "guid",
    "fullName": "System Admin",
    "email": "admin@system.com",
    "role": "ProjectManager",
    "claims": [],
    "token": "jwt-token",
    "expiresAt": "utc-date",
    "refreshToken": "refresh-token"
  }
}
```

---

## Default Admin Account

Automatically created if no `ProjectManager` user exists.

```text
Email: admin@system.com
Password: Admin@123
Role: ProjectManager
```

---

## Roles and Permissions

### Roles

- ProjectManager
- Supervisor
- WarehouseManager

### Claims / Permissions

- products:view
- products:create
- products:delete
- products:change-status
- users:view
- users:create
- statistics:view
- product-status-histories:view

---

## API Endpoints

### Products API

| Method | Endpoint | Description |
| --- | --- | --- |
| GET | `/api/products` | Get paginated products |
| POST | `/api/products` | Create a product |
| GET | `/api/products/{id}` | Get product details |
| PATCH | `/api/products/{id}/status` | Change product status |
| DELETE | `/api/products/{id}` | Soft delete product |

Create product request:

```json
{
  "name": "Laptop",
  "description": "Gaming laptop",
  "price": 2500,
  "quantity": 10
}
```

Change product status request:

```json
{
  "status": "OutOfStock"
}
```

### Users API

| Method | Endpoint | Description |
| --- | --- | --- |
| GET | `/api/users` | Get paginated users |
| POST | `/api/users` | Create user |
| GET | `/api/users/{id}` | Get user details |

Create user request:

```json
{
  "fullName": "John Doe",
  "email": "john@test.com",
  "password": "User@123",
  "confirmPassword": "User@123",
  "role": "Supervisor"
}
```

### Roles API

| Method | Endpoint | Description |
| --- | --- | --- |
| GET | `/api/roles` | Get all roles |
| GET | `/api/roles/{id}/claims` | Get claims for role |

### Statistics API

| Method | Endpoint | Description |
| --- | --- | --- |
| GET | `/api/statistics` | Get dashboard statistics |

Response:

```json
{
  "products": {
    "total": 10,
    "available": 5,
    "outOfStock": 3,
    "discontinued": 2
  },
  "statusChanges": {
    "total": 25
  },
  "users": {
    "total": 6,
    "active": 5,
    "inactive": 1
  }
}
```

---

## Authorization Policies

Protected policies:

- ProductsView
- ProductsCreate
- ProductsDelete
- ProductsChangeStatus
- UsersView
- UsersCreate
- StatisticsView

---

## Database Features

### Soft Delete

Products are not physically deleted.

### Audit Fields

Automatically stores:

- CreatedAt
- CreatedBy
- UpdatedAt
- UpdatedBy

### Views

- `UsersWithRoles`

---

## Validation

Request validation is implemented with `FluentValidation` through a MediatR `PipelineBehavior`.

---

## JWT Usage

Use this request header:

```text
Authorization: Bearer YOUR_TOKEN
```

---

## Future Improvements

- Email confirmation
- Forgot password flow
- Multi refresh tokens per device
- Rate limiting
- Caching
- Logging dashboard
- Frontend integration

---

## Author

Built as an advanced backend portfolio project using enterprise architecture principles.

