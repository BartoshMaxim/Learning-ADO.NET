using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Lesson3
{

    static class SqlTableHelper
    {
        public static void LoadWithSchema(this DataTable table, SqlDataReader reader)
        {
            table.Load(reader);
        }

        public static void CreateSchemaReader(this DataTable table, SqlDataReader reader)
        {
            DataTable schemaTable = reader.GetSchemaTable();

            foreach (DataRow schemaRow in schemaTable.Rows)
            {
                DataColumn column = new DataColumn((string)schemaRow["ColumnName"]);
                column.AllowDBNull = (bool)schemaRow["AllowDBNull"];
                column.DataType = (Type)schemaRow["DataType"];
                column.Unique = (bool)schemaRow["IsUnique"];
                column.ReadOnly = (bool)schemaRow["IsReadOnly"];                        // получение значения свойства Readonly
                column.AutoIncrement = (bool)schemaRow["IsIdentity"];                   // получение значения свойства AutoIncrement

                if (column.DataType == typeof(string))                                  // если поле типа string
                    column.MaxLength = (int)schemaRow["ColumnSize"];                    // получить значение свойства MaxLength

                if (column.AutoIncrement == true)                                       // Если поле с автоинкрементом 
                { column.AutoIncrementStep = -1; column.AutoIncrementSeed = 0; }        // задать свойства AutoIncrementStep и AutoIncrementSeed

                table.Columns.Add(column);
            }
        }
    }

    class Program
    {

        const string conStr = @"Data Source=.\SQLEXPRESS; Initial Catalog=ShopDB; Integrated Security=true;";

        static void Main(string[] args)
        {
            int [3]k;

            DataSet shopDb = new DataSet();
            DataTable orders = new DataTable("Orders");
            DataTable customers = new DataTable("Customers");
            DataTable employees = new DataTable("Employees");
            DataTable orderDetails = new DataTable("OrderDetails"); 
            DataTable products = new DataTable("Products");
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand selectOrdersCom = new SqlCommand("SELECT * FROM Orders", con);
                SqlCommand selectCustomersCom = new SqlCommand("SELECT * FROM Customers", con);
                SqlCommand selectEmployeesCom = new SqlCommand("SELECT * FROM Employees", con);
                SqlCommand selectOrderDetailsCom = new SqlCommand("SELECT * FROM OrderDetails", con);
                SqlCommand selectProductsCom = new SqlCommand("SELECT * FROM Products", con);

                con.Open();
                orders.LoadWithSchema(selectOrdersCom.ExecuteReader());
                customers.LoadWithSchema(selectCustomersCom.ExecuteReader());
                employees.LoadWithSchema(selectEmployeesCom.ExecuteReader());
                orderDetails.LoadWithSchema(selectOrderDetailsCom.ExecuteReader());
                products.LoadWithSchema(selectProductsCom.ExecuteReader());
            }
            orders.PrimaryKey = new DataColumn[] { orders.Columns[0] };
            customers.PrimaryKey = new DataColumn[] { customers.Columns[0] };
            employees.PrimaryKey = new DataColumn[] { employees.Columns[0] };
            products.PrimaryKey = new DataColumn[] { products.Columns[0] };

            var FK_CustomersOrders = new  ForeignKeyConstraint(customers.Columns["CustomerNo"], orders.Columns["CustomerNo"]);
            orders.Constraints.Add(FK_CustomersOrders);
        }
    }
}
