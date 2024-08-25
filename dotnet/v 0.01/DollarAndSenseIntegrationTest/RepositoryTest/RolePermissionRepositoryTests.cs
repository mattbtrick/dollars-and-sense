using Models;
using Repository;

namespace DollarAndSenseIntegrationTest.RepositoryTest
{
    [TestClass]
    public class RolePermissionRepositoryTests
    {
        private RolePermissionRepository repository => new RolePermissionRepository(TestData.GetConnection());

        [TestMethod]
        public void DeleteByRoleIdShouldDelete()
        {
            var roleId = -4;
            repository.DeleteByRoleId(roleId);

            var results = repository.GetByRoleId(roleId);
            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public void DeleteByPermissionIdShouldDelete()
        {
            var permissionId = -4;
            repository.DeleteByPermissionId(permissionId);

            var results = repository.GetByPermissionId(permissionId);
            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public void DeleteShouldDelete()
        {
            var rolePermission = new RolePermission { RoleId = -3, PermissionId = -5 };
            var results = repository.Get(rolePermission.RoleId, rolePermission.PermissionId);
            Assert.IsNotNull(results);
                        
            repository.Delete(rolePermission);

            results = repository.Get(rolePermission.RoleId, rolePermission.PermissionId);
            Assert.IsNull(results);
        }

        [TestMethod]
        public void GetByRoleIdShouldReturnList()
        {
            var roleId = -1;
            
            var results = repository.GetByRoleId(roleId);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void GetByPermissionIdShouldReturnList()
        {
            var permissionId = -1;

            var results = repository.GetByPermissionId(permissionId);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void GetShouldReturnCorrectly()
        {
            var rolePermission = new RolePermission { RoleId = -1, PermissionId = -1 };
            var results = repository.Get(rolePermission.RoleId, rolePermission.PermissionId);
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void SaveShouldReturnCorrectly()
        {
            var rolePermission = new RolePermission { RoleId = -7, PermissionId = 2 };
            var results = repository.Save(rolePermission);
            Assert.IsNotNull(results);
            Assert.AreEqual(rolePermission.RoleId, results.RoleId);
            Assert.AreEqual(rolePermission.PermissionId, results.PermissionId);
        }
    }
}
