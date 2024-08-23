using Microsoft.Data.SqlClient;

namespace DollarAndSenseIntegrationTest
{
    [TestClass]
    public sealed class TestData
    {
        public static string ConnectionString = "Data Source=.;Initial Catalog=DollarsAndSense;Trusted_Connection=true;TrustServerCertificate=true;";
        public static SqlConnection GetConnection() => new SqlConnection("Data Source=.;Initial Catalog=DollarsAndSense;Trusted_Connection=true;TrustServerCertificate=true;");
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            var userSql = @"SET IDENTITY_INSERT [User] ON
                INSERT INTO [User] (UserId, FirstName, LastName)
                VALUES 
                    (-1, 'Delete', 'Test User')
                   ,(-2, 'Update', 'Test User')
                   ,(-3, 'Role', 'Test User')
                   ,(-4, 'Expense', 'Test User')
                   ,(-5, 'Job', 'Test User')
                SET IDENTITY_INSERT [User] OFF";

            var roleSql = @"SET IDENTITY_INSERT [Role] ON
                INSERT INTO [Role] (RoleId, Role)
                VALUES 
                    (-1, 'Test Delete')
                   ,(-2, 'Test Update')
                   ,(-3, 'Test Read')
                SET IDENTITY_INSERT [Role] OFF";

            var userRoleSql = @"INSERT INTO UserRole (RoleId, UserId)
                VALUES 
                    (-1, -3)
                   ,(-2, -3)";

            var permsionSql = @"SET IDENTITY_INSERT [Permission] ON
                INSERT INTO [Permission] (PermissionId, Permission)
                VALUES 
                    (-1, 'Test Delete User')
                   ,(-2, 'Test Update User')
                   ,(-3, 'Test Read Users')
                SET IDENTITY_INSERT [Permission] OFF";

            var rolePermissionSql = @"INSERT INTO RolePermission (RoleId, PermissionId)
                VALUES 
                    (-1, -1)
                   ,(-2, -2)
                   ,(-3, -3)";

            var expenseSql = @"SET IDENTITY_INSERT [Expense] ON
                INSERT INTO Expense (ExpenseId, Expense, Description, Amount)
                VALUES 
                    (-1, 'Test expense 1', '$100 expense', 100)
                   ,(-2, 'Test expense 2', '$200 expense', 200)
                   ,(-3, 'Test expense 3', '$300 expense description', 300)
                    ,(-4, 'Test expense Delete', '$300 expense', 300)
                SET IDENTITY_INSERT [Expense] OFF";

            var jobSql = @"SET IDENTITY_INSERT [Job] ON
                INSERT INTO [Job] (JobId, Name, [Instructions], Wage)
                VALUES 
                    (-1, 'Test job 1', '$100 job', 100)
                   ,(-2, 'Test job 2', '$200 job', 200)
                   ,(-3, 'Test job 3', '$300 job instructions', 300)
                    ,(-4, 'Test job 4 Delete', '$400 job', 400)
                SET IDENTITY_INSERT [Job] OFF";

            var jobAssignmentSql = @"INSERT INTO JobAssignment (JobId, UserId)
                VALUES 
                    (-1, -5)
                   ,(-2, -5)";

            var jobRuleSql = @"INSERT INTO JobRule (JobId, CompletionScheduleId)
                VALUES 
                    (-1, 1)
                   ,(-2, 2)";

            var userExpenseSql = @"INSERT INTO UserExpense (UserId, ExpenseId, PaymentScheduleId)
                VALUES 
                    (-4, -1, 2)
                   ,(-4, -2, 1)";

            var userExpensePaymentLogSql = @"SET IDENTITY_INSERT [UserExpensePaymentLog] ON
                INSERT INTO UserExpensePaymentLog (UserExpensePaymentLogId, UserId, ExpenseId, AmountPaid, DatePaid)
                VALUES 
                    (-1, -4, -1, 10, '4/1/2024')
                   ,(-2, -4, -1, 10, '5/1/2024')
                SET IDENTITY_INSERT [UserExpensePaymentLog] OFF";

            var userJobCompletionLogSql = @"SET IDENTITY_INSERT [UserJobCompletionLog] ON
                INSERT INTO UserJobCompletionLog (UserJobCompletionLogId, UserId, JobId, Wage, DateCompleted, DatePaid, PaidByUserId)
                VALUES 
                    (-1, -5, -1, 10, '4/1/2024', '4/2/2024', -4)
                   ,(-2, -5, -1, 10, '5/1/2024', '5/2/2024', -4)
                SET IDENTITY_INSERT [UserJobCompletionLog] OFF";

            using var con = GetConnection();
            con.Open();
            using var command = con.CreateCommand();
            command.CommandText = userSql;
            command.ExecuteNonQuery();

            command.CommandText = roleSql;
            command.ExecuteNonQuery();

            command.CommandText = permsionSql;
            command.ExecuteNonQuery();

            command.CommandText = expenseSql;
            command.ExecuteNonQuery();

            command.CommandText = jobSql;
            command.ExecuteNonQuery();

            command.CommandText = userRoleSql;
            command.ExecuteNonQuery();

            command.CommandText = rolePermissionSql;
            command.ExecuteNonQuery();

            command.CommandText = jobAssignmentSql;
            command.ExecuteNonQuery();

            command.CommandText = jobRuleSql;
            command.ExecuteNonQuery();

            command.CommandText = userExpenseSql;
            command.ExecuteNonQuery();

            command.CommandText = userExpensePaymentLogSql;
            command.ExecuteNonQuery();

            command.CommandText = userJobCompletionLogSql;
            command.ExecuteNonQuery();
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
            
            var jobAssignmentSql = "DELETE JobAssignment WHERE UserId < 0";
            var userExpenseSql = "DELETE UserExpense WHERE UserId < 0";
            var userExpensePaymentLogSql = "DELETE UserExpensePaymentLog WHERE UserId < 0";
            var userJobCompletionLogSql = "DELETE UserJobCompletionLog WHERE UserId < 0";
            var userRoleSql = "DELETE UserRole WHERE UserId < 0";
            var rolePermissionSql = "DELETE RolePermission WHERE RoleId < 0";
            var jobRuleSql = "DELETE JobRule WHERE JobId < 0";

            var userSql = "DELETE [User] WHERE UserId < 0";
            var permissionSql = "DELETE [Permission] WHERE PermissionId < 0";
            var roleSql = "DELETE [Role] WHERE RoleId < 0";
            var jobSql = "DELETE [Job] WHERE JobId < 0";
            var expenseSql = "DELETE Expense WHERE ExpenseId < 0";

            using var con = GetConnection();
            con.Open();
            using var command = con.CreateCommand();
            command.CommandText = jobAssignmentSql;
            command.ExecuteNonQuery();

            command.CommandText = userExpenseSql;
            command.ExecuteNonQuery();

            command.CommandText = userExpensePaymentLogSql;
            command.ExecuteNonQuery();

            command.CommandText = userJobCompletionLogSql;
            command.ExecuteNonQuery();

            command.CommandText = userRoleSql;
            command.ExecuteNonQuery();

            command.CommandText = jobRuleSql;
            command.ExecuteNonQuery();

            command.CommandText = rolePermissionSql;
            command.ExecuteNonQuery();

            command.CommandText = userSql;
            command.ExecuteNonQuery();

            command.CommandText = permissionSql;
            command.ExecuteNonQuery();

            command.CommandText = roleSql;
            command.ExecuteNonQuery();

            command.CommandText = jobSql;
            command.ExecuteNonQuery();

            command.CommandText = expenseSql;
            command.ExecuteNonQuery();
        }
    }
}
