using System.Data;

namespace Repository.Extensions
{
    internal static class IDBCommandExtensions
    {
        public static void AddParameter(this IDbCommand command, string name, object value)
        {
            var idParam = command.CreateParameter();
            idParam.Value = value;
            idParam.ParameterName = name;
            command.Parameters.Add(idParam);
        }
    }
}
