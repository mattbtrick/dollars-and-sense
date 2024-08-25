using Interfaces.IRepository;
using Models;
using Repository.Extensions;
using System.Data;

namespace Repository
{
    public class UserExpensePaymentLogRepository : IUserExpensePaymentLogRepository
    {
        private readonly IDbConnection _connection;

        public UserExpensePaymentLogRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void DeleteById(long userExpensePaymentLogId)
        {
            string query = @"DELETE [UserExpensePaymentLog] WHERE UserExpensePaymentLogId = @userExpensePaymentLogId";
            _connection.ExecuteQuery(query, new Dictionary<string, object>
            {
                {"userExpensePaymentLogId", userExpensePaymentLogId}
            });
        }

        public IEnumerable<UserExpensePaymentLog> GetAll()
        {
            string query = @"SELECT JobId, UserId, Wage, DateCompleted, DatePaid, PaidByUserId
                                FROM [UserExpensePaymentLog]";

            return _connection.ExecuteReaderAndMapResult<IEnumerable<UserExpensePaymentLog>>(query
                , new Dictionary<string, object>()
                , GetUserExpensePaymentLogListFromReader) ?? new List<UserExpensePaymentLog>();
        }

        public UserExpensePaymentLog? GetById(long id)
        {
            string query = @"SELECT JobId, UserId, Wage, DateCompleted, DatePaid, PaidByUserId
                FROM [UserExpensePaymentLog] 
                WHERE JobId = @jobId";
            var parameters = new Dictionary<string, object>
            {
                { "jobId", id }
            };

            return _connection.ExecuteReaderAndMapResult<UserExpensePaymentLog>(query, parameters, GetUserExpensePaymentLogFromReader);
        }

        public UserExpensePaymentLog? Save(UserExpensePaymentLog userExpensePayment)
        {
            var query = @"
                    IF NOT EXISTS(SELECT 1 FROM [UserExpensePaymentLog] WHERE JobId = @jobId AND UserId = @userId)
                        INSERT INTO [UserExpensePaymentLog] (JobId, UserId, Wage, DateCompleted, DatePaid, PaidByUserId) 
                        VALUES (@jobId, @userId,  @wage, @dateCompleted, @datePaid, @paidByUserId)
                    ELSE
                        UPDATE [UserExpensePaymentLog]
                        SET 
                        WHERE JobId = @jobId AND UserId = @userId

                    SELECT JobId, UserId,  Wage, DateCompleted, DatePaid, PaidByUserId 
                    FROM [UserExpensePaymentLog] 
                    WHERE JobId = @jobId AND UserId = @userId
                    ";

            var parameters = new Dictionary<string, object>
            {
                { "jobId", userExpensePayment.ExpenseId},
                { "userId", userExpensePayment.UserId }
            };

            return _connection.ExecuteReaderAndMapResult<UserExpensePaymentLog>(query, parameters, GetUserExpensePaymentLogFromReader);
        }

        private IEnumerable<UserExpensePaymentLog> GetUserExpensePaymentLogListFromReader(IDataReader reader)
        {
            var jobAssignments = new List<UserExpensePaymentLog>();
            while (reader.Read())
            {
                jobAssignments.Add(MapUserExpensePaymentLog(reader));
            }
            return jobAssignments;
        }

        private UserExpensePaymentLog MapUserExpensePaymentLog(IDataReader reader)
        {
            return new UserExpensePaymentLog
            {
                ExpenseId = reader.GetInt64(0),
                UserId = reader.GetInt64(1)
            };
        }

        private UserExpensePaymentLog? GetUserExpensePaymentLogFromReader(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }

            if (reader.Read())
            {
                return MapUserExpensePaymentLog(reader);
            }
            return null;
        }

    }
}
