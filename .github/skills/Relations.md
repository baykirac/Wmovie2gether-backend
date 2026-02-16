# WMovie2Gether - Entity Relations & Project Context

> **Bu dosya proje entity ilişkilerini ve domain context'ini tanımlar.**
> **Copilot bu dosyayı okuyarak proje yapısını anlayabilir.**

---

## 📊 Entity Relationship Diagram (ERD)

```
┌─────────────────────────────────────────────────────────────────┐
│                          USER                                    │
├─────────────────────────────────────────────────────────────────┤
│ Id (PK, long)                                                    │
│ Username (unique, max 50)                                        │
│ Email (unique, max 100)                                          │
│ PasswordHash (max 255)                                           │
│ DisplayName (nullable, max 100)                                  │
│ IsActive (soft delete)                                           │
│ LastLoginAt (nullable)                                           │
│ CreatedAt                                                        │
│ UpdatedAt (nullable)                                             │
├─────────────────────────────────────────────────────────────────┤
│                         RELATIONSHIPS                            │
│ └── Folders (1:N) → User has many Folders                        │
└─────────────────────────────────────────────────────────────────┘
                              │
                              │ 1:N
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                         FOLDER                                   │
├─────────────────────────────────────────────────────────────────┤
│ Id (PK, long)                                                    │
│ Name (max 100)                                                   │
│ Description (nullable, max 500)                                  │
│ UserId (FK → User.Id)                                            │
│ IsActive (soft delete)                                           │
│ CreatedAt                                                        │
│ UpdatedAt (nullable)                                             │
├─────────────────────────────────────────────────────────────────┤
│                         CONSTRAINTS                              │
│ - Unique: (UserId, Name) → Same user cannot have duplicate names │
│ - FK: UserId → User.Id (CASCADE DELETE)                          │
└─────────────────────────────────────────────────────────────────┘
```

---

## 🔗 Entity Relationships

### User → Folder (One-to-Many)
| Parent | Child | Relationship | Delete Behavior |
|--------|-------|--------------|-----------------|
| User | Folder | 1:N | CASCADE |

**Açıklama:**
- Bir **User** birden fazla **Folder**'a sahip olabilir
- Bir **Folder** yalnızca bir **User**'a ait olabilir
- User silindiğinde (soft delete), ilişkili Folder'lar da etkilenir

---

## 📁 Entities Summary

### User
```csharp
// Encapsulated entity with private setters
// Static Create method for instantiation
// Soft delete via IsActive property
// Methods: Create(), Update(), Deactivate(), Activate(), UpdateLastLogin()
```

**Business Rules:**
- Email must be unique across all users
- Username must be unique across all users
- Delete operation sets IsActive = false (soft delete)
- GetById and GetAll filter by IsActive = true

### Folder
```csharp
// Encapsulated entity with private setters
// Static Create method for instantiation
// Soft delete via IsActive property
// Methods: Create(), Update(), Deactivate(), Activate()
```

**Business Rules:**
- Folder name must be unique per user (same user cannot have two folders with same name)
- Delete operation sets IsActive = false (soft delete)
- Folder creation requires valid UserId

---

## 📋 Domain Rules

### Soft Delete
- All entities use `IsActive` property for soft delete
- Delete operations call `entity.Deactivate()` instead of removing from database
- All queries (GetById, GetAll) filter by `IsActive = true`

### Entity Encapsulation
- All entities have private parameterless constructors (for EF Core)
- All entities have static `Create()` methods
- Properties have private setters
- State changes through domain methods (Update, Deactivate, Activate)

### Unique Constraints
| Entity | Unique Fields | Scope |
|--------|---------------|-------|
| User | Email | Global |
| User | Username | Global |
| Folder | Name | Per User |

---

## 🗄️ Database Tables

### Users Table
```sql
CREATE TABLE "Users" (
    "Id" bigint GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    "Username" varchar(50) NOT NULL,
    "Email" varchar(100) NOT NULL,
    "PasswordHash" varchar(255) NOT NULL,
    "DisplayName" varchar(100),
    "IsActive" boolean NOT NULL DEFAULT true,
    "LastLoginAt" timestamp with time zone,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);

CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");
CREATE UNIQUE INDEX "IX_Users_Username" ON "Users" ("Username");
```

### Folders Table
```sql
CREATE TABLE "Folders" (
    "Id" bigint GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    "Name" varchar(100) NOT NULL,
    "Description" varchar(500),
    "UserId" bigint NOT NULL,
    "IsActive" boolean NOT NULL DEFAULT true,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "FK_Folders_Users_UserId" FOREIGN KEY ("UserId") 
        REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_Folders_UserId_Name" ON "Folders" ("UserId", "Name");
```

---

## 🔄 Future Entities (Placeholder)

> Yeni entity'ler eklendiğinde bu bölüm güncellenecek.

```
┌─────────────────────────────────────────────────────────────────┐
│                        FUTURE ENTITIES                           │
├─────────────────────────────────────────────────────────────────┤
│ Room        → Folder ile 1:N ilişkili (bir folder birden fazla   │
│               room içerebilir)                                   │
│ Video       → Room ile 1:N ilişkili                              │
│ ChatMessage → Room ile 1:N ilişkili                              │
│ RoomMember  → User ve Room arasında N:N ilişki tablosu           │
└─────────────────────────────────────────────────────────────────┘
```

---

## 📝 API Endpoints Summary

### Users
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/users | Get all active users |
| GET | /api/users/{id} | Get user by ID |
| POST | /api/users | Create new user |
| PUT | /api/users/{id} | Update user |
| DELETE | /api/users/{id} | Soft delete user |

### Folders
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/folders | Get all active folders |
| GET | /api/folders/{id} | Get folder by ID |
| GET | /api/folders/user/{userId} | Get folders by user ID |
| POST | /api/folders | Create new folder |
| PUT | /api/folders/{id} | Update folder |
| DELETE | /api/folders/{id} | Soft delete folder |

---

## 🏗️ Layer Structure

```
WMovie2Gether.Domain/
├── Entities/
│   ├── BaseEntity.cs
│   ├── User.cs
│   └── Folder.cs
├── DTOs/
│   ├── User/
│   │   ├── UserDto.cs
│   │   ├── CreateUserDto.cs
│   │   └── UpdateUserDto.cs
│   └── Folder/
│       ├── FolderDto.cs
│       ├── CreateFolderDto.cs
│       └── UpdateFolderDto.cs
├── Interfaces/
│   ├── IBaseRepository.cs
│   ├── IUserRepository.cs
│   ├── IFolderRepository.cs
│   └── IUnitOfWork.cs
├── Mappings/
│   ├── UserMappingProfile.cs
│   └── FolderMappingProfile.cs
├── Resources/
│   ├── UserMessages.resx
│   └── FolderMessages.resx
└── ValueObjects/
    └── Result.cs

WMovie2Gether.Application/
├── Extensions/
│   └── SecurityExtensions.cs
├── Interfaces/
│   ├── IUserService.cs
│   └── IFolderService.cs
└── Services/
    ├── UserService.cs
    └── FolderService.cs

WMovie2Gether.Infrastructure/
├── Data/
│   ├── ApplicationDbContext.cs
│   └── Migrations/
├── Repositories/
│   ├── BaseRepository.cs
│   ├── UserRepository.cs
│   └── FolderRepository.cs
└── UnitOfWork/
    └── UnitOfWork.cs

WMovie2Gether.API/
├── Controllers/
│   ├── UsersController.cs
│   └── FoldersController.cs
└── Program.cs
```
