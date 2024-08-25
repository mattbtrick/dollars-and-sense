using Interfaces.IRepository;
using Models;
using Repository.Extensions;
using System.Data;

namespace Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IDbConnection _connection;

        public RoleRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void DeleteById(long id)
        {
            string query = @"DELETE [Role] WHERE RoleId = @roleId";
            _connection.ExecuteQuery(query, new Dictionary<string, object>
            {
                {"@roleId", id}
            });
        }

        public IEnumerable<Role> GetAll()
        {
            string query = @"SELECT * FROM [Role]";

            return _connection.ExecuteReaderAndMapResult<IEnumerable<Role>>(query
                , new Dictionary<string, object>()
                , GetRoleListFromReader) ?? new List<Role>();
        }

        public Role? GetById(long id)
        {
            return GetById(id, false);
        }

        public Role? GetById(long id, bool populationChildern)
        {
            string query = @"SELECT * FROM [Role] WHERE RoleId = @roleId";
            var parameters = new Dictionary<string, object>
            {
                { "@roleId", id }
            };

            return _connection.ExecuteReaderAndMapResult<Role>(query, parameters, GetRoleFromReader);
        }

        public Role? Save(Role role)
        {
            var query = @"
                    MERGE [Role] AS tgt
                    USING (SELECT @roleId, @name) AS src(RoleId, Name)
                        ON (tgt.RoleId = src.RoleId)
                    WHEN MATCHED
                        THEN
                            UPDATE
                            SET Name = src.Name
                    WHEN NOT MATCHED
                        THEN
                            INSERT (Name)
                            VALUES (src.Name)
                    OUTPUT inserted.*;";

            var parameters = new Dictionary<string, object>
            {
                { "@roleId", role.RoleId },
                { "@name", role.Name }
            };

            return _connection.ExecuteReaderAndMapResult<Role>(query, parameters, GetRoleFromReader);
        }

        private IEnumerable<Role> GetRoleListFromReader(IDataReader reader)
        {
            var users = new List<Role>();
            while (reader.Read())
            {
                users.Add(MapRole(reader));
            }
            return users;
        }

        private Role MapRole(IDataReader reader)
        {
            return new Role
            {
                RoleId = reader.GetInt64(0),
                Name = reader.GetString(1)
            };
        }

        private Role? GetRoleFromReader(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }

            if (reader.Read())
            {
                return MapRole(reader);
            }
            return null;
        }

    }
}
