using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace Lesson1
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnectionStringBuilder stringConnection = new SqlConnectionStringBuilder();
            stringConnection.DataSource = @".\SQLEXPRESS";
            stringConnection.InitialCatalog = "TestDb";
            stringConnection.IntegratedSecurity = true;

            SqlConnection connection = new SqlConnection(stringConnection.ConnectionString);

            connection.Open();
            Console.WriteLine(connection.State);
            connection.Close();
            Console.WriteLine(connection.State);

            var setting = new ConnectionStringSettings
            {
                Name = "TestDb",
                ConnectionString = stringConnection.ConnectionString
            };


            Configuration config;
           
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings.Add(setting);
            config.Save();

        }
    }
}
