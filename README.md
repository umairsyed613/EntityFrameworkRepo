# UnEntityFrameworkRepository
`UnEntityFrameworkRepository` is a lightweight, easy-to-use repository pattern implementation designed for .NET applications. It abstracts data access to make your application more testable and maintainable.

## Features

- Generic repository implementation.
- Easy integration with Entity Framework Core.
- Supports asynchronous operations.
- Configurable for any `DbContext`.
- Dependency Injection friendly.

## Status
[![Build, Package & Deploy](https://github.com/umairsyed613/UnEntityFrameworkRepository/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/umairsyed613/UnEntityFrameworkRepository/actions/workflows/dotnet.yml)
[![NuGet version](https://badge.fury.io/nu/UnEntityFrameworkRepository.svg)](https://badge.fury.io/nu/UnEntityFrameworkRepository)
[![Nuget downloads (EFDbFactory.Sql)](https://img.shields.io/nuget/dt/UnEntityFrameworkRepository)](https://nuget.org/packages/UnEntityFrameworkRepository)
## Installation

This package is available on NuGet. You can install it via the NuGet Package Manager Console:

```powershell
Install-Package EntityFrameworkRepo -Version 1.0.0
```

Getting Started
---------------

### Setting up your DbContext

First, ensure your project has a `DbContext`. Here's a simple example:


```csharp

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {}

    public DbSet<YourEntity> YourEntities { get; set; }
}

```

### Implementing IRepository

`YourPackageName` comes with a generic `IRepository<T>` interface. You don't need to implement this; it's ready to use.

### Configuring Dependency Injection

In your `Startup.cs` or wherever you configure services, add your `DbContext` and repositories to the service collection:


```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
}

```

### Using IRepository in your application

Inject `IRepository<T>` into your controllers or services to begin using it:


```csharp
public class YourService
{
    private readonly IRepository<YourEntity> _repository;

    public YourService(IRepository<YourEntity> repository)
    {
        _repository = repository;
    }

    public async Task<List<YourEntity>> GetAllEntitiesAsync()
    {
        return await _repository.GetAll().ToListAsync();
    }
}

```

Advanced Configuration
----------------------

### Abstracting DbContext with IDatabaseContext

To make your data access layer more flexible and testable, you can abstract your `DbContext` using the `IDatabaseContext` interface. This is especially useful for unit testing and when working with multiple database contexts.

#### Defining IDatabaseContext

Create an interface `IDatabaseContext` that your `DbContext` will implement:


```csharp

public interface IDatabaseContext
{
    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

```

#### Implementing IDatabaseContext in your DbContext

Make your `DbContext` implement `IDatabaseContext`:


```csharp

public class ApplicationDbContext : DbContext, IDatabaseContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {}

    // Implementation of DbSet<T> and SaveChangesAsync already provided by DbContext
    // Just ensure it implements IDatabaseContext
}

```

### Configuring the Repository with IDatabaseContext

When setting up your repository in the DI container, ensure it uses `IDatabaseContext`:


```csharp

public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<IDatabaseContext>(provider => provider.GetService<ApplicationDbContext>());
    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
}

```

This setup allows your repositories to interact with the database context through an abstraction, making your application more modular and easier to test.

Contributing
------------

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1.  Fork the Project
2.  Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3.  Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4.  Push to the Branch (`git push origin feature/AmazingFeature`)
5.  Open a Pull Request

License
-------

Distributed under the MIT License. See `LICENSE` for more information.
