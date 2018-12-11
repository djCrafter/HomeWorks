using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace UserDbConnect
{
    class Program
    {
        static void Main(string[] args)
        {
            string conStr = @"Data Source=.\SQLEXPRESS; Initial Catalog=Userdb; Integrated Security=True;";

            SqlConnection connection = new SqlConnection(conStr);
          
            SqlCommand insertCommand = connection.CreateCommand();
            insertCommand.CommandText =
                "INSERT Users " +
                "VALUES " +
                "(1, 'Александр', 33)," +
                "(2, 'Николай', 24)," +
                "(3, 'Пётр', 28)," +
                "(4, 'Владимир', 66)," +
                "(5, 'Дмитрий', 45)";

            SqlCommand cmd = new SqlCommand("SELECT * FROM Users", connection);

            connection.Open();

            int rowAffected = insertCommand.ExecuteNonQuery();

            Console.WriteLine("Добавлено {0} строк.\n", rowAffected);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; ++i)
                    {
                        Console.WriteLine(reader.GetName(i) + ": " + reader[i]);
                    }
                    Console.WriteLine(new string('=', 15));
                }
            }

            connection.Close();
            Console.ReadLine();
        }
    }
}
