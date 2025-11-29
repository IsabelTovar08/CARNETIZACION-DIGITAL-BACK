using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Specifics;
using Entity.Models;

namespace Data.Interfases.Security
{
    public interface IUserData : ICrudBase<User>
    {
        Task<User?> ValidateUserAsync(string email, string password);
        Task<User?> FindByEmail(string email);

        Task<List<string>> GetUserRolesByIdAsync(int userId);

        Task<User?> FindByLoginIdentifierAsync(string identifier);

        Task<string?> RequestPasswordResetAsync(string email);

        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);

        //Task<UserMeDto?> GetMeMinimalAsync(int userId);      
        //Task<UserMeDto?> GetMeWithProfileAsync(int userId);

        Task<User?> GetByIdForMeAsync(int userId);
        Task<User?> GetByIdWithPersonAsync(int userId);

        /// <summary>
        /// Devuelve todos los usuarios que tengan asignado el rol especificado.
        /// </summary>
        Task<IEnumerable<User>> GetUsersByRoleAsync(string roleName);

        Task<int?> GetUserIdByPersonIdAsync(int personId);

        /// <summary>
        /// Retorna el estado de autenticación en dos pasos del usuario (nullable).
        /// </summary>
        Task<bool?> IsTwoFactorEnabledAsync(int userId);

        /// <summary>
        /// Cambia el estado del 2FA para un usuario
        /// </summary>
        Task<bool> ToggleTwoFactorAsync(int userId);
=========
        Task<int?> GetUserIdByPersonIdAsync(int personId);
>>>>>>>>> Temporary merge branch 2


    }
}
