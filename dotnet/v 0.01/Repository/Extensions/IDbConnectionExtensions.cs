using System.Data;

namespace Repository.Extensions
{
    internal static class IDbConnectionExtensions
    {
        public static T? ExecuteReaderAndMapResult<T>(this IDbConnection connection, string query, Dictionary<string, object> parameters, Func<IDataReader, T?> mapper)
        {
            try
            {
                connection.Open();

                using IDbCommand command = connection.CreateCommand();
                command.CommandText = query;

                foreach (var parameter in parameters)
                {
                    command.AddParameter(parameter.Key, parameter.Value);
                }

                using var reader = command.ExecuteReader();
                var userResult = mapper(reader);
                reader.Close();
                return userResult;

            }
            finally
            {
                connection.Close();
            }
            return default;
        }

        public static int ExecuteQuery(this IDbConnection connection, string query, Dictionary<string, object> parameters)
        {
            try
            {
                connection.Open();

                using IDbCommand command = connection.CreateCommand();
                command.CommandText = query;

                foreach (var parameter in parameters)
                {
                    command.AddParameter(parameter.Key, parameter.Value);
                }

                return command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
