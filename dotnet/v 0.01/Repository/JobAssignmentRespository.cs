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

        public void DeleteById(long id)
        {
            string query = @"DELETE [JobAssignment] WHERE JobId = @jobId";
            _connection.ExecuteQuery(query, new Dictionary<string, object>
            {
                {"jobId", id}
            });
        }

        public IEnumerable<JobAssignment> GetAll()
        {
            string query = @"SELECT JobId, UserId FROM [JobAssignment]";

            return _connection.ExecuteReaderAndMapResult<IEnumerable<JobAssignment>>(query
                , new Dictionary<string, object>()
                , GetJobAssignmentListFromReader) ?? new List<JobAssignment>();
        }

        public JobAssignment? GetById(long id)
        {
            string query = @"SELECT JobId, UserId FROM [JobAssignment] WHERE JobId = @jobId";
            var parameters = new Dictionary<string, object>
            {
                { "jobId", id }
            };

            return _connection.ExecuteReaderAndMapResult<JobAssignment>(query, parameters, GetJobAssignmentFromReader);
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
