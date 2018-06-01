using DataLayer;
using DataLayer.Models;
using Explosives.Custom;
using Explosives.Mapping;
using Explosives.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Explosives.Controllers
{
    public class VideosController : Controller
    {

        private VideosDAO dataAccess;

        public VideosController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            string filePath = ConfigurationManager.AppSettings["logPath"];
            dataAccess = new VideosDAO(connectionString, filePath);
        }

        // GET: Videos
        public ActionResult Index(Int64 munitionID)
        {
            List<VideosPO> mappedVideos = new List<VideosPO>();
            try
            {
                //Instantiating and mapping videos by munition ID
                List<VideosDO> videos = dataAccess.ViewVideos(munitionID);
                mappedVideos = VideosMapper.MapDOtoPO(videos);

                //Pass munitionID even if there aren't videos in table
                //if (videos.Count == 0)
                //{
                //    mappedVideos.Add(new VideosPO() { MunitionID = munitionID });
                //}
            }
            catch (Exception ex)
            {

            }
            ViewBag.ID = munitionID;
            return View(mappedVideos);
        }

        [HttpGet]
        [SecurityFilter("RoleId", 1, 2, 3)]
        public ActionResult UploadFile(Int64 munitionID)
        {
            VideosPO videosPO = new VideosPO();
            videosPO.MunitionID = munitionID;
            return View(videosPO);
        }

        [SecurityFilter("RoleId", 1, 2, 3)]
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase uploadedFile, VideosDO video, Int64 munitionID)
        {
            ActionResult oResponse = RedirectToAction("Index", "Videos", new { munitionID });
            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadedFile != null/* && uploadedFile.ContentType == ".mp4"*/)
                    {
                        string pathToSaveTo = Path.Combine(Server.MapPath("/Files/"), uploadedFile.FileName);
                        pathToSaveTo = pathToSaveTo.Replace("\"", "/").Replace("\\", "//");
                        uploadedFile.SaveAs(pathToSaveTo);
                        video.UserID = (Int64)Session["UserID"];
                        video.VideoPath = "~/Files/" + uploadedFile.FileName;
                    }
                    dataAccess.UploadVideo(video);
                }
                catch (Exception ex)
                {

                }
            }
            return oResponse;
        }

        [HttpGet]
        [SecurityFilter("RoleId", 1, 2, 3)]
        public ActionResult Update(Int64 videoID, Int64 munitionID)
        {

            ActionResult oResponse = View(videoID);


            try
            {
                VideosDO videos = dataAccess.ViewVideosByID(videoID);
                VideosPO displayObject = VideosMapper.MapDOtoPO(videos);
                oResponse = View(displayObject);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Video could not be updated at this time.";
            }

            return oResponse;
        }

        [HttpPost]
        [SecurityFilter("RoleId", 1, 2, 3)]
        public ActionResult Update(VideosPO form)
        {
            //Declaring local variables
            ActionResult oResponse = RedirectToAction("Index", "Videos", new { form.VideoID, form.MunitionID });

            if (ModelState.IsValid)
            {
                try
                {
                    VideosDO dataObject = VideosMapper.MapPOtoDO(form);
                    dataAccess.UpdateVideo(dataObject);
                    TempData["Message"] = $"Successfully updated video";
                }
                catch (Exception ex)
                {
                    oResponse = View(form);
                }
            }
            else
            {
                oResponse = View(form);
            }
            return oResponse;
        }

        [HttpGet]
        [SecurityFilter("RoleId", 1, 2)]
        public ActionResult Delete(Int64 munitionID, Int64 videoID)
        {
            ActionResult oResponse = RedirectToAction("Index", "Videos", new { munitionID });
            try
            {
                dataAccess.DeleteVideo(videoID);
                TempData["Message"] = $"Video has been deleted.";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Munition could not be deleted at this time.";

            }
            return oResponse;
        }
    }
}