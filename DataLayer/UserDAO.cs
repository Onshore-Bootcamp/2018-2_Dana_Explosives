using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace DataLayer
{
    public class UserDAO
    {
        //public string currentClass = "UserDAO";
        private readonly string connectionString;
        private string filePath;

        //Passing connection string from controller
        public UserDAO(string dataConnection, string path)
        {
            connectionString = dataConnection;
            filePath = path;
        }
        /// <summary>
        /// READ USER BY USERNAME to verify password in login
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserDO ReadUserByUsername(string username)
        {
            UserDO user = new UserDO();

            try
            {
                //Instantiate SqlConnection 
                using (SqlConnection conn = new SqlConnection(connectionString))

                //Creating a new SqlCommand to use a stored procedure.
                using (SqlCommand enterCommand = new SqlCommand("DISPLAY_USER_BY_USERNAME", conn))
                {
                    //Using stored procedure
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue("@Username", username);
                    conn.Open();
                    
                    //Reading from datareader by index
                    using (SqlDataReader userReader = enterCommand.ExecuteReader())
                    {
                        if (userReader.Read())
                        {
                            user.UserID = userReader.GetInt64(0);
                            user.Username = userReader.GetString(1);
                            user.Password = userReader.GetString(2);
                            user.FirstName = userReader.GetString(3);
                            user.LastName = userReader.GetString(4);
                            user.Email = userReader["Email"] == DBNull.Value ? String.Empty : (string)userReader["Email"];
                            user.RoleID = userReader.GetInt32(6);
                        }
                    }
                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                UserErrorHandler("fatal", "UserDAO", "ReadUserByUsername", ex.Message, ex.StackTrace);
                throw ex;
            }
            return user;
        }
        /// <summary>
        /// CREATE USER
        /// </summary>
        /// <param name="user"></param>
        public void CreateUser(UserDO user)
        {
            try
            {
                //Instantiate SqlConnection 
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Creating a new SqlCommand to use a stored procedure.
                    SqlCommand cmd = new SqlCommand("REGISTER_USER", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Parameters that are being passed to the stored procedures.
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@RoleID", user.RoleID);

                    //Opening connection and executing nonquery
                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                UserErrorHandler("fatal", "UserDAO", "CreateUser", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// VIEW USERS
        /// </summary>
        /// <returns></returns>
        public List<UserDO> ViewAllUsers()
        {
            List<UserDO> allUsers = new List<UserDO>();
            try
            {


                //Instantiate SqlConnection 
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //View users by stored procedure
                    SqlCommand enterCommand = new SqlCommand("DISPLAY_USER", conn);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    //Using adapter to get table from the database
                    DataTable userInfo = new DataTable();
                    using (SqlDataAdapter userAdapter = new SqlDataAdapter(enterCommand))
                    {
                        userAdapter.Fill(userInfo);
                        userAdapter.Dispose();
                    }

                    //Put datarow into a list of userDO
                    foreach (DataRow row in userInfo.Rows)
                    {
                        UserDO mappedRow = MapAllUsers(row);
                        allUsers.Add(mappedRow);
                    }
                }
            }
            catch (Exception ex)
            {
                UserErrorHandler("fatal", "UserDAO", "ViewAllUsers", ex.Message, ex.StackTrace);
                throw ex;
            }
            return allUsers;
        }
        /// <summary>
        /// VIEW USER BY ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserDO ViewUserByID(Int64 userId)
        {
            UserDO user = new UserDO();
            try
            {

                //Instantiate SqlConnection 
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Creating a new SqlCommand to use a stored procedure.
                    SqlCommand enterCommand = new SqlCommand("DISPLAY_USER_BY_ID", conn);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue("@UserID", userId);
                    conn.Open();

                    //Using SqlDataAdapter to get SQL table.
                    DataTable userInfo = new DataTable();
                    using (SqlDataAdapter userAdapter = new SqlDataAdapter(enterCommand))
                    {
                        userAdapter.Fill(userInfo);
                        userAdapter.Dispose();
                    }

                    //Putting datarow into a List of the users object.
                    foreach (DataRow row in userInfo.Rows)
                    {
                        user = MapAllUsers(row);
                    }
                }

            }
            catch (Exception ex)
            {
                UserErrorHandler("fatal", "UserDAO", "ViewUserByID", ex.Message, ex.StackTrace);
                throw ex;
            }
            //Returning an updated list of the user object.
            return user;
        }
        /// <summary>
        /// UPDATE A USER
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(UserDO user)
        {
            try
            {
                //Instantiate SqlConnection 
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Creating new SqlCommand to use a stored procedure
                    SqlCommand enterCommand = new SqlCommand("UPDATE_USER", conn);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    
                    //Parameters being passed from UserDO to the stored procedures
                    enterCommand.Parameters.AddWithValue("@Username", user.Username);
                    enterCommand.Parameters.AddWithValue("@Password", user.Password);
                    enterCommand.Parameters.AddWithValue("@FirstName", user.FirstName);
                    enterCommand.Parameters.AddWithValue("@LastName", user.LastName);
                    enterCommand.Parameters.AddWithValue("@Email", user.Email);
                    enterCommand.Parameters.AddWithValue("@RoleID", user.RoleID);
                    enterCommand.Parameters.AddWithValue("@UserID", user.UserID);

                    //Opening connection and executing nonquery
                    conn.Open();
                    enterCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                UserErrorHandler("fatal", "UserDAO", "UpdateUser", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// DELETE USER
        /// </summary>
        /// <param name="userID"></param>
        public void DeleteUser(Int64 userID)
         {

            try
            {
                //Instantiate SqlConnection 
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Using stored procedure to delete user
                        SqlCommand enterCommand = new SqlCommand("DELETE_USER", conn);
                        enterCommand.CommandType = CommandType.StoredProcedure;
                        enterCommand.Parameters.AddWithValue("UserID", userID);

                        //Opening connection and executing nonquery
                        conn.Open();
                        enterCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                UserErrorHandler("fatal", "UserDAO", "DeleteUser", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// MAPS ALL USERS
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public UserDO MapAllUsers(DataRow dataRow)
        {
            try
            {
                UserDO user = new UserDO();

                //Check to see if database object is null
                if (dataRow["UserID"] != DBNull.Value)
                {
                    user.UserID = (Int64)dataRow["UserID"];
                }
                user.Username = dataRow["Username"].ToString();
                user.Password = dataRow["Password"].ToString();
                user.FirstName = dataRow["FirstName"].ToString();
                user.LastName = dataRow["LastName"].ToString();
                user.Email = dataRow["Email"].ToString();

                return user;
            }
            catch (Exception ex)
            {
                UserErrorHandler("fatal", "UserDAO", "MapAllUsers", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
        public void UserErrorHandler(string level, string currentClass, string currentMethod, string message, string stackTrace = null)
        {

            string errorLog = filePath + "/ErrorLogUser.txt";

            try
            {
                //using StreamWriter to write error message to file.
                using (StreamWriter logWriter = new StreamWriter(errorLog, true))
                {

                    logWriter.WriteLine(new string('-', 150));
                    logWriter.WriteLine($"{DateTime.Now.ToString()} - {level} - {currentClass} - {currentMethod}");
                    logWriter.WriteLine(message);

                    if (!string.IsNullOrWhiteSpace(stackTrace))
                    {
                        logWriter.WriteLine(stackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                UserErrorHandler("fatal", "UserDAO", "UserErrorHandler", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
    }
}
