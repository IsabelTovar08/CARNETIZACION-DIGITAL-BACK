using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Classes.Base;
using Business.Interfases;
using Data.Classes.Specifics;
using Entity.DTOs.Auth;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Specifics;
using Entity.Models;

namespace Business.Interfaces.Security
{
    public interface IUserBusiness : IBaseBusiness<User, UserDtoRequest, UserDTO>
    {
        Task<List<string>> GetUserRolesById(int userId);
        Task<UserMeDto?> GetByIdForMe(int userId, List<string> roles);
        Task<User?> GetUserByIdAsync(int userId);

        Task<UserProfileDto?> GetProfileAsync(int userId);
        Task<UserProfileDto?> UpdateProfileAsync(int userId, UserProfileRequestDto dto);

        /// <summary>
        /// Devuelve los usuarios que pertenecen al rol indicado (por nombre).
        /// </summary>
        Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(string roleName);

    }
}
