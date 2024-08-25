using Models;
using Repository;

namespace DollarAndSenseIntegrationTest.RepositoryTest
{
    [TestClass]
    public class JobRuleRespositoryTests
    {
        private JobRuleRepository repository => new JobRuleRepository(TestData.GetConnection());

        [TestMethod]
        public void DeleteByJobIdShouldDelete()
        {
            var jobId = -6;
            repository.DeleteByJobId(jobId);

            var results = repository.GetByJobId(jobId);
            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public void DeleteByCompletionScheduleIdShouldDelete()
        {
            var scheduleId = 3;
            repository.DeleteByScheduleId(scheduleId);

            var results = repository.GetByScheduleId(scheduleId);
            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public void DeleteShouldDelete()
        {
            var assignment = new JobRule { JobId = -6, CompletionScheduleId = 2};
            repository.Delete(assignment);

            var results = repository.Get(assignment.JobId, assignment.CompletionScheduleId);
            Assert.IsNull(results);
        }

        [TestMethod]
        public void GetByJobIdShouldReturnList()
        {
            var jobId = -1;
            
            var results = repository.GetByJobId(jobId);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void GetByCompletionScheduleIdShouldReturnList()
        {
            var scheudleId = 1;

            var results = repository.GetByScheduleId(scheudleId);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void GetShouldReturnCorrectly()
        {
            var assignment = new JobRule { JobId = -1, CompletionScheduleId = 1 };
            var results = repository.Get(assignment.JobId, assignment.CompletionScheduleId);
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void SaveShouldReturnCorrectly()
        {
            var assignment = new JobRule { JobId = -7, CompletionScheduleId = 2 };
            var results = repository.Save(assignment);
            Assert.IsNotNull(results);
            Assert.AreEqual(assignment.JobId, results.JobId);
            Assert.AreEqual(assignment.CompletionScheduleId, results.CompletionScheduleId);
        }
    }
}
