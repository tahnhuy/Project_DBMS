using System;
using System.Data;
using System.Data.SqlClient;

namespace Sale_Management.DatabaseAccess
{
    internal class AccountRepository
    {
        public static DataTable CheckLogin(string username, string password)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username },
                new SqlParameter("@Password", SqlDbType.NVarChar, 255) { Value = password }
            };
            return DatabaseConnection.ExecuteQuery("CheckLogin", CommandType.StoredProcedure, parameters);
        }

        public static DataTable ChangePassword(string username, string oldPassword, string newPassword)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username },
                new SqlParameter("@OldPassword", SqlDbType.NVarChar, 255) { Value = oldPassword },
                new SqlParameter("@NewPassword", SqlDbType.NVarChar, 255) { Value = newPassword }
            };
            return DatabaseConnection.ExecuteQuery("ChangePassword", CommandType.StoredProcedure, parameters);
        }
    }
}
