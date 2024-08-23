using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DollarsAndSenseIntegrationTest
{
    internal class TestData
    {
        public void AddUsers()
        {
            var userSql = @"INSERT INTO User (UserId, FirstName, LastName)
                VALUES 
                    (-1, 'Delete', 'Test User')
                   ,(-2, 'Update', 'Test User')";

            using var con = new SqlConnection("Data Source=.;Initial Catalog=DollarsAndSense;Trusted_Connection=true;TrustServerCertificate=true;");
            con.Open();
            using var command = con.CreateCommand();
            command.CommandText = userSql;
            command.ExecuteNonQuery();
        }
    }
}
