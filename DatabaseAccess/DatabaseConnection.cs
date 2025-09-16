using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Sale_Management.DatabaseAccess
{
    internal class DatabaseConnection
    {
        private static string connectionString = Properties.Settings.Default.ConnectionString;

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        public static DataTable ExecuteQuery(string q, CommandType ct, params SqlParameter[] pa)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(q, conn))
                {
                    cmd.CommandType = ct;
                    if (pa != null)
                    {
                        cmd.Parameters.AddRange(pa);
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public static int ExecuteNonQuery(string q, CommandType ct, params SqlParameter[] pa)
        {
            int affectedRows = 0;
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(q, conn))
                {
                    cmd.CommandType = ct;
                    if (pa != null)
                    {
                        cmd.Parameters.AddRange(pa);
                    }
                    affectedRows = cmd.ExecuteNonQuery();
                }
            }
            return affectedRows;
        }
    }
}       
