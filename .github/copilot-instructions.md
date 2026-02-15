# Project Instructions for GitHub Copilot

## Technology Stack
- .NET 10 Web API
- C# latest version
- Controllers based Web API (NO Minimal APIs)
- Entity Framework Core
- PostgreSQL
- Code First approach ONLY
- async/await everywhere
- No dynamic typing
- AutoMapper for DTO mapping

---

## Architecture
- Clean Architecture is mandatory

### Layers
- API
- Application
- Domain
- Infrastructure

### Layer Responsibilities
- API:
  - Controllers only
  - Controllers return Result<T> directly (NOT IActionResult)
  - No business logic
  - No direct database access
- Application:
  - Services
  - Validators
  - Business rules
  - Extensions (helper methods like HashPassword)
- Domain:
  - Entities (with static Create methods for encapsulation)
  - Interfaces
  - Enums
  - ValueObjects
  - DTOs (organized by entity: DTOs/User/, DTOs/Room/, etc.)
  - Mappings (AutoMapper profiles in Mappings/ folder)
  - Resources (.resx files for messages)
- Infrastructure:
  - EF Core DbContext
  - Repositories
  - UnitOfWork implementation
  - Database configurations

---

## Entity Rules (VERY IMPORTANT)
- BaseEntity must use `long Id` (NOT Guid)
- Entities must use ENCAPSULATION
- Do NOT create entities with `new Entity { ... }`
- Each entity must have a static `Create()` method
- Entity properties should have private setters where appropriate
- Entities must have a private parameterless constructor for EF Core

### Soft Delete Rules
- Use `IsActive` property for soft delete (NOT hard delete)
- Delete operations must call `entity.Deactivate()` method
- GetById and GetAll must filter by `IsActive = true`
- Entities should have `Deactivate()` and `Activate()` methods

### Entity Example
```csharp
public class User : BaseEntity
{
    private User() { } // EF Core constructor

    public string Username { get; private set; }
    public string Email { get; private set; }
    public bool IsActive { get; private set; } = true;

    public static User Create(string username, string email, string passwordHash)
    {
        return new User
        {
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            IsActive = true
        };
    }

    public void Update(string? username, string? displayName, bool? isActive)
    {
        if (!string.IsNullOrWhiteSpace(username)) Username = username;
        if (displayName != null) DisplayName = displayName;
        if (isActive.HasValue) IsActive = isActive.Value;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
```

---

## DTO Rules
- DTOs must be in Domain layer under `DTOs/{EntityName}/` folder
- Use AutoMapper for mapping between Entity and DTO
- Create MappingProfile classes in Domain/Mappings/ folder
- Do NOT create manual MapToDto methods in services

---

## Resource Files (Messages)
- Use .resx files for all messages (NOT static classes)
- Place resource files in Domain/Resources/ folder
- Name format: {EntityName}Messages.resx
- Access via generated designer class (e.g., UserMessages.UserNotFound)

---

## Extensions Rules
- Create Extensions folder in each layer as needed
- Helper methods (HashPassword, etc.) must be in Extensions
- Do NOT put helper methods inside Services
- Extension methods must be static and in static classes
- Organize by functionality: SecurityExtensions, StringExtensions, etc.

---

## Persistence Rules (VERY IMPORTANT)
- Code First is mandatory
- No Database First
- All entities must be mapped via EF Core
- All entity constraints must be defined using Data Annotations
- Fluent API is allowed only if absolutely necessary

---

## Repository Pattern Rules
- Use Repository + UnitOfWork patterns together

### BaseRepository
- Create a generic BaseRepository<TEntity>
- BaseRepository must include common CRUD operations
- All entity-specific repositories must inherit from BaseRepository

### Entity Repositories
- Each entity must have its own repository interface and implementation
- Entity repositories are used in services
- If an entity needs custom queries or logic, extend its own repository
- Do NOT put entity-specific logic into BaseRepository

### Repository Access (VERY IMPORTANT)
- Services must access repositories via `unitOfWork.GetRepository<IEntityRepository>()`
- Repository should be stored as a private readonly field in the service
- GetRepository<TRepository>() should be called ONCE in the constructor
- Do NOT call GetRepository in every method

### Service Constructor Example
```csharp
public class UserService : IUserService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IUserRepository userRepository;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.userRepository = unitOfWork.GetRepository<IUserRepository>();
    }
}
```

### Incorrect Example
```csharp
User? user = await unitOfWork.Users.GetByIdAsync(id, cancellationToken); // WRONG!
```

---

## UnitOfWork Rules
- UnitOfWork must:
  - Manage DbContext lifetime
  - Expose repositories via GetRepository<T>() ONLY
  - Handle SaveChangesAsync
- No direct DbContext usage in services
- One UnitOfWork per request

---

## Validation Rules
- Entity-level constraints:
  - Use Data Annotations
- Request-level validation:
  - Use FluentValidation
- Controllers must NOT perform validation logic

---

## Result Pattern
- Create a generic Result structure
- Result must:
  - Support Success / Failure states
  - Contain Message
  - Optionally contain Data
  - Optionally contain Error details
- Services must return Result<T>
- Controllers must return Result<T> directly (NOT IActionResult)
- Do NOT return raw entities or primitives from services

---

## Coding Rules
- Explicit types only (avoid var unless type is obvious)
- No magic strings or numbers
- Constants must be centralized
- Async methods must end with Async
- Interface-based programming everywhere
- Use Dependency Injection exclusively

---

## Naming Conventions
- English only
- PascalCase for:
  - Classes
  - Methods
  - Public members
- camelCase for:
  - Local variables
  - Method parameters
- Interfaces must start with `I`

---

## Service Naming Rules (IMPORTANT)
- Service names must NOT start with underscore (_)
- Do NOT prefix service variables or fields with "_"
- Use camelCase for service variables
- Services must be injected and referenced without underscore prefix

### Correct Examples
- userService
- orderService
- authenticationService

### Incorrect Examples
- _userService
- _orderService
- _authenticationService

---

## Logging & Error Handling
- Use ILogger with structured logging
- No Console.WriteLine
- Global exception handling via middleware
- Never swallow exceptions

---

## Testing & Maintainability
- Code must be testable
- Avoid static dependencies
- Avoid tight coupling
- Prefer clarity over brevity
- Avoid overengineering

---

## General Copilot Behavior
- Follow these instructions strictly
- Do not invent alternative architectures
- Do not simplify patterns
- Do not bypass layers
- Always prefer long-term maintainability

