using Models;

namespace Interfaces.IRepository
{
    public interface IRolePermissionRepository
    {
        public IEnumerable<RolePermission> GetByRoleId(long roleId);
        public IEnumerable<RolePermission> GetByPermissionId(long permissionId);
        public void DeleteByRoleId(long roleId);
        public void DeleteByPermissionId(long permissionId);
        public void Delete(RolePermission rolePermission);
        public RolePermission Save(RolePermission rolePermission);
    }
}
