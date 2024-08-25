using Interfaces.IRepository;
using Models;
using Repository.Extensions;
using System.Data;

namespace Repository
{
    public class JobAssignmentRepository : IJobAssignmentRepository
    {
        private readonly IDbConnection _connection;

        public JobAssignmentRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void DeleteByJobId(long jobId)
        {
            string query = @"DELETE [JobAssignment] WHERE JobId = @jobId";
            _connection.ExecuteQuery(query, new Dictionary<string, object>
            {
                {"jobId", jobId}
            });
        }

        public void DeleteByUserId(long userId)
        {
            string query = @"DELETE [JobAssignment] WHERE UserId = @userId";
            _connection.ExecuteQuery(query, new Dictionary<string, object>
            {
                {"userId", userId}
            });
        }

        public void Delete(JobAssignment jobAssignment)
        {
            string query = @"DELETE [JobAssignment] WHERE UserId = @userId AND JobId = @jobId";
            _connection.ExecuteQuery(query, new Dictionary<string, object>
            {
                {"@userId", jobAssignment.UserId},
                {"@jobId", jobAssignment.JobId}
            });
        }

        public IEnumerable<JobAssignment> GetByJobId(long jobId)
        {
            string query = @"SELECT JobId, UserId FROM [JobAssignment] WHERE JobId = @jobId";

            return _connection.ExecuteReaderAndMapResult<IEnumerable<JobAssignment>>(query
                , new Dictionary<string, object>
                {
                    {"@jobId", jobId}
                }
                , GetJobAssignmentListFromReader) ?? new List<JobAssignment>();
        }

        public IEnumerable<JobAssignment> GetByUserId(long userId)
        {
            string query = @"SELECT JobId, UserId FROM [JobAssignment] WHERE UserId = @userId";

            return _connection.ExecuteReaderAndMapResult<IEnumerable<JobAssignment>>(query
                , new Dictionary<string, object>
                {
                    {"@userId", userId}
                }
                , GetJobAssignmentListFromReader) ?? new List<JobAssignment>();
        }

        public JobAssignment? Get(long jobId, long userId)
        {
            string query = @"SELECT JobId, UserId FROM [JobAssignment] WHERE UserId = @userId AND JobId = @jobId";

            return _connection.ExecuteReaderAndMapResult<JobAssignment>(query
                , new Dictionary<string, object>
                {
                    {"@userId", userId},
                    {"@jobId", jobId}
                }
                , GetJobAssignmentFromReader);
        }

        public JobAssignment? Save(JobAssignment jobAssignment)
        {
            var query = @"
                    IF NOT EXISTS(SELECT 1 FROM [JobAssignment] WHERE JobId = @jobId AND UserId = @userId)
                        INSERT INTO [JobAssignment] (JobId, UserId) VALUES (@jobId, @userId)

                    SELECT JobId, UserId FROM [JobAssignment] WHERE JobId = @jobId AND UserId = @userId
                    ";

            var parameters = new Dictionary<string, object>
            {
                { "jobId", jobAssignment.JobId },
                { "userId", jobAssignment.UserId }
            };

            return _connection.ExecuteReaderAndMapResult<JobAssignment>(query, parameters, GetJobAssignmentFromReader);
        }

        private IEnumerable<JobAssignment> GetJobAssignmentListFromReader(IDataReader reader)
        {
            var jobAssignments = new List<JobAssignment>();
            while (reader.Read())
            {
                jobAssignments.Add(MapJobAssignment(reader));
            }
            return jobAssignments;
        }

        private JobAssignment MapJobAssignment(IDataReader reader)
        {
            return new JobAssignment
            {
                JobId = reader.GetInt64(0),
                UserId = reader.GetInt64(1)
            };
        }

        private JobAssignment? GetJobAssignmentFromReader(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }

            if (reader.Read())
            {
                return MapJobAssignment(reader);
            }
            return null;
        }
    }
}
