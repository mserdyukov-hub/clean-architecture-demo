namespace Application.Common.Interfaces;

/// <summary>
/// Паттерн Unit of Work — гарантирует атомарность операций с БД.
/// Все изменения коммитятся одной транзакцией.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Сохранить все изменения в БД одной транзакцией
    /// </summary>
    /// <returns>Количество затронутых строк</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}