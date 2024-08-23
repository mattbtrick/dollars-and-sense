using Repository;

namespace DollarAndSenseIntegrationTest.RepositoryTest
{
    [TestClass]
    public class JobRepositoryTests
    {
        private JobRepository repository => new JobRepository(TestData.GetConnection());

        [TestMethod]
        public void GetById_ShouldReturnNullWhenNotFound()
        {
            var jobRepository = repository;
            var job = jobRepository.GetById(0, false);
            Assert.IsNull(job);
        }

        [TestMethod]
        public void GetAll_ShouldReturnNonEmptyList()
        {
            var jobRepository = repository;
            var job = jobRepository.GetAll();
            Assert.IsNotNull(job);
            Assert.IsTrue(job.Any());
        }

        [TestMethod]
        public void GetById_ShouldReturnCorrectly()
        {
            var jobRepository = repository;
            var job = jobRepository.GetById(-3);
            Assert.IsNotNull(job);
            Assert.AreEqual(-3, job.JobId);
            Assert.AreEqual(300, job.Wage);
            Assert.AreEqual("$300 job instructions", job.Instructions);
            Assert.AreEqual("Test job 3", job.DisplayText);
        }

        [TestMethod]
        public void DeleteById_ShouldReturnDeleteCorrectly()
        {
            var jobRepository = repository;
            var job = jobRepository.GetById(-4);
            Assert.IsNotNull(job);

            jobRepository.DeleteById(-4);

            job = jobRepository.GetById(-4);
            Assert.IsNull(job);
        }

        [TestMethod]
        public void Save_ShouldAddAJob()
        {
            var expectedjob = new Models.Job
            {
                Instructions = "Test Intructions",
                DisplayText = "Test Display Text",
                Wage = 1,
            };

            var jobRepository = repository;
            var actualjob = jobRepository.Save(expectedjob);

            Assert.IsNotNull(actualjob);
            Assert.IsTrue(actualjob.JobId > 0);
            Assert.AreEqual(expectedjob.Wage, actualjob.Wage);
            Assert.AreEqual(expectedjob.Instructions, actualjob.Instructions);
            Assert.AreEqual(expectedjob.DisplayText, actualjob.DisplayText);

            jobRepository.DeleteById(actualjob.JobId);
        }

        [TestMethod]
        public void Save_ShouldUpdateAJob()
        {
            var jobRepository = repository;

            var expectedjob = jobRepository.GetById(-3);
            Assert.IsNotNull(expectedjob);

            expectedjob.Instructions += "Test";
            expectedjob.DisplayText += "Test";
            expectedjob.Wage += 1;

            var actualjob = jobRepository.Save(expectedjob);

            Assert.IsNotNull(actualjob);
            Assert.AreEqual(expectedjob.JobId, actualjob.JobId);
            Assert.AreEqual(expectedjob.Wage, actualjob.Wage);
            Assert.AreEqual(expectedjob.Instructions, actualjob.Instructions);
            Assert.AreEqual(expectedjob.DisplayText, actualjob.DisplayText);
        }
    }
}
