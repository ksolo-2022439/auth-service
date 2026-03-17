using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Constants;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Application.Services;

public class UserManagementService(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    ICloudinaryService cloudinaryService,
    ILogger<UserManagementService> logger) : IUserManagementService
{
    public async Task<UserResponseDto> UpdateUserRoleAsync(string userId, string roleName)
    {
        // 1. Verificar si el usuario existe
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            logger.LogWarning("Intento de actualizar rol de usuario inexistente: {UserId}", userId);
            throw new KeyNotFoundException($"Usuario con ID {userId} no encontrado.");
        }

        // 2. Verificar si el rol existe en la base de datos
        var role = await roleRepository.GetByNameAsync(roleName);
        if (role == null)
        {
            logger.LogWarning("Intento de asignar rol inexistente: {RoleName}", roleName);
            throw new KeyNotFoundException($"Rol '{roleName}' no existe en el sistema.");
        }

        // 3. Actualizar el rol
        await userRepository.UpdateUserRoleAsync(userId, role.Id);
        logger.LogInformation("Rol de usuario {UserId} actualizado exitosamente a {RoleName}", userId, roleName);

        // 4. Recargar el usuario con su nuevo rol y mapearlo
        var updatedUser = await userRepository.GetByIdAsync(userId);
        return MapToUserResponseDto(updatedUser!);
    }

    public async Task<IReadOnlyList<string>> GetUserRolesAsync(string userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"Usuario con ID {userId} no encontrado.");
        }

        return await roleRepository.GetUserRoleNamesAsync(userId);
    }

    public async Task<IReadOnlyList<UserResponseDto>> GetUsersByRoleAsync(string roleName)
    {
        var role = await roleRepository.GetByNameAsync(roleName);
        if (role == null)
        {
            throw new KeyNotFoundException($"Rol '{roleName}' no existe en el sistema.");
        }

        var users = await roleRepository.GetUsersByRoleAsync(roleName);
        return users.Select(MapToUserResponseDto).ToList();
    }

    #region Private Mapper Methods

    private UserResponseDto MapToUserResponseDto(User user)
    {
        var userRole = user.UserRoles?.FirstOrDefault()?.Role?.Name ?? RoleConstants.USER_ROLE;
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Username = user.Username,
            Email = user.Email,
            ProfilePicture = cloudinaryService.GetFullImageUrl(user.Profile?.ProfilePicture ?? string.Empty),
            Phone = user.Profile?.Phone ?? string.Empty,
            Role = userRole,
            Status = user.Status,
            IsEmailVerified = user.UserEmail?.EmailVerified ?? false,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    #endregion
}