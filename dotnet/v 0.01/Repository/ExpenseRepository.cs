using Interfaces.IRepository;
using Models;
using Repository.Extensions;
using System.Data;

namespace Repository
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly IDbConnection _connection;

        public ExpenseRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void DeleteById(long id)
        {
            string query = @"DELETE [Expense] WHERE ExpenseId = @expenseId";
            _connection.ExecuteQuery(query, new Dictionary<string, object>
            {
                {"expenseId", id}
            });
        }

        public IEnumerable<Expense> GetAll()
        {
            string query = @"SELECT ExpenseId, Expense, Description, Amount FROM [Expense]";

            return _connection.ExecuteReaderAndMapResult<IEnumerable<Expense>>(query
                , new Dictionary<string, object>()
                , GetExpenseListFromReader) ?? new List<Expense>();
        }

        public Expense? GetById(long id)
        {
            return GetById(id, false);
        }

        public Expense? GetById(long id, bool loadChildern)
        {
            string query = @"SELECT ExpenseId, Expense, Description, Amount FROM [Expense] WHERE ExpenseId = @expenseId";
            var parameters = new Dictionary<string, object>
            {
                { "expenseId", id }
            };

            return _connection.ExecuteReaderAndMapResult<Expense>(query, parameters, GetExpenseFromReader);
        }

        public Expense? Save(Expense expense)
        {
            var query = @"
                    MERGE [Expense] AS tgt
                    USING (SELECT @expenseId, @expense, @description, @amount) AS src(ExpenseId, Expense, Description, Amount)
                        ON (tgt.ExpenseId = src.ExpenseId)
                    WHEN MATCHED
                        THEN
                            UPDATE
                            SET Expense = src.Expense
			                , Description = src.Description
                            , Amount = src.Amount
                    WHEN NOT MATCHED
                        THEN
                            INSERT (Expense, Description, Amount)
                            VALUES (src.Expense, src.Description, src.Amount)
                    OUTPUT inserted.*;";

            var parameters = new Dictionary<string, object>
            {
                { "@expenseId", expense.ExpenseId },
                { "@expense", expense.DisplayText },
                { "@description", expense.Description },
                { "@amount", expense.Amount }
            };

            return _connection.ExecuteReaderAndMapResult<Expense>(query, parameters, GetExpenseFromReader);
        }

        private IEnumerable<Expense> GetExpenseListFromReader(IDataReader reader)
        {
            var expenses = new List<Expense>();
            while (reader.Read())
            {
                expenses.Add(MapExpense(reader));
            }
            return expenses;
        }

        private Expense MapExpense(IDataReader reader)
        {
            return new Expense
            {
                ExpenseId = reader.GetInt64(0),
                DisplayText = reader.GetString(1),
                Description = reader.GetString(2),
                Amount = reader.GetDecimal(3)
            };
        }

        private Expense? GetExpenseFromReader(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }

            if (reader.Read())
            {
                return MapExpense(reader);
            }
            return null;
        }

    }
}
