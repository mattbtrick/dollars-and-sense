using Interfaces.IRepository;
using Models;
using Repository.Extensions;
using System.Data;

namespace Repository
{
    public class JobRuleRepository : IJobRuleRepository
    {
        private readonly IDbConnection _connection;

        public JobRuleRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void DeleteByJobId(long jobId)
        {
            string query = @"DELETE [JobRule] WHERE JobId = @jobId";
            _connection.ExecuteQuery(query, new Dictionary<string, object>
            {
                {"jobId", jobId}
            });
        }

        public void DeleteByScheduleId(long scheduleId)
        {
            string query = @"DELETE [JobRule] WHERE [CompletionScheduleId] = @scheduleId";
            _connection.ExecuteQuery(query, new Dictionary<string, object>
            {
                {"@scheduleId", scheduleId}
            });
        }

        public void Delete(JobRule jobRule)
        {
            string query = @"DELETE [JobRule] WHERE CompletionScheduleId = @scheduleId AND JobId = @jobId";
            _connection.ExecuteQuery(query, new Dictionary<string, object>
            {
                {"@scheduleId", jobRule.CompletionScheduleId},
                {"@jobId", jobRule.JobId}
            });
        }

        public IEnumerable<JobRule> GetByJobId(long jobId)
        {
            string query = @"SELECT JobId, CompletionScheduleId FROM [JobRule] WHERE JobId = @jobId";

            return _connection.ExecuteReaderAndMapResult<IEnumerable<JobRule>>(query
                , new Dictionary<string, object>
                {
                    {"@jobId", jobId}
                }
                , GetJobRuleListFromReader) ?? new List<JobRule>();
        }

        public IEnumerable<JobRule> GetByScheduleId(long scheduleId)
        {
            string query = @"SELECT JobId, CompletionScheduleId FROM [JobRule] WHERE CompletionScheduleId = @scheduleId";

            return _connection.ExecuteReaderAndMapResult<IEnumerable<JobRule>>(query
                , new Dictionary<string, object>
                {
                    {"@scheduleId", scheduleId}
                }
                , GetJobRuleListFromReader) ?? new List<JobRule>();
        }

        public JobRule? Get(long jobId, long scheduleId)
        {
            string query = @"SELECT JobId, CompletionScheduleId FROM [JobRule] WHERE JobId = @jobId AND CompletionScheduleId = @scheduleId";

            return _connection.ExecuteReaderAndMapResult<JobRule>(query
                , new Dictionary<string, object>
                {
                    {"@jobId", jobId},
                    {"@scheduleId", scheduleId}
                }
                , GetJobRuleFromReader);
        }

        public JobRule? Save(JobRule jobAssignment)
        {
            var query = @"
                    IF NOT EXISTS(SELECT 1 FROM [JobRule] WHERE JobId = @jobId AND CompletionScheduleId = @scheduleId)
                        INSERT INTO [JobRule] (JobId, CompletionScheduleId) VALUES (@jobId, @scheduleId)

                    SELECT JobId, CompletionScheduleId 
                    FROM [JobRule] 
                    WHERE JobId = @jobId AND CompletionScheduleId = @scheduleId
                    ";

            var parameters = new Dictionary<string, object>
            {
                { "@jobId", jobAssignment.JobId },
                { "@scheduleId", jobAssignment.CompletionScheduleId }
            };

            return _connection.ExecuteReaderAndMapResult<JobRule>(query, parameters, GetJobRuleFromReader);
        }

        private IEnumerable<JobRule> GetJobRuleListFromReader(IDataReader reader)
        {
            var jobAssignments = new List<JobRule>();
            while (reader.Read())
            {
                jobAssignments.Add(MapJobRule(reader));
            }
            return jobAssignments;
        }

        private JobRule MapJobRule(IDataReader reader)
        {
            return new JobRule
            {
                JobId = reader.GetInt64(0),
                CompletionScheduleId = reader.GetInt64(1)
            };
        }

        private JobRule? GetJobRuleFromReader(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }

            if (reader.Read())
            {
                return MapJobRule(reader);
            }
            return null;
        }
    }
}
