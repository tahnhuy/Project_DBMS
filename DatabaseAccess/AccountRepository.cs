using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale_Management.DatabaseAccess
{
    internal class AccountRepository
    {
        public static DataTable GetAllAccounts()
        {
            string query = "GetAllAccounts";
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure);
        }

        public static DataTable CheckLogin(string username, string password)
        {
            string query = "CheckLogin";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password)
            };
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure, parameters);
        }

        public static DataTable AddAccount(string creatorUsername, string newUsername, string newPassword, string newRole)
        {
            string query = "AddAccount";
            SqlParameter[] parameters = {
                new SqlParameter("@CreatorUsername", creatorUsername),
                new SqlParameter("@NewUsername", newUsername),
                new SqlParameter("@NewPassword", newPassword),
                new SqlParameter("@NewRole", newRole)
            };
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure, parameters);
        }

        public static DataTable UpdateAccount(string username, string newPassword = null, string newRole = null)
        {
            string query = "UpdateAccount";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", username),
                new SqlParameter("@NewPassword", newPassword ?? (object)DBNull.Value),
                new SqlParameter("@NewRole", newRole ?? (object)DBNull.Value)
            };
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure, parameters);
        }

        public static DataTable DeleteAccount(string username)
        {
            string query = "DeleteAccount";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", username)
            };
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure, parameters);
        }

        public static DataTable GetAccountDetails(string username)
        {
            string query = "GetAccountDetails";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", username)
            };
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure, parameters);
        }

        public static DataTable CheckAccountExists(string username)
        {
            string query = "CheckAccountExists";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", username)
            };
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure, parameters);
        }

        public static DataTable SearchAccounts(string searchTerm)
        {
            string query = "SearchAccounts";
            SqlParameter[] parameters = {
                new SqlParameter("@SearchTerm", searchTerm)
            };
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure, parameters);
        }

        public static DataTable GetAccountsByRole(string role)
        {
            string query = "GetAccountsByRole";
            SqlParameter[] parameters = {
                new SqlParameter("@Role", role)
            };
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure, parameters);
        }

        public static DataTable CountAccountsByRole(string role)
        {
            string query = "CountAccountsByRole";
            SqlParameter[] parameters = {
                new SqlParameter("@Role", role)
            };
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure, parameters);
        }

        public static DataTable CheckUserPermission(string username, string action)
        {
            string query = "CheckUserPermission";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", username),
                new SqlParameter("@Action", action)
            };
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure, parameters);
        }

        public static DataTable ChangePassword(string username, string oldPassword, string newPassword)
        {
            string query = "ChangePassword";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", username),
                new SqlParameter("@OldPassword", oldPassword),
                new SqlParameter("@NewPassword", newPassword)
            };
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure, parameters);
        }

        public static DataTable ResetPassword(string managerUsername, string targetUsername, string newPassword)
        {
            string query = "ResetPassword";
            SqlParameter[] parameters = {
                new SqlParameter("@ManagerUsername", managerUsername),
                new SqlParameter("@TargetUsername", targetUsername),
                new SqlParameter("@NewPassword", newPassword)
            };
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure, parameters);
        }

        public static DataTable GetAccountStatistics()
        {
            string query = "GetAccountStatistics";
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure);
        }

        public static DataTable GetAccountActivity(string username)
        {
            string query = "GetAccountActivity";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", username)
            };
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure, parameters);
        }

        public static DataTable ValidateUsername(string username)
        {
            string query = "ValidateUsername";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", username)
            };
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure, parameters);
        }

        public static DataTable ValidatePassword(string password)
        {
            string query = "ValidatePassword";
            SqlParameter[] parameters = {
                new SqlParameter("@Password", password)
            };
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure, parameters);
        }

        public static DataTable GetAccountsWithDetails()
        {
            string query = "GetAccountsWithDetails";
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure);
        }

        public static DataTable BackupAccounts()
        {
            string query = "BackupAccounts";
            return DatabaseConnection.ExecuteQuery(query, CommandType.StoredProcedure);
        }
    }
}
