using Repository;

namespace DollarAndSenseIntegrationTest.RepositoryTest
{
    [TestClass]
    public class ExpenseRepositoryTests
    {
        private ExpenseRepository repository => new ExpenseRepository(TestData.GetConnection());

        [TestMethod]
        public void GetById_ShouldReturnNullWhenNotFound()
        {
            var expenseRepository = repository;
            var expense = expenseRepository.GetById(0, false);
            Assert.IsNull(expense);
        }

        [TestMethod]
        public void GetById_ShouldReturnCorrectly()
        {
            var expenseRepository = repository;
            var expense = expenseRepository.GetById(-3);
            Assert.IsNotNull(expense);
            Assert.AreEqual(-3, expense.ExpenseId);
            Assert.AreEqual(300, expense.Amount);
            Assert.AreEqual("$300 expense description", expense.Description);
            Assert.AreEqual("Test expense 3", expense.DisplayText);
        }

        [TestMethod]
        public void GetAll_ShouldReturnNonEmptyList()
        {
            var expenseRepository = repository;
            var expense = expenseRepository.GetAll();
            Assert.IsNotNull(expense);
            Assert.IsTrue(expense.Any());
        }

        [TestMethod]
        public void DeleteById_ShouldReturnDeleteCorrectly()
        {
            var expenseRepository = repository;
            var expense = expenseRepository.GetById(-4);
            Assert.IsNotNull(expense);

            expenseRepository.DeleteById(-4);

            expense = expenseRepository.GetById(-4);
            Assert.IsNull(expense);
        }

        [TestMethod]
        public void Save_ShouldAddAnExpense()
        {
            var expectedExpense = new Models.Expense
            {
                Amount = 1,
                Description = "Test Description",
                DisplayText = "Test Display Text",
            };

            var expenseRepository = repository;
            var actualExpense = expenseRepository.Save(expectedExpense);
            
            Assert.IsNotNull(actualExpense);
            Assert.IsTrue(actualExpense.ExpenseId > 0);
            Assert.AreEqual(expectedExpense.Amount, actualExpense.Amount);
            Assert.AreEqual(expectedExpense.Description, actualExpense.Description);
            Assert.AreEqual(expectedExpense.DisplayText, actualExpense.DisplayText);

            expenseRepository.DeleteById(actualExpense.ExpenseId);
        }

        [TestMethod]
        public void Save_ShouldUpdateAnExpense()
        {
            var expenseRepository = repository;

            var expectedExpense = expenseRepository.GetById(-3);
            Assert.IsNotNull(expectedExpense);

            expectedExpense.Description += "Test";
            expectedExpense.DisplayText += "Test";
            expectedExpense.Amount += 1;

            var actualExpense = expenseRepository.Save(expectedExpense);

            Assert.IsNotNull(actualExpense);
            Assert.AreEqual(expectedExpense.ExpenseId, actualExpense.ExpenseId);
            Assert.AreEqual(expectedExpense.Amount, actualExpense.Amount);
            Assert.AreEqual(expectedExpense.Description, actualExpense.Description);
            Assert.AreEqual(expectedExpense.DisplayText, actualExpense.DisplayText);
        }
    }
}
