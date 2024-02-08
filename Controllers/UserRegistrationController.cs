using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using FinalProject.Repository;
using FinalProject.Common;

namespace FinalProject.Controllers
{
    public class UserRegistrationController : Controller
    {
        /// <summary>
        /// GET: UserRegistration
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Post: UserRegistration
        /// </summary>
        /// <param name="userClass"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(UserClass userClass, HttpPostedFileBase file)
        {
            try
            {
            string mainconn = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string encodedPassword = Password.Encode(userClass.Password);

            SqlCommand sqlCommand = new SqlCommand("SPI_InsertUserReg", sqlconn);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@FirstName", userClass.FirstName);
            sqlCommand.Parameters.AddWithValue("@LastName", userClass.LastName);
            sqlCommand.Parameters.AddWithValue("@DateOfBirth", userClass.DateOfBirth);
            sqlCommand.Parameters.AddWithValue("@Gender", userClass.Gender);
            sqlCommand.Parameters.AddWithValue("@PhoneNumber", userClass.PhoneNumber);
            sqlCommand.Parameters.AddWithValue("@EmailAddress", userClass.EmailAddress);
            sqlCommand.Parameters.AddWithValue("@Address", userClass.Address);
            sqlCommand.Parameters.AddWithValue("@Country", userClass.Country);
            sqlCommand.Parameters.AddWithValue("@State", userClass.State);
            sqlCommand.Parameters.AddWithValue("@City", userClass.City);
            sqlCommand.Parameters.AddWithValue("@Postcode", userClass.Postcode);
            sqlCommand.Parameters.AddWithValue("@PassportNumber", userClass.PassportNumber);
            sqlCommand.Parameters.AddWithValue("@Username", userClass.Username);
            sqlCommand.Parameters.AddWithValue("@Password", encodedPassword);
   
            if (file != null && file.ContentLength > 0)
            {
                string filename = Path.GetFileName(file.FileName);
                string imgpath = Path.Combine(Server.MapPath("/User-Images/"), filename);
                file.SaveAs(imgpath);
                sqlCommand.Parameters.AddWithValue("@Photo", "/User-Images/" + filename);
            } 
            else
            {
                sqlCommand.Parameters.AddWithValue("@Photo", DBNull.Value);
            }
            sqlconn.Open();
            sqlCommand.ExecuteNonQuery();
            sqlconn.Close();

            ViewData["Message"] = "User Record " + userClass.FirstName + " Is Saved Successfully!";
            return View();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "An error occurred while saving the user record: " + ex.Message;
                LogError(logFilePath, ex);
                return View();
            }
        }

        /// <summary>
        /// ErrorLog file
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