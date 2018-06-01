using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace DataLayer
{
    public class VideosDAO
    {

        public string currentClass = "VideosDAO";
        private string connectionString;
        private string filePath;

        public VideosDAO(string dataConnection, string path)
        {
            connectionString = dataConnection;
            filePath = path;
        }

        public void UploadVideo(VideosDO video)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand enterCommand = new SqlCommand("UPLOAD_VIDEO", conn);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    
                    enterCommand.Parameters.AddWithValue("@VideoName", video.VideoName);
                    enterCommand.Parameters.AddWithValue("@VideoPath", video.VideoPath);
                    enterCommand.Parameters.AddWithValue("@VideoDescription", video.VideoDescription);
                    enterCommand.Parameters.AddWithValue("@MunitionID", video.MunitionID);
                    enterCommand.Parameters.AddWithValue("@UserID", video.UserID);

                    conn.Open();
                    enterCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                VideosErrorHandler("fatal", "VideosDAO", "CreateNewVideo", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public List<VideosDO> ViewVideos(Int64 MunitionID)
        {
            try
            {
                List<VideosDO> videos = new List<VideosDO>();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand enterCommand = new SqlCommand("DISPLAY_VIDEOS_BY_MUNITION", conn);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue("@MunitionID", MunitionID);
                    conn.Open();

                    DataTable videoInfo = new DataTable();
                    using (SqlDataAdapter videosAdapter = new SqlDataAdapter(enterCommand))
                    {
                        videosAdapter.Fill(videoInfo);
                        videosAdapter.Dispose();
                    }

                    foreach (DataRow row in videoInfo.Rows)
                    {
                        VideosDO mappedRow = MapAllVideos(row);
                        videos.Add(mappedRow);
                    }
                }
                return videos;
            }
            catch (Exception ex)
            {
                VideosErrorHandler("fatal", "VideosDAO", "ViewAllMunitionVideos", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
        public VideosDO MapAllVideos(DataRow dataRow)
        {
            try
            {
                VideosDO video = new VideosDO();

                //Check to see if database object is null
                if (dataRow["VideoID"] != DBNull.Value)
                {
                    video.VideoID = (Int64)dataRow["VideoID"];
                }
                video.VideoName = dataRow["Videoname"].ToString();
                video.VideoPath = dataRow["VideoPath"].ToString();
                video.VideoDescription = dataRow["VideoDescription"].ToString();
                video.MunitionID = (Int64)dataRow["MunitionID"];
                //video.UserID = (Int64)dataRow["UserID"];

                return video;
            }
            catch (Exception ex)
            {
                VideosErrorHandler("fatal", "UserDAO", "MapAllVideos", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
        public VideosDO ViewVideosByID(Int64 videoID)
        {
            VideosDO videos = new VideosDO();
            try
            {

                //Instantiate SqlConnection
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Creating a new SqlCommand to use a stored procedure.
                    SqlCommand enterCommand = new SqlCommand("DISPLAY_VIDEOS_BY_ID", conn);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue("@VideoID", videoID);
                    conn.Open();

                    //Using SqlDataAdapter to get SQL table.
                    DataTable videosInfo = new DataTable();
                    using (SqlDataAdapter videoAdapter = new SqlDataAdapter(enterCommand))
                    {
                        videoAdapter.Fill(videosInfo);
                        videoAdapter.Dispose();
                    }

                    //Mapping row to munition                  
                    foreach (DataRow row in videosInfo.Rows)
                    {
                        videos = MapAllVideos(row);

                    }
                }

            }
            catch (Exception ex)
            {
                VideosErrorHandler("fatal", "MunitionsDAO", "ViewMunitionsByID", ex.Message, ex.StackTrace);
                throw ex;
            }
            return videos;
        }

        public void UpdateVideo(VideosDO videos)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand enterCommand = new SqlCommand("UPDATE_VIDEO", conn);

                    enterCommand.CommandType = CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue(@"VideoID", videos.VideoID);
                    enterCommand.Parameters.AddWithValue("@VideoName", videos.VideoName);
                    enterCommand.Parameters.AddWithValue("@VideoDescription", videos.VideoDescription);

                    conn.Open();
                    enterCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                VideosErrorHandler("fatal", "VideosDAO", "UpdateVideo", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public void DeleteVideo(Int64 videoID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand enterCommand = new SqlCommand("DELETE_VIDEO", conn);

                    enterCommand.CommandType = CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue("@VideoID", videoID);

                    conn.Open();
                    enterCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                VideosErrorHandler("fatal", "VideosDAO", "DeleteVideo", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public void VideosErrorHandler(string level, string currentClass, string currentMethod, string message, string stackTrace = null)
        {

            string errorLog = filePath + "/ErrorLogVideos.txt";

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
                VideosErrorHandler("fatal", "VideosDAO", "VideosErrorHandler", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
    }
}

