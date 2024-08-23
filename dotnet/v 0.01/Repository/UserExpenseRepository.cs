using Interfaces.IRepository;
using Models;
using Repository.Extensions;
using System.Data;

namespace Repository
{
    public class UserExpenseRepository : IUserExpenseRepository
    {
        private readonly IDbConnection _connection;

        public UserExpenseRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void DeleteById(long id)
        {
            string query = @"DELETE [UserExpense] WHERE UserId = @userId";
            _connection.ExecuteQuery(query, new Dictionary<string, object>
            {
                {"userId", id}
            });
        }

        public IEnumerable<UserExpense> GetAll()
        {
            string query = @"SELECT ExpenseId, UserId, PaymentScheduleId FROM [UserExpense]";

            return _connection.ExecuteReaderAndMapResult<IEnumerable<UserExpense>>(query
                , new Dictionary<string, object>()
                , GetUserExpenseListFromReader) ?? new List<UserExpense>();
        }

        public UserExpense? GetById(long id)
        {
            string query = @"SELECT ExpenseId, UserId, PaymentScheduleId FROM [UserExpense] WHERE UserId = @userId";
            var parameters = new Dictionary<string, object>
            {
                { "userId", id }
            };

            return _connection.ExecuteReaderAndMapResult<UserExpense>(query, parameters, GetUserExpenseFromReader);
        }

        public UserExpense? Save(UserExpense userExpense)
        {
            var query = @"
                    IF NOT EXISTS(SELECT 1 FROM [UserExpense] WHERE ExpenseId = @expenseId AND UserId = @userId)
                        INSERT INTO [UserExpense] (ExpenseId, UserId) VALUES (@ExpenseId, @userId, @paymentScheduleId)
                    ELSE
                        UPDATE [UserExpense]
                        SET PaymentScheduleId = @paymentScheduleId
                        WHERE ExpenseId = @expenseId AND UserId = @userId

                    SELECT ExpenseId, UserId, PaymentScheduleId FROM [UserExpense] WHERE ExpenseId = @expenseId AND UserId = @userId
                    ";

            var parameters = new Dictionary<string, object>
            {
                { "expenseId", userExpense.ExpenseId },
                { "userId", userExpense.UserId },
                { "paymentScheduleId", userExpense.PaymentScheduleId }
            };

            return _connection.ExecuteReaderAndMapResult<UserExpense>(query, parameters, GetUserExpenseFromReader);
        }

        private IEnumerable<UserExpense> GetUserExpenseListFromReader(IDataReader reader)
        {
            var jobAssignments = new List<UserExpense>();
            while (reader.Read())
            {
                jobAssignments.Add(MapUserExpense(reader));
            }
            return jobAssignments;
        }

        private UserExpense MapUserExpense(IDataReader reader)
        {
            return new UserExpense
            {
                ExpenseId = reader.GetInt64(0),
                UserId = reader.GetInt64(1),
                PaymentScheduleId = reader.GetInt64(2)
            };
        }

        private UserExpense? GetUserExpenseFromReader(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }

            if (reader.Read())
            {
                return MapUserExpense(reader);
            }
            return null;
        }

    }
}
