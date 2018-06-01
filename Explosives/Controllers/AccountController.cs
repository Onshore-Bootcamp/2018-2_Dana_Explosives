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
    public class AccountController : Controller
    {
        //Dependencies
        private UserDAO dataAccess;

        public AccountController()
        {
            string filePath = ConfigurationManager.AppSettings["logPath"];
            dataAccess = new UserDAO(connectionString, filePath);
        }

        private string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

        // GET: Account
        public ActionResult Index()
        {
            List<UserPO> mappedItems = new List<UserPO>();
            try
            {
                //Displaying list of users
                List<UserDO> dataObjects = dataAccess.ViewAllUsers();
                mappedItems = UserMapper.MapDOToPO(dataObjects);
            }
            catch (Exception ex)
            {
                dataAccess.UserErrorHandler("fatal", "AccountController", "Index", ex.Message, ex.StackTrace);
                TempData["Message"] = "List of users could not be displayed";
            }
            //Returns mappedItems to view
            return View(mappedItems);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginPO form)
        {
            ActionResult oResponse = RedirectToAction("Index", "Home");

            //Check if username is valid
            if (ModelState.IsValid)
            {
                try
                {
                    UserDO storedUser = dataAccess.ReadUserByUsername(form.Username);
                    if (storedUser != null && form.Password.Equals(storedUser.Password))
                    {
                        //Give sessionID and role
                        Session["RoleID"] = storedUser.RoleID;
                        Session["Username"] = storedUser.Username;
                        Session["UserID"] = storedUser.UserID;
                        Session.Timeout = 3;
                    }
                    else
                    {
                        oResponse = RedirectToAction("Login", "Account");
                        TempData["Message"] = "Incorrect Username or Password";
                    }
                }
                catch (Exception ex)
                {
                    oResponse = View(form);
                    TempData["Message"] = "Incorrect Username or Password";
                }
            }
            else
            {
                oResponse = View(form);
                ModelState.AddModelError("Password", "Username or Password is incorrect.");
            }

            return oResponse;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserPO form)
        {
            //Declaring local variables
            ActionResult oResponse = RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                try
                {
                    form.RoleID = 3;
                    UserDO dataObject = UserMapper.MapPOtoDO(form);
                    dataAccess.CreateUser(dataObject);

                    TempData["Message"] = $"{form.Username} was created successfully.";
                }
                catch (Exception ex)
                {
                    oResponse = View(form);
                    TempData["Message"] = "Fail";
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
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(UserPO form)
        {
            ActionResult oResponse = RedirectToAction("Index", "Account");
            
            //Validation check
            if (ModelState.IsValid)
            {
                try
                {
                    //Passing dataObjects mapped from PO to DO for CreateUser()
                    UserDO dataObject = UserMapper.MapPOtoDO(form);
                    dataAccess.CreateUser(dataObject);

                    TempData["Message"] = $"{form.Username} was created successfully.";
                }
                catch (Exception ex)
                {
                    oResponse = View(form);
                    TempData["Message"] = "Fail";
                }
            }
            else
            {
                oResponse = View(form);
            }

            return oResponse;
        }

        [HttpGet]
        [SecurityFilter("RoleId", 1)]
        public ActionResult Update(Int64 userID)
        {
            UserPO displayObject = new UserPO();
            ActionResult oResponse = View(displayObject);
            try
            {
                UserDO item = dataAccess.ViewUserByID(userID);
                displayObject = UserMapper.MapDOToPO(item);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "User could not be updated at this time.";
            }
            return View(displayObject);
        }

        [HttpPost]
        public ActionResult Update(UserPO form)
        {
            ActionResult oResponse = RedirectToAction("Index", "Account");

            if (ModelState.IsValid)
            {
                try
                {
                    UserDO dataObject = UserMapper.MapPOtoDO(form);
                    dataAccess.UpdateUser(dataObject);
                    TempData["Message"] = $"{form.Username} was updated successfully";
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
        [SecurityFilter("RoleId", 1)]
        public ActionResult Delete(Int64 userID)
        {
            ActionResult oResponse = RedirectToAction("Index", "Account");
            try
            {
                if (userID != 2)
                {
                    dataAccess.DeleteUser(userID);
                    TempData["Message"] = "User has been deleted.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Prohibited from deleting administrator";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "User could not be deleted at this time.";
            }
            return oResponse;
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }
    }
}