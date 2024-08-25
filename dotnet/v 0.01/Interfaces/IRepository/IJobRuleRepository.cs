using Models;

namespace Interfaces.IRepository
{
    public interface IJobRuleRepository
    {
        public IEnumerable<JobRule> GetByJobId(long jobId);
        public IEnumerable<JobRule> GetByScheduleId(long scheduleId);
        JobRule? Get(long jobId, long scheduleId);
        public void DeleteByJobId(long jobId);
        public void DeleteByScheduleId(long scheduleId);
        public void Delete(JobRule jobRule);
        public JobRule Save(JobRule jobRule);
    }
}
