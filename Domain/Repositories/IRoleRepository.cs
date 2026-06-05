using Domain.Entities;

namespace Domain.Repositories;

public interface IRoleRepository
{
    /// <summary>
    /// Получить все роли
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<Role>> GetAllAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получить роль по ID без связанных данных
    /// </summary>
    Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получить роль по Id с разрешениями (для проверки прав доступа)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Role?> GetDetailsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получить роль по имени с разрешениями (для проверки прав доступа)
    /// </summary>
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получить все роли пользователя (для JWT claims, проверки прав)
    /// </summary>
    Task<List<Role>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Проверить существование роли по имени (перед созданием)
    /// </summary>
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    
    void Add(Role role);
    void Remove(Role role);
}