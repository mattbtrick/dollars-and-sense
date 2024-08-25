using Repository;

namespace DollarAndSenseIntegrationTest.RepositoryTest
{
    [TestClass]
    public class PermissionRepositoryTests
    {
        private PermissionRepository repository => new PermissionRepository(TestData.GetConnection());

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
            Assert.AreEqual(-3, result.PermissionId);
            Assert.AreEqual("Test Read Users", result.Name);
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
        public void Save_ShouldAddAnExpense()
        {
            var expectedPermission = new Models.Permission
            {
                Name = "Test Name"
            };

            var actualPermission = repository.Save(expectedPermission);
            
            Assert.IsNotNull(actualPermission);
            Assert.IsTrue(actualPermission.PermissionId > 0);
            Assert.AreEqual(expectedPermission.Name, actualPermission.Name);

            repository.DeleteById(actualPermission.PermissionId);
        }

        [TestMethod]
        public void Save_ShouldUpdateAnExpense()
        {
            var expectedPermission = repository.GetById(-3);
            Assert.IsNotNull(expectedPermission);

            expectedPermission.Name += "Test";

            var actualPermission = repository.Save(expectedPermission);

            Assert.IsNotNull(actualPermission);
            Assert.AreEqual(expectedPermission.Name, actualPermission.Name);
        }
    }
}
