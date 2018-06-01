using Business_Layer.Models;
using BusinessLayer;
using DataLayer;
using DataLayer.Models;
using Explosives.Custom;
using Explosives.Mapping;
using Explosives.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace Explosives.Controllers
{
    public class MunitionsController : Controller
    {
        //Creating dependencies
        private MunitionsDAO dataAccess;
        private CalcBAO businessAccess;

        //Constructor allowing access to SQL, data layer, business layer, and logging
        public MunitionsController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            string filePath = ConfigurationManager.AppSettings["logPath"];
            dataAccess = new MunitionsDAO(connectionString, filePath);
            businessAccess = new CalcBAO();
        }

        /// <summary>
        /// -- Index -- using ViewMunitions(), MunitionCount(), CountVideos()
        /// </summary>
        /// <param name="topicID"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(Int64 topicID, string topic)
        {
            //Intantiating new list to return mappedMunitions
            List<MunitionsPO> mappedMunitions = new List<MunitionsPO>();
            try
            {
                //Filling list of objects using method in data layer
                List<MunitionsDO> munitions = dataAccess.ViewMunitions(topicID);

                //Mapping to a list of presentation objects
                mappedMunitions = MunitionsMapper.MapDOtoPO(munitions);

                //Pass topicID even if there aren't munitions in table
                if (munitions.Count == 0)
                {
                    mappedMunitions.Add(new MunitionsPO() { TopicID = topicID });
                }

                //Using CoundVideos() to get a count of each munitionID
                List<MunitionBO> videoCount = businessAccess.CountVideos(dataAccess.MunitionCount());

                //Putting list into viewbag as a list to display
                ViewBag.Videos = new List<MunitionBO>();
                foreach(MunitionBO munition in videoCount)
                {
                    ViewBag.Videos.Add(new MunitionBO() { MunitionID = munition.MunitionID, VideoCount = munition.VideoCount });
                }

                //Creating viewbag for topic
                ViewBag.Topic = topic;
            }
            catch (Exception ex)
            {
                //Error message
                TempData["ErrorMessage"] = "There was an issue displaying Munitions";
            }
            //Returning list of munitions to be displayed in Index view
            return View(mappedMunitions);
        }

        /// <summary>
        /// -- DropDown List -- using ViewTopics()
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult RenderTopics()
        {
            //Getting list of topics from data layer
            List<TopicDO> topics = dataAccess.ViewTopics();
            List<TopicPO> mappedTopics = MunitionsMapper.MapDOtoPO(topics);

            //Returning list of topics for DDL
            return PartialView("_TopicsButton", topics);
        }

        /// <summary>
        /// Get -- CREATE -- pass topicID to form
        /// </summary>
        /// <param name="topicID"></param>
        /// <returns></returns>
        [HttpGet]
        [SecurityFilter("RoleId", 1, 2)]
        public ActionResult Create(int topicID)
        {
            //Intantiating MunitionsPO to get topicID
            MunitionsPO munitions = new MunitionsPO();
            munitions.TopicID = topicID;

            //Returning view
            return View(munitions);
        }

        /// <summary>
        /// Post -- CREATE -- using CreateMunitions()
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [SecurityFilter("RoleId", 1, 2)]
        public ActionResult Create(MunitionsPO form)
        {
            //Redirection to Index page for Munitions
            ActionResult oResponse = RedirectToAction("Index", "Munitions", new { form.TopicID });

            //Checking for valid modelstate
            if (ModelState.IsValid)
            {
                try
                {
                    //Mapping form objects from PO to DO
                    MunitionsDO dataObject = MunitionsMapper.MapPOtoDO(form);

                    //Passing objects to CreateMunitions()
                    dataAccess.CreateMunitions(dataObject);
                    TempData["Message"] = "Munition was created successfully.";
                }
                catch (Exception ex)
                {
                    //Error message
                    oResponse = View(form);
                    TempData["ErrorMessage"] = "Munition was not created";
                }
            }
            //ModelState not valid
            else
            {
                //User filled form out incorrectly
                oResponse = View(form);
            }
            //Return redirect ActionResult 
            return oResponse;
        }

        /// <summary>
        /// Get -- UPDATE -- prefill form using ViewMunitionsByID()
        /// </summary>
        /// <param name="topicID"></param>
        /// <param name="munitionID"></param>
        /// <returns></returns>
        [HttpGet]
        [SecurityFilter("RoleId", 1, 2)]
        public ActionResult Update(Int64 topicID, Int64 munitionID)
        {
            //Viewing munition with specific munitionID selected
            ActionResult oResponse = View(munitionID);
            try
            {
                //Getting munition by ID from data layer and mapping to presentation layer
                MunitionsDO munitions = dataAccess.ViewMunitionsByID(munitionID);
                MunitionsPO displayObject = MunitionsMapper.MapDOtoPO(munitions);

                //Setting topic ID to current topic
                displayObject.TopicID = topicID;
                oResponse = View(displayObject);
            }
            catch (Exception ex)
            {
                //Error message
                TempData["Message"] = "Munition could not be updated at this time.";
            }
            //Return ActionResult 
            return oResponse;
        }

        /// <summary>
        /// Post -- UPDATE -- using UpdateMunitions()
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [SecurityFilter("RoleId", 1, 2)]
        public ActionResult Update(MunitionsPO form)
        {
            //Redirection to Index page for Munitions
            ActionResult oResponse = RedirectToAction("Index", "Munitions", new { form.TopicID });

            //Checking for valid modelstate
            if (ModelState.IsValid)
            {
                try
                {
                    //Mapping form objects from PO to DO
                    MunitionsDO dataObject = MunitionsMapper.MapPOtoDO(form);
                    
                    //Passing objects to UpdateMunitions()
                    dataAccess.UpdateMunitions(dataObject);
                    TempData["Message"] = $"Successfully updated {form.Munition}";
                }
                catch (Exception ex)
                {
                    //Error message
                    oResponse = View(form);
                    TempData["ErrorMessage"] = "Update was unsuccessful";
                }
            }
            //ModelState not valid
            else
            {
                //User filled form out incorrectly
                oResponse = View(form);
            }
            //Return redirect ActionResult 
            return oResponse;
        }

        /// <summary>
        /// -- DELETE -- using DeleteMunitions()
        /// </summary>
        /// <param name="TopicID"></param>
        /// <param name="munitionID"></param>
        /// <returns></returns>
        [HttpGet]
        [SecurityFilter("RoleId", 1, 2)]
        public ActionResult Delete(Int64 topicID, Int64 munitionID)
        {
            //Redirect to index of munitions
            ActionResult oResponse = RedirectToAction("Index", "Munitions", new { topicID });
            try
            {
                //Calling DeleteMunitions() by munitionID
                dataAccess.DeleteMunitions(munitionID);
                TempData["Message"] = "Munition has been deleted.";
            }
            catch (Exception ex)
            {
                //Error message
                TempData["ErrorMessage"] = "Munition could not be deleted at this time.";
            }
            //Return redirect ActionResult
            return oResponse;
        }
    }
}

