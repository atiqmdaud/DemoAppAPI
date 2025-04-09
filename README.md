# API Key Management & CRUD Operations Demo Project

## Overview

This demo project is a minimal API built with **ASP.NET Core** to provide functionality for API key generation, management, rate-limiting, and CRUD operations for items. It leverages **Entity Framework Core** for database interaction and demonstrates the use of middleware for authentication and request throttling.

---

## Features

### **API Key Management**

- Generate API keys for users.
- Validate API keys through middleware.
- Track and limit API requests per key.
- Respond with appropriate HTTP status codes (`401`, `429`, etc.).

### **Rate-Limiting**

- Enforce request limits per API key.
- Block requests exceeding limits with `429 Too Many Requests`.

### **Item CRUD Operations**

- Create, Read, Update, and Delete item entities.

---

## Endpoints

### **API Key Management Endpoints**

| Method | Endpoint                       | Description             |
| ------ | ------------------------------ | ----------------------- |
| POST   | `/apikeys/generate?user=user1` | Generate a new API key. |

### **Item CRUD Endpoints**

| Method | Endpoint      | Description              |
| ------ | ------------- | ------------------------ |
| POST   | `/items`      | Create a new item.       |
| GET    | `/items`      | Retrieve all items.      |
| GET    | `/items/{id}` | Retrieve an item by ID.  |
| PUT    | `/items/{id}` | Update an existing item. |
| DELETE | `/items/{id}` | Delete an item by ID.    |

---

## Installation

### **Prerequisites**

1. Install the [.NET SDK](https://dotnet.microsoft.com/download).
2. Set up a local or remote instance of **SQL Server**.

### **Step 1: Clone the Repository**

```bash
git clone https://github.com/atiqmdaud/DemoAppAPI.git
cd DemoAppAPI
```

### **Step 2: Install Dependencies**

Run the following command to restore NuGet dependencies:

```bash
dotnet restore
```

### **Step 3: Configure the Database**

Update the connection string in appsettings.json:

```Json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  }
}
```

### **Step 4: Apply Migrations**

Set up the database by running EF Core migrations:

```Bash
dotnet ef database update
```

### **Start the API**

Run the project using the following command:

```Bash
dotnet run
```

### **Testing**

Include headers like X-API-KEY for API key validation
