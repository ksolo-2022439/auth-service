using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Domain.Entities;

public class UserProfile
{
    [Key]
    [MaxLength(16)]
    public string Id { get; set; } = string.Empty;

    [Required][MaxLength(16)]
    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = string.Empty;

    public string? ProfilePicture { get; set; } // Cambiado de ProfilePictureUrl a ProfilePicture
    
    [MaxLength(20)]
    public string? Phone { get; set; } // Propiedad agregada para coincidir con el DTO

    public string? Bio { get; set; }              
    public DateTime DateOfBirth { get; set; }

    public User User { get; set; } = null!; 
}