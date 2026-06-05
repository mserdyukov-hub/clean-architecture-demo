using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Repositories;

public interface IUserRepository
{
    /// <summary>
    /// Получить пользователя по ID без связанных данных
    /// </summary>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получить пользователи и его роли
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<User?> GetDetailsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получить пользователя по Email с ролями (для логина, JWT claims)
    /// </summary>
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получить всех пользователей
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получить пользователя по Username без связанных данных
    /// </summary>
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Проверить существование Email (для регистрации)
    /// </summary>
    Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Проверить существование Username (для регистрации)
    /// </summary>
    Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);
    
    void Add(User user);
    void Update(User user);
    void Remove(User user);
}