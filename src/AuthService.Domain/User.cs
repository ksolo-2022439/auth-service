using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Entities;

public class User
{
    [Key]
    [MaxLength(16)]
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(25)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [MaxLength(25)]
    public string Surname { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress] // El valor de esta propiedad debe tener un formato de correo electronico
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(255)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MaxLength(25)]
    public string Status { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    
    //Relaciones de navegacion solo dentro del codigo
    //Esto no altera la base de datos
    public UserProfile Profile { get; set; } = null;
    public ICollection<UserRole> UserRoles { get; set; }
    public UserPasswordReset PasswordReset { get; set; }


}

