using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces;

public interface IUserRepository
{
    // MÉTODOS DE CONSULTA
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByPasswordResetTokenAsync(string token);
    Task<User?> GetByEmailVerificationTokenAsync(string token);
    
    // MÉTODOS DE ESCRITURA
    Task<User?> CreateAsync(User user); 
    Task<User> UpdateAsync(User user); 
    Task<bool> DeleteAsync(string id); 

    // MÉTODOS DE VALIDACIÓN Y ROLES
    Task<bool> ExistsByEmailAsync(string email); 
    Task<bool> ExistsByUsernameAsync(string username);
    Task UpdateUserRoleAsync(string userId, string roleId);
}