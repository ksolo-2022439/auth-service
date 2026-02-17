using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Entities;

public class Role
{
    [Key]
    [MaxLength(16)]
    public string Id { get; set; } = string.Empty; // Inicializado para evitar CS8618

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty; // Inicializado

    [Required]
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty; // Inicializado

    // Relación con UserRoles
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>(); // Inicializado
}

// Example of predefined

/*
Roles
+--------------+--------------+------------------+
| Id           | Name         | Description      |
+--------------+--------------+------------------+
| ADMIN        | Admin        | Administrador    |
| USER         | User         | Usuario normal   |
| GUEST        | Guest        | Invitado         |
+--------------+--------------+------------------+
*/
 