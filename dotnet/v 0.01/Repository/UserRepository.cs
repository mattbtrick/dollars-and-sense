using Interfaces.IRepository;
using Models;
using Repository.Extensions;
using System.Data;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void DeleteById(long id)
        {
            string query = @"DELETE [User] WHERE UserId = @userId";
            _connection.ExecuteQuery(query, new Dictionary<string, object>
            {
                {"userId", id}
            });
        }

        public IEnumerable<User> GetAll()
        {
            string query = @"SELECT * FROM [User]";

            return _connection.ExecuteReaderAndMapResult<IEnumerable<User>>(query
                , new Dictionary<string, object>()
                , GetUserListFromReader) ?? new List<User>();
        }

        public User? GetById(long id)
        {
            return GetById(id, false);
        }

        public User? GetById(long id, bool populatePermissions)
        {
            string query = @"SELECT * FROM [User] WHERE UserId = @userId";
            var parameters = new Dictionary<string, object>
            {
                { "userId", id }
            };

            return _connection.ExecuteReaderAndMapResult<User>(query, parameters, GetUserFromReader);
        }

        public User? Save(User user)
        {
            var query = @"
                    MERGE [User] AS tgt
                    USING (SELECT @userId, @firstName, @lastName) AS src(UserId, FirstName, LastName)
                        ON (tgt.UserId = src.UserId)
                    WHEN MATCHED
                        THEN
                            UPDATE
                            SET FirstName = src.FirstName
			                , LastName = src.LastName
                    WHEN NOT MATCHED
                        THEN
                            INSERT (FirstName, LastName)
                            VALUES (src.FirstName, src.LastName)
                    OUTPUT inserted.*;";

            var parameters = new Dictionary<string, object>
            {
                { "userId", user.UserId },
                { "firstName", user.FirstName },
                { "lastName", user.LastName }
            };

            return _connection.ExecuteReaderAndMapResult<User>(query, parameters, GetUserFromReader);
        }

        private IEnumerable<User> GetUserListFromReader(IDataReader reader)
        {
            var users = new List<User>();
            while (reader.Read())
            {
                users.Add(MapUser(reader));
            }
            return users;
        }

        private User MapUser(IDataReader reader)
        {
            return new User
            {
                UserId = reader.GetInt64(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2)
            };
        }

        private User? GetUserFromReader(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }

            if (reader.Read())
            {
                return MapUser(reader);
            }
            return null;
        }

    }
}
