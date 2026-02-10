using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Entities;

    public class Role
    {
        [Key]
        [MaxLength(10)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        //Relaciones con UserRole
        public iCollection<UserRole> UserRole { get; set; }

    }

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