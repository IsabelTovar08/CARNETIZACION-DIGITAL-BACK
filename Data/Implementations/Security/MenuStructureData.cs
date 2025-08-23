using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfaces.Security;
using Entity.Context;
using Entity.Models;
using Entity.Models.ModelSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Security
{
    // Data access for MenuStructure
    public class MenuStructureData : BaseData<MenuStructure>, IMenuStructureData
    {
        public MenuStructureData(ApplicationDbContext context, ILogger<MenuStructure> logger)
            : base(context, logger)
        {
        }

        /// <summary>
        /// Returns the full menu tree (all roots with children) including Form/Module.
        /// </summary>
        public override async Task<IEnumerable<MenuStructure>> GetAllAsync()
        {
            try
            {
                // Cargar árbol de raíces con relaciones necesarias (hasta 3 niveles)
                var roots = await LoadRootTreeAsync();

                // Ordenar hijos por OrderIndex en todo el árbol
                foreach (var r in roots)
                    SortChildren(r);

                // Romper referencias Parent para evitar ciclos al serializar
                foreach (var r in roots)
                    RemoveParentReferences(r);

                return roots;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting full menu tree.");
                throw;
            }
        }

        /// <summary>
        /// Returns the menu tree filtered by user permissions (UserRoles -> RolFormPermission).
        /// Only allowed forms and their ancestor nodes are kept.
        /// </summary>
        public async Task<List<MenuStructure>> GetMenuTreeForUserAsync(int userId)
        {
            try
            {
                // 1) Obtener roles del usuario (ids únicos)
                var roleIds = await _context.Set<UserRoles>()            // ← tu entidad actual se llama UserRoles
                    .AsNoTracking()
                    .Where(ur => ur.UserId == userId /* && !ur.IsDeleted */)
                    .Select(ur => ur.RolId)
                    .Distinct()
                    .ToListAsync();

                // Usuario sin roles => no hay acceso
                if (roleIds.Count == 0)
                    return new List<MenuStructure>();

                var isSuperAdmin = await _context.Set<Role>()
                .AsNoTracking()
                .AnyAsync(r => roleIds.Contains(r.Id) && r.HasAllPermissions);
                if (isSuperAdmin)
                {
                    // # Devuelve el árbol completo, respetando orden y rompiendo referencias Parent para evitar ciclos
                    var rootsAll = await LoadRootTreeAsync();

                    // Ordenar hijos en todo el árbol
                    foreach (var r in rootsAll)
                        SortChildren(r);

                    // Romper referencias Parent para evitar ciclos al serializar
                    foreach (var r in rootsAll)
                        RemoveParentReferences(r);

                    return rootsAll;
                }


                // 2) Formularios permitidos para esos roles (cualquier PermissionId)
                var allowedFormIds = await _context.Set<RolFormPermission>()
                    .AsNoTracking()
                    .Where(rfp => roleIds.Contains(rfp.RolId) /* && !rfp.IsDeleted */)
                    .Select(rfp => rfp.FormId)
                    .Distinct()
                    .ToListAsync();

                // Sin formularios => menú vacío
                if (allowedFormIds.Count == 0)
                    return new List<MenuStructure>();

                var allowedSet = allowedFormIds.ToHashSet();

                // 3) Cargar árbol de raíces con relaciones necesarias
                var roots = await LoadRootTreeAsync();

                // 4) Podar: conservar solo formularios permitidos y sus ancestros
                var prunedRoots = new List<MenuStructure>();
                foreach (var root in roots)
                {
                    var kept = PruneNode(root, allowedSet);
                    if (kept != null)
                        prunedRoots.Add(kept);
                }

                // Ordenar hijos en el árbol resultante
                foreach (var r in prunedRoots)
                    SortChildren(r);

                // Romper referencias Parent para evitar ciclos al serializar
                foreach (var r in prunedRoots)
                    RemoveParentReferences(r);

                return prunedRoots;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting permission-filtered menu for user {UserId}.", userId);
                throw;
            }
        }

        // --- Helpers privados ---

        /// <summary>
        /// Loads root nodes and required relations up to 3 levels (adjust if needed).
        /// </summary>
        private async Task<List<MenuStructure>> LoadRootTreeAsync()
        {
            // Nota: AsSplitQuery evita explosión cartesiana con múltiples Includes.
            return await _context.Set<MenuStructure>()
                .AsNoTracking()
                .AsSplitQuery()
                .Where(x => x.ParentMenuId == null && !x.IsDeleted) // ← respeta soft delete si tu QueryFilter no lo tiene global
                .OrderBy(x => x.OrderIndex)

                // Nivel 0 (root)
                .Include(x => x.Form)
                .Include(x => x.Module)

                // Nivel 1
                .Include(x => x.Children).ThenInclude(c => c.Form)
                .Include(x => x.Children).ThenInclude(c => c.Module)

                // Nivel 2
                .Include(x => x.Children).ThenInclude(c => c.Children).ThenInclude(cc => cc.Form)
                .Include(x => x.Children).ThenInclude(c => c.Children).ThenInclude(cc => cc.Module)

                // Nivel 3 (duplicar el patrón si requiere más profundidad)
                .Include(x => x.Children).ThenInclude(c => c.Children)
                                         .ThenInclude(cc => cc.Children).ThenInclude(ccc => ccc.Form)
                .Include(x => x.Children).ThenInclude(c => c.Children)
                                         .ThenInclude(cc => cc.Children).ThenInclude(ccc => ccc.Module)

                .ToListAsync();
        }

        /// <summary>
        /// Prunes a node to keep only allowed forms and their ancestor path.
        /// Returns null if the node (and all descendants) are not allowed.
        /// </summary>
        private static MenuStructure? PruneNode(MenuStructure node, HashSet<int> allowedFormIds)
        {
            // Poda recursiva de hijos (post-orden)
            var filteredChildren = (node.Children ?? Enumerable.Empty<MenuStructure>())
                .Select(child => PruneNode(child, allowedFormIds))
                .Where(child => child != null)
                .Cast<MenuStructure>()
                .ToList();

            // ¿Este nodo representa un Form permitido?
            var isAllowedForm = node.FormId.HasValue && allowedFormIds.Contains(node.FormId.Value);

            // Conservar si es formulario permitido o si tiene hijos que calzan
            if (isAllowedForm || filteredChildren.Count > 0)
            {
                node.Children = filteredChildren;
                return node;
            }

            // Nodo hoja sin permisos => descartar
            return null;
        }

        /// <summary>
        /// Sort children by OrderIndex recursively.
        /// </summary>
        private static void SortChildren(MenuStructure node)
        {
            if (node.Children == null || node.Children.Count == 0) return;

            node.Children = node.Children.OrderBy(c => c.OrderIndex).ToList();
            foreach (var child in node.Children)
                SortChildren(child);
        }

        /// <summary>
        /// Remove Parent back-references recursively to avoid JSON cycles on serialization.
        /// </summary>
        private static void RemoveParentReferences(MenuStructure node)
        {
            // Romper referencia al padre
            node.Parent = null;

            if (node.Children == null || node.Children.Count == 0) return;

            foreach (var child in node.Children)
            {
                child.Parent = null;            // asegurar que cada hijo también pierda back-reference
                RemoveParentReferences(child);  // recursión
            }
        }
    }
}
