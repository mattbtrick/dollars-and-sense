using Repository;

namespace DollarAndSenseIntegrationTest.RepositoryTest
{
    [TestClass]
    public class RoleRepositoryTests
    {
        private RoleRepository repository => new RoleRepository(TestData.GetConnection());

        [TestMethod]
        public void GetById_ShouldReturnNullWhenNotFound()
        {
            var result = repository.GetById(0, false);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetById_ShouldReturnCorrectly()
        {
            var result = repository.GetById(-3);
            Assert.IsNotNull(result);
            Assert.AreEqual(-3, result.RoleId);
            Assert.AreEqual("Test Read", result.Name);
        }

        [TestMethod]
        public void GetAll_ShouldReturnNonEmptyList()
        {
            var result = repository.GetAll();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void DeleteById_ShouldReturnDeleteCorrectly()
        {
            var result = repository.GetById(-5);
            Assert.IsNotNull(result);

            repository.DeleteById(-4);

            result = repository.GetById(-4);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Save_ShouldAddARole()
        {
            var expectedRole = new Models.Role
            {
                Name = "Test Name"
            };

            var actualRole = repository.Save(expectedRole);
            
            Assert.IsNotNull(actualRole);
            Assert.IsTrue(actualRole.RoleId > 0);
            Assert.AreEqual(expectedRole.Name, actualRole.Name);

            repository.DeleteById(actualRole.RoleId);
        }

        [TestMethod]
        public void Save_ShouldUpdateARole()
        {
            var expectedRole = repository.GetById(-3);
            Assert.IsNotNull(expectedRole);

            expectedRole.Name += "Test";

            var actualRole = repository.Save(expectedRole);

            Assert.IsNotNull(actualRole);
            Assert.AreEqual(expectedRole.Name, actualRole.Name);
        }
    }
}
