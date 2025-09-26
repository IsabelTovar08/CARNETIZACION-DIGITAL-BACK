using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Auth;
using Entity.DTOs.ModelSecurity.Response;
using Business.Interfaces.Security;
using Entity.Models;
using Data.Interfases.Security;

namespace Business.Services.Auth
{
    public class MenuService : IMenuService
    {
        private readonly IModuleData _moduleData;
        private readonly IUserRoleBusiness _userRolesBusiness;
        private readonly IRoleBusiness _roleBusiness;
        private readonly IRolFormPermissionBusiness _rolFormPermissionBusiness;

        public MenuService(
            IModuleData moduleData,
            IUserRoleBusiness userRolesBusiness,
            IRoleBusiness roleBusiness,
            IRolFormPermissionBusiness rolFormPermissionBusiness)
        {
            _moduleData = moduleData;
            _userRolesBusiness = userRolesBusiness;
            _roleBusiness = roleBusiness;
            _rolFormPermissionBusiness = rolFormPermissionBusiness;
        }

        public async Task<List<MenuStructureDto>> GetMenuAsync()
        {
            List<Module> modules = (List<Module>)await _moduleData.GetActiveAsync();
            return BuildMenu(modules);
        }

        public async Task<List<MenuStructureDto>> GetMenuForUserAsync(int userId)
        {
            List<string> roleIds = await _userRolesBusiness.GetRolesByUserIdAsync(userId);

            if (!roleIds.Any())
                return new List<MenuStructureDto>();

            bool isSuperAdmin = await _roleBusiness.UserIsSuperAdminAsync(roleIds);

            if (isSuperAdmin)
                return await GetMenuAsync();

            List<int> allowedFormIds = await _rolFormPermissionBusiness.GetAllowedFormIdsAsync(roleIds);

            if (!allowedFormIds.Any())
                return new List<MenuStructureDto>();

            List<Module> modules = await _moduleData.GetModulesWithFormsByAllowedFormsAsync(allowedFormIds);

            return BuildMenu(modules);
        }

        // ================= Helpers =================

        private List<MenuStructureDto> BuildMenu(List<Module> modules)
        {
            var result = new List<MenuStructureDto>();

            if (modules == null || !modules.Any())
                return result;

            foreach (var module in modules)
            {
                // ID artificial único para el grupo
                var groupId = module.Id * 1000;

                // 1) Group
                var groupNode = new MenuStructureDto
                {
                    Id = groupId,
                    ModuleId = module.Id,
                    Type = "group",
                    Title = module.Name,
                    Icon = module.Icon,
                    Url = null,
                    Classes = null,
                    ParentMenuId = null,
                    FormId = null,
                    Children = new List<MenuStructureDto>()
                };

                // ID artificial único para el collapse
                var collapseId = module.Id * 1000 + 1;

                // 2) Collapse
                var collapseNode = new MenuStructureDto
                {
                    Id = collapseId,
                    ModuleId = module.Id,
                    Type = "collapse",
                    Title = module.Name,
                    Icon = module.Icon,
                    Url = null,
                    Classes = null,
                    ParentMenuId = groupId,
                    FormId = null,
                    Children = module.Forms?
                        .Select(f => new MenuStructureDto
                        {
                            Id = f.Id,   // ID real del formulario (no se toca)
                            FormId = f.Id,
                            Type = "item",
                            Title = f.Name,
                            Icon = f.Icon,
                            Url = f.Url,
                            Classes = "nav-item",
                            ParentMenuId = collapseId,
                            ModuleId = null,
                            Children = new List<MenuStructureDto>()
                        })
                        .ToList() ?? new List<MenuStructureDto>()
                };

                // Collapse dentro del group
                groupNode.Children.Add(collapseNode);

                // Agregar al menú final
                result.Add(groupNode);
            }

            return result;
        }

    }
}
