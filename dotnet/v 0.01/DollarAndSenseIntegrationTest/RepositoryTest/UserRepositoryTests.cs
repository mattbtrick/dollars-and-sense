using Microsoft.Data.SqlClient;
using Models;
using Repository;

namespace DollarAndSenseIntegrationTest.RepositoryTest
{
    [TestClass]
    public class UserRepositoryTests
    {
        private UserRepository repository => new UserRepository(TestData.GetConnection());

        [TestMethod]
        public void GetById_ShouldReturnNullWhenNotFound()
        {
            var userRepository = repository;
            var user = userRepository.GetById(0, false);
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetById_ShouldReturnWhenNotFound()
        {
            var userRepository = repository;
            var user = userRepository.GetById(0, false);
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetAll_ShouldReturnCorrectNumberOfRecords()
        {
            var userRepository = repository;
            var userList = userRepository.GetAll();
            Assert.IsNotNull(userList);
            Assert.AreEqual(11, userList.Count());
        }

        [TestMethod]
        public void User_Crud_ShouldWork()
        {
            var expectedUser = new User { FirstName = "TestFirst", LastName = "TestLast" };
            var userRepository = repository;
            var actualUser = userRepository.Save(expectedUser);
            Assert.IsNotNull(actualUser);
            Assert.AreEqual(expectedUser.FirstName, actualUser.FirstName);
            Assert.AreEqual(expectedUser.LastName, actualUser.LastName);

            expectedUser.UserId = actualUser.UserId;
            actualUser = userRepository.GetById(expectedUser.UserId, false);
            Assert.IsNotNull(actualUser);
            Assert.AreEqual(expectedUser.UserId, actualUser.UserId);
            Assert.AreEqual(expectedUser.FirstName, actualUser.FirstName);
            Assert.AreEqual(expectedUser.LastName, actualUser.LastName);

            expectedUser.FirstName += "1";
            expectedUser.LastName += "1";
            actualUser = userRepository.Save(expectedUser);
            Assert.IsNotNull(actualUser);
            Assert.AreEqual(expectedUser.UserId, actualUser.UserId);
            Assert.AreEqual(expectedUser.FirstName, actualUser.FirstName);
            Assert.AreEqual(expectedUser.LastName, actualUser.LastName);

            actualUser = userRepository.GetById(expectedUser.UserId, false);
            Assert.IsNotNull(actualUser);
            Assert.AreEqual(expectedUser.UserId, actualUser.UserId);
            Assert.AreEqual(expectedUser.FirstName, actualUser.FirstName);
            Assert.AreEqual(expectedUser.LastName, actualUser.LastName);

            userRepository.DeleteById(expectedUser.UserId);

            actualUser = userRepository.GetById(expectedUser.UserId, false);
            Assert.IsNull(actualUser);
        }
    }
}