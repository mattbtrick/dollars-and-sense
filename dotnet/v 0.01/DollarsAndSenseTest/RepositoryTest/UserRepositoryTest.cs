using Microsoft.Data.SqlClient;
using Models;
using Repository;

namespace DollarsAndSenseIntegrationTest.RepositoryTest
{
    public class UserRepositoryTest
    {
        private UserRepository repository => new UserRepository(new SqlConnection("Data Source=.;Initial Catalog=DollarsAndSense;Trusted_Connection=true;TrustServerCertificate=true;"));

        [Fact]
        public void GetById_ShouldReturnNullWhenNotFound()
        {
            var userRepository = repository;
            var user = userRepository.GetById(0, false);
            Assert.Null(user);
        }

        [Fact]
        public void GetById_ShouldReturnWhenNotFound()
        {
            var userRepository = repository;
            var user = userRepository.GetById(0, false);
            Assert.Null(user);
        }

        [Fact]
        public void GetAll_ShouldReturnCorrectNumberOfRecords()
        {
            var userRepository = repository;
            var userList = userRepository.GetAll();
            Assert.NotNull(userList);
            Assert.Equal(6, userList.Count());
        }

        [Fact]
        public void User_Crud_ShouldWork()
        {
            var expectedUser = new User { FirstName = "TestFirst", LastName = "TestLast" };
            var userRepository = repository;
            var actualUser = userRepository.Save(expectedUser);
            Assert.NotNull(actualUser);
            Assert.Equal(expectedUser.FirstName, actualUser.FirstName);
            Assert.Equal(expectedUser.LastName, actualUser.LastName);

            expectedUser.UserId = actualUser.UserId;
            actualUser = userRepository.GetById(expectedUser.UserId, false);
            Assert.NotNull(actualUser);
            Assert.Equal(expectedUser.UserId, actualUser.UserId);
            Assert.Equal(expectedUser.FirstName, actualUser.FirstName);
            Assert.Equal(expectedUser.LastName, actualUser.LastName);

            expectedUser.FirstName += "1";
            expectedUser.LastName += "1";
            actualUser = userRepository.Save(expectedUser);
            Assert.NotNull(actualUser);
            Assert.Equal(expectedUser.UserId, actualUser.UserId);
            Assert.Equal(expectedUser.FirstName, actualUser.FirstName);
            Assert.Equal(expectedUser.LastName, actualUser.LastName);

            actualUser = userRepository.GetById(expectedUser.UserId, false);
            Assert.NotNull(actualUser);
            Assert.Equal(expectedUser.UserId, actualUser.UserId);
            Assert.Equal(expectedUser.FirstName, actualUser.FirstName);
            Assert.Equal(expectedUser.LastName, actualUser.LastName);

            userRepository.DeleteById(expectedUser.UserId);

            actualUser = userRepository.GetById(expectedUser.UserId, false);
            Assert.Null(actualUser);
        }
    }
}