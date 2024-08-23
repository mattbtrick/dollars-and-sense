using Interfaces.IRepository;
using Models;
using Repository.Extensions;
using System.Data;

namespace Repository
{
    public class JobRepository : IJobRepository
    {
        private readonly IDbConnection _connection;

        public JobRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void DeleteById(long id)
        {
            string query = @"DELETE [Job] WHERE JobId = @jobId";
            _connection.ExecuteQuery(query, new Dictionary<string, object>
            {
                {"jobId", id}
            });
        }

        public IEnumerable<Job> GetAll()
        {
            string query = @"SELECT JobId, Name, Instructions, Wage FROM [Job]";

            return _connection.ExecuteReaderAndMapResult<IEnumerable<Job>>(query
                , new Dictionary<string, object>()
                , GetJobListFromReader) ?? new List<Job>();
        }

        public Job? GetById(long id)
        {
            return GetById(id, false);
        }

        public Job? GetById(long id, bool populatePermissions)
        {
            string query = @"SELECT JobId, Name, Instructions, Wage FROM [Job] WHERE JobId = @jobId";
            var parameters = new Dictionary<string, object>
            {
                { "jobId", id }
            };

            return _connection.ExecuteReaderAndMapResult<Job>(query, parameters, GetJobFromReader);
        }

        public Job? Save(Job job)
        {
            var query = @"
                    MERGE [Job] AS tgt
                    USING (SELECT @jobId, @name, @instrustions, @wage) AS src(JobId, Name, Instructions, Wage)
                        ON (tgt.JobId = src.JobId)
                    WHEN MATCHED
                        THEN
                            UPDATE
                            SET Name = src.Name
			                , Instructions = src.Instructions
                            , Wage = src.Wage
                    WHEN NOT MATCHED
                        THEN
                            INSERT (Name, Instructions, Wage)
                            VALUES (src.Name, src.Instructions, src.Wage)
                    OUTPUT inserted.*;";

            var parameters = new Dictionary<string, object>
            {
                { "@jobId", job.JobId },
                { "@name", job.DisplayText },
                { "@instrustions", job.Instructions },
                { "@wage", job.Wage }
            };

            return _connection.ExecuteReaderAndMapResult<Job>(query, parameters, GetJobFromReader);
        }

        private IEnumerable<Job> GetJobListFromReader(IDataReader reader)
        {
            var jobs = new List<Job>();
            while (reader.Read())
            {
                jobs.Add(MapJob(reader));
            }
            return jobs;
        }

        private Job MapJob(IDataReader reader)
        {
            return new Job
            {
                JobId = reader.GetInt64(0),
                DisplayText = reader.GetString(1),
                Instructions = reader.GetString(2),
                Wage = reader.GetDecimal(3)
            };
        }

        private Job? GetJobFromReader(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }

            if (reader.Read())
            {
                return MapJob(reader);
            }
            return null;
        }

    }
}
