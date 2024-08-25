using Interfaces.IRepository;
using Models;
using Repository.Extensions;
using System.Data;

namespace Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IDbConnection _connection;

        public PermissionRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void DeleteById(long id)
        {
            string query = @"DELETE [Permission] WHERE PermissionId = @permissionId";
            _connection.ExecuteQuery(query, new Dictionary<string, object>
            {
                {"@permissionId", id}
            });
        }

        public IEnumerable<Permission> GetAll()
        {
            string query = @"SELECT * FROM [Permission]";

            return _connection.ExecuteReaderAndMapResult<IEnumerable<Permission>>(query
                , new Dictionary<string, object>()
                , GetPermissionListFromReader) ?? new List<Permission>();
        }

        public Permission? GetById(long id)
        {
            return GetById(id, false);
        }

        public Permission? GetById(long id, bool populationChildern)
        {
            string query = @"SELECT * FROM [Permission] WHERE PermissionId = @permissionId";
            var parameters = new Dictionary<string, object>
            {
                { "@permissionId", id }
            };

            return _connection.ExecuteReaderAndMapResult<Permission>(query, parameters, GetPermissionFromReader);
        }

        public Permission? Save(Permission permission)
        {
            var query = @"
                    MERGE [Permission] AS tgt
                    USING (SELECT @permissionId, @name) AS src(PermissionId, Permission)
                        ON (tgt.PermissionId = src.PermissionId)
                    WHEN MATCHED
                        THEN
                            UPDATE
                            SET Permission = src.Permission
                    WHEN NOT MATCHED
                        THEN
                            INSERT (Permission)
                            VALUES (src.Permission)
                    OUTPUT inserted.*;";

            var parameters = new Dictionary<string, object>
            {
                { "@permissionId", permission.PermissionId },
                { "@name", permission.Name }
            };

            return _connection.ExecuteReaderAndMapResult<Permission>(query, parameters, GetPermissionFromReader);
        }

        private IEnumerable<Permission> GetPermissionListFromReader(IDataReader reader)
        {
            var users = new List<Permission>();
            while (reader.Read())
            {
                users.Add(MapPermission(reader));
            }
            return users;
        }

        private Permission MapPermission(IDataReader reader)
        {
            return new Permission
            {
                PermissionId = reader.GetInt64(0),
                Name = reader.GetString(1)
            };
        }

        private Permission? GetPermissionFromReader(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }

            if (reader.Read())
            {
                return MapPermission(reader);
            }
            return null;
        }

    }
}
