using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Lesson2
{
    class Program
    {
        const string conStr = @"Data Source=.\SQLEXPRESS; Initial Catalog=ShopDB; Integrated Security=True;";
        async static void showProduct(int product_id)
        {

            SqlConnection connection = new SqlConnection(conStr);

            connection.Open();

            SqlCommand command = new SqlCommand("productById", connection) { CommandType = CommandType.StoredProcedure };
            SqlDataReader dataReader = null;
            try
            {
                command.Transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                command.Parameters.AddWithValue("Id", product_id);
                dataReader = await command.ExecuteReaderAsync();
                while (dataReader.Read())
                {
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        Console.WriteLine(dataReader.GetName(i) + ": " + dataReader[i]);
                    }
                    Console.WriteLine(new string('_', 20));
                }
            }
            catch (Exception)
            {
                command.Transaction.Rollback();
            }
            connection.Close();
        }

        async static void showAllProducts()
        {
            SqlConnection connection = new SqlConnection(conStr);
            connection.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Products", connection);
            
            SqlDataReader dataReader = await cmd.ExecuteReaderAsync();
            while (dataReader.Read())
            {
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    Console.WriteLine(dataReader.GetName(i) + ": " + dataReader[i]);
                }
                Console.WriteLine(new string('_', 20));
            }

            connection.Close();
        }

        static void Main(string[] args)
        {
            showAllProducts();
            showProduct(1);

            Console.ReadKey();


        }

    }
}
