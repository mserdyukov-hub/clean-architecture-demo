using Domain.Entities;

namespace Domain.Repositories;

public interface IPermissionRepository
{
    /// <summary>
    /// Получить все разрешения
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<Permission>> GetAllAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Получить разрешение по ID без связанных данных
    /// </summary>
    Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получить разрешение по коду (для проверки прав из JWT claims)
    /// </summary>
    Task<Permission?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получить все разрешения роли (для отображения в админке)
    /// </summary>
    Task<List<Permission>> GetByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Проверить существование разрешения по коду (перед сидированием)
    /// </summary>
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
    
    void Add(Permission permission);
    void Remove(Permission permission);
}