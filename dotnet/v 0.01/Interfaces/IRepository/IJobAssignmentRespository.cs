using Models;

namespace Interfaces.IRepository
{
    public interface IJobAssignmentRepository
    {
        public IEnumerable<JobAssignment> GetByJobId(long jobId);
        public IEnumerable<JobAssignment> GetByUserId(long userId);
        JobAssignment? Get(long jobId, long userId);
        public void DeleteByJobId(long jobId);
        public void DeleteByUserId(long userId);
        public void Delete(JobAssignment jobAssignment);
        public JobAssignment Save(JobAssignment jobAssignment);
    }
}
