using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace DataLayer
{
    public class MunitionsDAO
    {
        //Creating dependencies
        private string connectionString;
        private string filePath;

        //Constructor allowing access to SQL and logging
        public MunitionsDAO(string dataConnection, string path)
        {
            connectionString = dataConnection;
            filePath = path;
        }

        /// <summary>
        /// View Munitions by TopicID -- called in Controller Index()
        /// </summary>
        /// <param name="topicID"></param>
        /// <returns></returns>
        public List<MunitionsDO> ViewMunitions(Int64 topicID)
        {
            //Intantiating new list to return munitions
            List<MunitionsDO> munitions = new List<MunitionsDO>();
            try
            {
                //Instantiating SqlConnection
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Creating a SqlCommand to use a stored procedure.
                    SqlCommand cmd = new SqlCommand("DISPLAY_MUNITIONS_BY_TOPIC", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TopicID", topicID);
                    conn.Open();

                    //Using SqlDataAdapter to get SQL table.
                    DataTable munitionInfo = new DataTable();
                    using (SqlDataAdapter munitionAdapter = new SqlDataAdapter(cmd))
                    {
                        munitionAdapter.Fill(munitionInfo);
                        munitionAdapter.Dispose();
                    }

                    //Putting datarow into a list.                    
                    foreach (DataRow row in munitionInfo.Rows)
                    {
                        MunitionsDO mappedMunitions = MapAllMunitions(row);
                        munitions.Add(mappedMunitions);
                    }
                }
            }
            catch (Exception ex)
            {
                //Error logging
                MunitionsErrorHandler("error", "MunitionsDAO", "ViewMunitions", ex.Message, ex.StackTrace);
                throw ex;
            }
            //Returning list of munitions
            return munitions;
        }

        /// <summary>
        /// Viewing Topics for DropDown List -- called in Controller RenderTopics()
        /// </summary>
        /// <returns></returns>
        public List<TopicDO> ViewTopics()
        {
            //Creating a new list to return
            List<TopicDO> allTopics = new List<TopicDO>();
            try
            {

                //Instantiating SqlConnection 
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Creating a SqlCommand to use a stored procedure
                    SqlCommand cmd = new SqlCommand("DISPLAY_TOPICS", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    //Filling table using adapter
                    DataTable topicsTable = new DataTable();
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                    {
                        dataAdapter.Fill(topicsTable);
                        dataAdapter.Dispose();
                    }

                    //Adding each topic into a list of topics
                    foreach (DataRow row in topicsTable.Rows)
                    {
                        TopicDO mappedTopic = MapAllTopics(row);
                        allTopics.Add(mappedTopic);
                    }
                }
            }
            catch (Exception ex)
            {
                //Error logging
                MunitionsErrorHandler("error", "MunitionsDAO", "ViewTopics", ex.Message, ex.StackTrace);
                throw ex;
            }
            //Returning all Topics
            return allTopics;
        }

        /// <summary>
        /// Create Munition -- called in Controller Post Create()
        /// </summary>
        /// <param name="munitions"></param>
        public void CreateMunitions(MunitionsDO munitions)
        {
            try
            {
                //Instantiating SqlConnection
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Creating a SqlCommand to use a stored procedure
                    SqlCommand cmd = new SqlCommand("CREATE_MUNITION", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Munition", munitions.Munition);
                    cmd.Parameters.AddWithValue("@Description", munitions.Description);
                    cmd.Parameters.AddWithValue("@TopicID", munitions.TopicID);

                    //Open connection and execute nonquery
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //Error logging
                MunitionsErrorHandler("error", "MunitionsDAO", "CreateMunitions", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Viewing munitions by their ID -- called in Controller Get Update()
        /// </summary>
        /// <param name="munitionID"></param>
        /// <returns></returns>
        public MunitionsDO ViewMunitionsByID(Int64 munitionID)
        {
            //Instantiating munitionsDO
            MunitionsDO munitions = new MunitionsDO();
            try
            {
                //Instantiating SqlConnection
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Creating a new SqlCommand to use a stored procedure.
                    SqlCommand cmd = new SqlCommand("DISPLAY_MUNITIONS_BY_ID", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MunitionID", munitionID);
                    conn.Open();

                    //Using SqlDataAdapter to get SQL table.
                    DataTable munitionInfo = new DataTable();
                    using (SqlDataAdapter munitionAdapter = new SqlDataAdapter(cmd))
                    {
                        munitionAdapter.Fill(munitionInfo);
                        munitionAdapter.Dispose();
                    }

                    //Mapping row to munitions                 
                    foreach (DataRow row in munitionInfo.Rows)
                    {
                        munitions = MapAllMunitions(row);
                    }
                }
            }
            catch (Exception ex)
            {
                //Error Logging
                MunitionsErrorHandler("error", "MunitionsDAO", "ViewMunitionsByID", ex.Message, ex.StackTrace);
                throw ex;
            }
            //returning munition's info by munitionID 
            return munitions;
        }

        /// <summary>
        /// Updates munition -- called in Controller Post Update()
        /// </summary>
        /// <param name="munitions"></param>
        public void UpdateMunitions(MunitionsDO munitions)
        {
            try
            {
                //Instantiating SqlConnection
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Creating a SqlCommand to use a stored procedure
                    SqlCommand cmd = new SqlCommand("UPDATE_MUNITION", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MunitionID", munitions.MunitionID);
                    cmd.Parameters.AddWithValue("@Munition", munitions.Munition);
                    cmd.Parameters.AddWithValue("@Description", munitions.Description);

                    //Open connection and execute nonquery
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                //Error logging
                MunitionsErrorHandler("error", "MunitionsDAO", "UpdateMunitions", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Deletes munition -- called in Controller Delete()
        /// </summary>
        /// <param name="munitionID"></param>
        public void DeleteMunitions(Int64 munitionID)
        {
            try
            {
                //Instantiating SqlConnection
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Creating a SqlCommand to use a stored procedure
                    SqlCommand cmd = new SqlCommand("DELETE_MUNITION", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MunitionID", munitionID);

                    //Open connection and execute nonquery
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //Error logging
                MunitionsErrorHandler("error", "MunitionsDAO", "DeleteMunitions", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Maps all munitions by row -- called in ViewMunitions()
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public MunitionsDO MapAllMunitions(DataRow dataRow)
        {
            //Instantiating new MunitionsDO
            MunitionsDO munitions = new MunitionsDO();
            try
            {
                //Checking to make sure row exists and is not null
                if (dataRow.Table.Columns.Contains("TopicID") && dataRow["TopicID"] != DBNull.Value)
                {
                    munitions.TopicID = (Int64)dataRow["TopicID"];

                }
                munitions.MunitionID = (Int64)dataRow["MunitionID"];
                munitions.Munition = dataRow["Munition"].ToString();
                munitions.Description = dataRow["Description"].ToString();
            }
            catch (Exception ex)
            {
                //Error logging
                MunitionsErrorHandler("error", "MunitionsDAO", "MapAllMunitions", ex.Message, ex.StackTrace);
                throw ex;
            }
            //returning munitions
            return munitions;
        }

        /// <summary>
        /// Maps all topics by row -- called in ViewTopics()
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public TopicDO MapAllTopics(DataRow dataRow)
        {
            //Instantiating new TopicDO to return topic
            TopicDO topic = new TopicDO();
            try
            {
                //checking data row for topicID of 0
                if ((Int64)dataRow["TopicID"] != 0)
                {
                    topic.TopicID = (Int64)dataRow["TopicID"];
                }
                topic.Topic = dataRow["Topic"].ToString();
            }
            catch (Exception ex)
            {
                //Error logging
                MunitionsErrorHandler("error", "MunitionsDAO", "MapAllTopics", ex.Message, ex.StackTrace);
                throw ex;
            }
            //Returning topic
            return topic;
        }

        /// <summary>
        /// Error Handler
        /// </summary>
        /// <param name="level"></param>
        /// <param name="currentClass"></param>
        /// <param name="currentMethod"></param>
        /// <param name="message"></param>
        /// <param name="stackTrace"></param>
        public void MunitionsErrorHandler(string level, string currentClass, string currentMethod, string message, string stackTrace = null)
        {
            //Filepath saved in Logs folder
            filePath.Insert(filePath.Length, "/ErrorLogMunitions.txt");

            try
            {
                //Using StreamWriter to write error message to file.
                using (StreamWriter logWriter = new StreamWriter(filePath, true))
                {
                    //Layout for file
                    logWriter.WriteLine(new string('-', 150));
                    logWriter.WriteLine($"{DateTime.Now.ToString()} - {level} - {currentClass} - {currentMethod}");
                    logWriter.WriteLine(message);

                    //checking for valid stackTrace
                    if (!string.IsNullOrWhiteSpace(stackTrace))
                    {
                        logWriter.WriteLine(stackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// View Munitions by TopicID -- called in Controller Index()/// 
        /// </summary>
        /// <returns></returns>
        public DataTable MunitionCount()
        {
            //Intantiating new table to return munitionInfo
            DataTable munitionInfo = new DataTable();
            try
            {
                //Instantiate SqlConnection
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Creating a SqlCommand to use a stored procedure.
                    SqlCommand cmd = new SqlCommand("MEANINGFUL_CALCULATION", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    //Using SqlDataAdapter to fill table.
                    using (SqlDataAdapter munitionAdapter = new SqlDataAdapter(cmd))
                    {
                        munitionAdapter.Fill(munitionInfo);
                        munitionAdapter.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //Error logging
                MunitionsErrorHandler("error", "MunitionsDAO", "MunitionCount", ex.Message, ex.StackTrace);
                throw ex;
            }
            //Returning table
            return munitionInfo;
        }
    }
}

