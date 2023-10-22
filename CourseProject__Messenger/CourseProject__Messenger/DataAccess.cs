using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CourseProject__Messenger
{
    internal class DataAccess
    {

        private string connectionString;

        public DataAccess(string connection)
        {
            connectionString = connection;

        }
        private SqlConnection GetOpenConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public class MyData
        {
            public int id { get; set; }
            public string nikname { get; set; }
            public string email { get; set; }
            public string pass { get; set; }
            public string discription { get; set; }
        }
        public List<MyData> GetData()
        {
            using (SqlConnection connection = GetOpenConnection())
            using (SqlCommand command = new SqlCommand("SELECT * FROM users", connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                List<MyData> data = new List<MyData>();
                while (reader.Read())
                {
                    MyData item = new MyData
                    {
                       
                    };
                    data.Add(item);
                }
                return data;
            }
        }

    }

}


