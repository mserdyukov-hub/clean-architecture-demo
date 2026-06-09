using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data;

///
/// Design-Time фабрика для EF Core.
///
/// EF Core использует этот класс во время выполнения команд:
///
/// dotnet ef migrations add
/// dotnet ef database update
/// dotnet ef migrations remove
///
/// В этот момент ASP.NET Core приложение ещё не запущено,
/// Dependency Injection контейнер не построен,
/// а значит EF не может получить зависимости через обычный механизм DI.
///
/// После добавления IMediator в конструктор DbContext:
///
/// CaDemoDbContext(
/// DbContextOptions options,
/// IMediator mediator)
///
/// EF больше не способен самостоятельно создать экземпляр контекста
/// во время работы с миграциями.
///
/// Поэтому мы явно показываем EF:
///
/// "Вот как нужно создавать CaDemoDbContext во время миграций".
///
/// Во время выполнения приложения этот класс НЕ используется.
/// Он нужен только для Design-Time сценариев EF Core.
///
public class CaDemoDbContextFactory : IDesignTimeDbContextFactory<CaDemoDbContext>
{

    public CaDemoDbContext CreateDbContext(string[] args)
    {
        // Читаем конфигурацию из WebApi проекта.
        //
        // Нам нужен тот же ConnectionString,
        // который используется приложением во время работы.
        var configuration = BuildConfiguration();

        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found.");

        var optionsBuilder =
            new DbContextOptionsBuilder<CaDemoDbContext>();

        optionsBuilder.UseNpgsql(connectionString);

        // Создаём DbContext вручную.
        // Вместо настоящего IMediator передаём заглушку.
        // Во время генерации миграций EF никогда не вызывает SaveChangesAsync(),
        // поэтому публикация Domain Events не происходит.
        // Следовательно полноценный MediatR здесь не нужен.
        return new CaDemoDbContext(
            optionsBuilder.Options,
            new NoOpMediator());
    }


    private static IConfiguration BuildConfiguration()
    {
        // Текущая директория при выполнении команд EF
        // обычно указывает на проект Infrastructure.
        // appsettings.json находится в WebApi,
        // поэтому переходим в соседний проект.
        var apiProjectPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "..",
            "WebApi");

        return new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json")
            .AddJsonFile(
                "appsettings.Development.json",
                optional: true)
            .AddEnvironmentVariables()
            .Build();
    }
}
