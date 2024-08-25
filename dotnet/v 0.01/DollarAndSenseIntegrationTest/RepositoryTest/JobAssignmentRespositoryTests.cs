using Models;
using Repository;

namespace DollarAndSenseIntegrationTest.RepositoryTest
{
    [TestClass]
    public class JobAssignmentRespositoryTests
    {
        private JobAssignmentRepository repository => new JobAssignmentRepository(TestData.GetConnection());

        [TestMethod]
        public void DeleteByJobIdShouldDelete()
        {
            var jobId = -3;
            repository.DeleteByJobId(jobId);

            var results = repository.GetByJobId(jobId);
            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public void DeleteByUserIdShouldDelete()
        {
            var userId = -2;
            repository.DeleteByUserId(userId);

            var results = repository.GetByUserId(userId);
            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public void DeleteShouldDelete()
        {
            var assignment = new JobAssignment { JobId = -1, UserId = -1};
            repository.Delete(assignment);

            var results = repository.Get(assignment.JobId, assignment.UserId);
            Assert.IsNull(results);
        }

        [TestMethod]
        public void GetByJobIdShouldReturnList()
        {
            var jobId = -5;
            
            var results = repository.GetByJobId(jobId);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void GetByUserIdShouldReturnList()
        {
            var userId = -5;

            var results = repository.GetByUserId(userId);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void GetShouldReturnCorrectly()
        {
            var assignment = new JobAssignment { JobId = -6, UserId = -6 };
            var results = repository.Get(assignment.JobId, assignment.UserId);
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void SaveShouldReturnCorrectly()
        {
            var assignment = new JobAssignment { JobId = -7, UserId = -7 };
            var results = repository.Save(assignment);
            Assert.IsNotNull(results);
            Assert.AreEqual(assignment.JobId, results.JobId);
            Assert.AreEqual(assignment.UserId, results.UserId);
        }
    }
}
