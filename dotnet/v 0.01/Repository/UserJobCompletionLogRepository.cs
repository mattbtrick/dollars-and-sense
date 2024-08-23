using Interfaces.IRepository;
using Models;
using Repository.Extensions;
using System.Data;

namespace Repository
{
    public class UserJobCompletionLogRepository : IUserJobCompletionLogRepository
    {
        private readonly IDbConnection _connection;

        public UserJobCompletionLogRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void DeleteById(long id)
        {
            string query = @"DELETE [UserJobCompletionLog] WHERE JobId = @jobId";
            _connection.ExecuteQuery(query, new Dictionary<string, object>
            {
                {"jobId", id}
            });
        }

        public IEnumerable<UserJobCompletionLog> GetAll()
        {
            string query = @"SELECT JobId, UserId, Wage, DateCompleted, DatePaid, PaidByUserId
                                FROM [UserJobCompletionLog]";

            return _connection.ExecuteReaderAndMapResult<IEnumerable<UserJobCompletionLog>>(query
                , new Dictionary<string, object>()
                , GetUserJobCompletionLogListFromReader) ?? new List<UserJobCompletionLog>();
        }

        public UserJobCompletionLog? GetById(long id)
        {
            string query = @"SELECT JobId, UserId, Wage, DateCompleted, DatePaid, PaidByUserId
                FROM [UserJobCompletionLog] 
                WHERE JobId = @jobId";
            var parameters = new Dictionary<string, object>
            {
                { "jobId", id }
            };

            return _connection.ExecuteReaderAndMapResult<UserJobCompletionLog>(query, parameters, GetUserJobCompletionLogFromReader);
        }

        public UserJobCompletionLog? Save(UserJobCompletionLog jobAssignment)
        {
            var query = @"
                    IF NOT EXISTS(SELECT 1 FROM [UserJobCompletionLog] WHERE JobId = @jobId AND UserId = @userId)
                        INSERT INTO [UserJobCompletionLog] (JobId, UserId, Wage, DateCompleted, DatePaid, PaidByUserId) 
                        VALUES (@jobId, @userId,  @wage, @dateCompleted, @datePaid, @paidByUserId)
                    ELSE
                        UPDATE [UserJobCompletionLog]
                        SET 
                        WHERE JobId = @jobId AND UserId = @userId

                    SELECT JobId, UserId,  Wage, DateCompleted, DatePaid, PaidByUserId 
                    FROM [UserJobCompletionLog] 
                    WHERE JobId = @jobId AND UserId = @userId
                    ";

            var parameters = new Dictionary<string, object>
            {
                { "jobId", jobAssignment.JobId },
                { "userId", jobAssignment.UserId }
            };

            return _connection.ExecuteReaderAndMapResult<UserJobCompletionLog>(query, parameters, GetUserJobCompletionLogFromReader);
        }

        private IEnumerable<UserJobCompletionLog> GetUserJobCompletionLogListFromReader(IDataReader reader)
        {
            var jobAssignments = new List<UserJobCompletionLog>();
            while (reader.Read())
            {
                jobAssignments.Add(MapUserJobCompletionLog(reader));
            }
            return jobAssignments;
        }

        private UserJobCompletionLog MapUserJobCompletionLog(IDataReader reader)
        {
            return new UserJobCompletionLog
            {
                JobId = reader.GetInt64(0),
                UserId = reader.GetInt64(1)
            };
        }

        private UserJobCompletionLog? GetUserJobCompletionLogFromReader(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }

            if (reader.Read())
            {
                return MapUserJobCompletionLog(reader);
            }
            return null;
        }

    }
}
