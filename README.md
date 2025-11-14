# 📦 WiseByteDatabaseHelper
### _Lightweight attribute-driven database table generator for .NET_

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
![NuGet](https://img.shields.io/badge/NuGet-coming_soon-blue)
![Build](https://img.shields.io/badge/Build-passing-brightgreen)
![Platform](https://img.shields.io/badge/Platform-.NET_8-blue)

---

## 🚀 Overview

WiseByteDatabaseHelper is a lightweight, reflection-based ORM-style helper library—inspired by EF Core—that automatically generates database tables from C# classes using simple custom attributes, while giving developers the freedom to create or update the database schema at runtime without any CLI tools.
It provides:

- ✔ **Custom attributes** for tables, primary keys, auto-increment, foreign keys  
- ✔ **DbContext-like workflow**  
- ✔ **Automatic CREATE TABLE generation**  
- ✔ **Foreign key detection & correct ordering**  
- ✔ **SQLite support** (SQL Server & PostgreSQL planned)  
- ✔ **Schema update checks** (`Update<T>()`)  
- ✔ **Extensible SQL dialect provider**

Perfect for small and medium projects where EF Core is overkill.



## 📁 Project Structure

``` csharp
WiseByteDatabaseHelper/
│
├── WiseByteDatabaseHelper.Core/
│   ├── Attributes/
│   │   ├── TableAttribute.cs
│   │   ├── ColumnAttribute.cs
│   │   ├── PrimaryKeyAttribute.cs
│   │   ├── AutoIncrementAttribute.cs
│   │   ├── ForeignKeyAttribute.cs
│   │
│   ├── Dialects/
│   │   ├── ISqlDialect.cs
│   │   ├── SqliteDialect.cs
│   │   ├── SqlServerDialect.cs (future)
│   │   ├── PostgreSqlDialect.cs (future)
│   │
│   ├── Engine/
│   │   ├── TypeParser.cs
│   │   ├── TableBuilder.cs
│   │   ├── ForeignKeyResolver.cs
│   │   ├── SqlGenerator.cs
│   │
│   ├── Context/
│   │   ├── DatabaseContext.cs
│   │   ├── TableSet.cs
│   │
│   └── WiseByteDatabaseHelper.Core.csproj
│
└── WiseByteDatabaseHelper.Tests/
    ├── DatabaseCreationTests.cs
    └── WiseByteDatabaseHelper.Tests.csproj
```

### 📥 Installation (NuGet)

Coming soon:

``` cli
dotnet add package WiseByteDatabaseHelper
```

### 📝 Usage Example

#### 1. Define your models

```csharp
using WiseByteDatabaseHelper.Core.Attributes;

[Table("Users")]
public class User
{
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }

    [Column]
    public string Name { get; set; }
}

[Table("Orders")]
public class Order
{
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }

    [ForeignKey<User>("Id")]
    public int UserId { get; set; }

    [Column]
    public string Item { get; set; }
}
```

### 2. Define your custom DbContext

```
public class AppDatabaseContext : DatabaseContext
{
    public AppDatabaseContext()
    {
        UseSqlite("Data Source=app.db");
        AddTable<User>();
        AddTable<Order>();
    }
}
```

### 3️. Create the database

```csharp
var db = new AppDatabaseContext();
await db.CreateDatabaseAsync();
```

### 4️. Update the schema of a single table

```csharp
await db.UpdateTableAsync<User>();
```

This will:

- Detect missing columns
- Detect foreign key relationship problems
- Refuse destructive updates unless safe

## 🧪 Testing

The WiseByteDatabaseHelper.Tests project includes:

- ✔ Table creation tests
- ✔ Foreign key ordering tests
- ✔ Schema update tests
- ✔ SQLite integration test

Run tests:

``` csharp
dotnet test
```

## 🛠 Roadmap

- 🔹 Add SQL Server Dialect
- 🔹 Add PostgreSQL Dialect
- 🔹 Add table diff + migration generator
- 🔹 Add fluent API alternative to attributes
- 🔹 Add insert/update helper methods

## 📄 License

This project is licensed under the MIT License.