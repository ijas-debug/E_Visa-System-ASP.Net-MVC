using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Home page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// About us page
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        /// <summary>
        /// Contactus page
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {   
            ViewBag.Message = "Your contact page.";
            return View();
        }

        /// <summary>
        /// function for logout
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "UserLogin");
        }
        /// <summary>
        /// Error log file
        /// </summary>
        string logFilePath = "C:\\Users\\IJAS\\source\\repos\\FinalProject\\ErrorLog.txt";
        private void LogError(string logFilePath, Exception ex)
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine("Error Date: " + DateTime.Now.ToString());
                writer.WriteLine("Message:" + ex.Message);
                writer.WriteLine("Stack Trace:" + ex.StackTrace);
                writer.WriteLine("-----------------------------------------------");

            }
        }
    }
}