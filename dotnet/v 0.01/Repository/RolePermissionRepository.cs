using Interfaces.IRepository;
using Models;
using Repository.Extensions;
using System.Data;

namespace Repository
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly IDbConnection _connection;

        public RolePermissionRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void DeleteByRoleId(long roleId)
        {
            string query = @"DELETE [RolePermission] WHERE RoleId = @roleId";
            _connection.ExecuteQuery(query, new Dictionary<string, object>
            {
                {"roleId", roleId}
            });
        }

        public void DeleteByPermissionId(long permissionId)
        {
            string query = @"DELETE [RolePermission] WHERE [PermissionId] = @permissionId";
            _connection.ExecuteQuery(query, new Dictionary<string, object>
            {
                {"@permissionId", permissionId}
            });
        }

        public void Delete(RolePermission rolePermission)
        {
            string query = @"DELETE [RolePermission] WHERE PermissionId = @permissionId AND RoleId = @roleId";
            _connection.ExecuteQuery(query, new Dictionary<string, object>
            {
                {"@permissionId", rolePermission.PermissionId},
                {"@roleId", rolePermission.RoleId}
            });
        }

        public IEnumerable<RolePermission> GetByRoleId(long roleId)
        {
            string query = @"SELECT RoleId, PermissionId FROM [RolePermission] WHERE RoleId = @roleId";

            return _connection.ExecuteReaderAndMapResult<IEnumerable<RolePermission>>(query
                , new Dictionary<string, object>
                {
                    {"@roleId", roleId}
                }
                , GetRolePermissionListFromReader) ?? new List<RolePermission>();
        }

        public IEnumerable<RolePermission> GetByPermissionId(long permissionId)
        {
            string query = @"SELECT RoleId, PermissionId FROM [RolePermission] WHERE PermissionId = @permissionId";

            return _connection.ExecuteReaderAndMapResult<IEnumerable<RolePermission>>(query
                , new Dictionary<string, object>
                {
                    {"@permissionId", permissionId}
                }
                , GetRolePermissionListFromReader) ?? new List<RolePermission>();
        }

        public RolePermission? Get(long roleId, long permissionId)
        {
            string query = @"SELECT RoleId, PermissionId FROM [RolePermission] WHERE RoleId = @roleId AND PermissionId = @permissionId";

            return _connection.ExecuteReaderAndMapResult<RolePermission>(query
                , new Dictionary<string, object>
                {
                    {"@roleId", roleId}
                    ,{"@permissionId", permissionId}
                }
                , GetRolePermissionFromReader);
        }

        public RolePermission? Save(RolePermission jobAssignment)
        {
            var query = @"
                    IF NOT EXISTS(SELECT 1 FROM [RolePermission] WHERE RoleId = @roleId AND PermissionId = @permissionId)
                        INSERT INTO [RolePermission] (RoleId, PermissionId) VALUES (@roleId, @permissionId)

                    SELECT RoleId, PermissionId 
                    FROM [RolePermission] 
                    WHERE RoleId = @roleId AND PermissionId = @permissionId
                    ";

            var parameters = new Dictionary<string, object>
            {
                { "@roleId", jobAssignment.RoleId },
                { "@permissionId", jobAssignment.PermissionId }
            };

            return _connection.ExecuteReaderAndMapResult<RolePermission>(query, parameters, GetRolePermissionFromReader);
        }

        private IEnumerable<RolePermission> GetRolePermissionListFromReader(IDataReader reader)
        {
            var rolePermissions = new List<RolePermission>();
            while (reader.Read())
            {
                rolePermissions.Add(MapRolePermission(reader));
            }
            return rolePermissions;
        }

        private RolePermission MapRolePermission(IDataReader reader)
        {
            return new RolePermission
            {
                RoleId = reader.GetInt64(0),
                PermissionId = reader.GetInt64(1)
            };
        }

        private RolePermission? GetRolePermissionFromReader(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }

            if (reader.Read())
            {
                return MapRolePermission(reader);
            }
            return null;
        }
    }
}
