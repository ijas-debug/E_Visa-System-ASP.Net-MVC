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
using System.Web.Security;
using FinalProject.Repository;
using BCrypt.Net;
using FinalProject.Common;


namespace FinalProject.Controllers
{
    
    public class UserLoginController : Controller
    {
        ///GET: Login <summary>
        /// GET: Login
        /// </summary>
        /// <returns></returns>
     
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: Login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult Index(LoginClass login)
        {
            try
            {
                string mainConnection = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(mainConnection);

                SqlCommand sqlCommand = new SqlCommand("SPS_FinalLogin", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@EmailAddress", login.EmailAddress);
                sqlCommand.Parameters.AddWithValue("@Password", Password.Encode(login.Password));

                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    string userType = sqlDataReader["Usertype"].ToString();

                    FormsAuthentication.SetAuthCookie(login.EmailAddress, false);
                    Session["emailid"] = login.EmailAddress.ToString();

                    if (userType == "Admin")
                    {
                        return RedirectToAction("AdminHome", "Admin");
                    }
                    else if (userType == "User")
                    {
                        return RedirectToAction("UserHome", "UserLogin");
                    }
                    else
                    {
                        ViewData["message"] = "Invalid Usertype!";
                    }
                }
                else
                {
                    ViewData["message"] = "Username & Password are wrong!";
                }

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                ViewData["message"] = "An error occurred: " + ex.Message;
                LogError(logFilePath, ex);
            }
            return View();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Welcome()
        {
          try
            {
            string loggedInEmail = (string)Session["emailid"];
            string connectionString = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SPS_GetUserDetails", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmailAddress", loggedInEmail);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                UserClass user = new UserClass();
                  
                if (reader.Read())
                    {
                    string image = reader["Photo"].ToString();
                    ViewData["Img"] = image;
                    TempData["Oldimg"] = image;

                    user.FirstName = reader["FirstName"].ToString();
                    user.LastName = reader["LastName"].ToString();
                    user.DateOfBirth = (DateTime)reader["DateOfBirth"];
                    user.Gender = reader["Gender"].ToString();
                    user.PhoneNumber = reader["PhoneNumber"].ToString();
                    user.EmailAddress = reader["EmailAddress"].ToString();
                    user.Address = reader["Address"].ToString();
                    user.Country = reader["Country"].ToString();
                    user.State = reader["State"].ToString();
                    user.City = reader["City"].ToString();
                    user.Postcode = reader["Postcode"].ToString();
                    user.PassportNumber = reader["PassportNumber"].ToString();
                    user.Username = reader["Username"].ToString();
                    user.Password = reader["Password"].ToString();
                    }

                connection.Close();
                return View(user);
            }
        }
            catch (Exception ex)
            {
                // Handle exception or log error
                ViewData["message"] = "An error occurred: " + ex.Message;
                LogError(logFilePath, ex);
            }

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult UserImageChange(HttpPostedFileBase file)
        {
            try
            {
                var emailId = (string)Session["emailid"];

                string imagepath = Server.MapPath((string)TempData["Oldimg"]);
                string fileImagePath = imagepath;
                FileInfo fileInfo = new FileInfo(fileImagePath);
                
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }

                if (file != null && file.ContentLength > 0)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string filepath = Path.Combine(Server.MapPath("/User-Images/"), filename);
                    file.SaveAs(filepath);

                    string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
                    using (SqlConnection sqlconn = new SqlConnection(mainconn))
                    {
                        sqlconn.Open();
                        SqlCommand sqlcomm = new SqlCommand("SPU_UpdateUserPhoto", sqlconn);
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@Photo", "/User-Images/" + filename);
                        sqlcomm.Parameters.AddWithValue("@EmailAddress", emailId);
                        sqlcomm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                ViewData["message"] = "An error occurred: " + ex.Message;
                LogError(logFilePath, ex);
            }
            return RedirectToAction("Welcome", "UserLogin");
        }


        /// <summary>
        /// POST: Login/UpdateUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateUser")]
        [Authorize]
        public ActionResult UpdateUser(UserClass user)
        {
            try
            {
                string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
                using (SqlConnection sqlconn = new SqlConnection(mainconn))

                {
                    sqlconn.Open();
                    SqlCommand sqlcommand = new SqlCommand("SPU_UpdateUser", sqlconn);
                    sqlcommand.CommandType = CommandType.StoredProcedure;
                   
                    sqlcommand.Parameters.AddWithValue("@FirstName", user.FirstName);
                    sqlcommand.Parameters.AddWithValue("@LastName", user.LastName);
                    sqlcommand.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                    sqlcommand.Parameters.AddWithValue("@Gender", user.Gender);
                    sqlcommand.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                    sqlcommand.Parameters.AddWithValue("@Address", user.Address);
                    sqlcommand.Parameters.AddWithValue("@Country", user.Country);
                    sqlcommand.Parameters.AddWithValue("@State", user.State);
                    sqlcommand.Parameters.AddWithValue("@City", user.City);
                    sqlcommand.Parameters.AddWithValue("@Postcode", user.Postcode);
                    sqlcommand.Parameters.AddWithValue("@PassportNumber", user.PassportNumber);
                    sqlcommand.Parameters.AddWithValue("@Username", user.Username);                 
                    sqlcommand.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);
                    sqlcommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                ViewData["message"] = "An error occurred: " + ex.Message;
                LogError(logFilePath, ex);
            }
            return RedirectToAction("Welcome");
        }
        /// <summary>
        /// get change password
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewBag.Message = "Change Password page";
            return View();
        }
        /// <summary>
        /// Post change password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
       [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
         try
        {
        string mainConnection = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
        using (SqlConnection sqlConnection = new SqlConnection(mainConnection))
        {
             sqlConnection.Open();

                    // Retrieve the user's current password from the database based on the user's unique identifier (ID)
                    SqlCommand retrievePasswordCommand = new SqlCommand("SPU_GetUserPassword", sqlConnection);
                    retrievePasswordCommand.CommandType = CommandType.StoredProcedure;
                    retrievePasswordCommand.Parameters.AddWithValue("@ID", model.ID);
                    string storedPassword = retrievePasswordCommand.ExecuteScalar()?.ToString();

             // Compare the entered old password with the retrieved password
            if (model.OldPassword == storedPassword)
            {
                // Update the password in the database using a stored procedure
                SqlCommand updatePasswordCommand = new SqlCommand("SPU_UpdatePassword", sqlConnection);
                updatePasswordCommand.CommandType = CommandType.StoredProcedure;
                updatePasswordCommand.Parameters.AddWithValue("@ID", model.UserID);
                updatePasswordCommand.Parameters.AddWithValue("@NewPassword", Password.Encode(model.NewPassword));
                updatePasswordCommand.ExecuteNonQuery();

                TempData["SuccessMessage"] = "Password updated successfully.";
                return RedirectToAction("Welcome");
            }
            else
            {
                ViewData["ErrorMessage"] = "The entered old password is incorrect.";
                return View(model);
            }
        }
    }
    catch (Exception ex)
    {
        // Handle exception or log error
        ViewData["ErrorMessage"] = "An error occurred while changing the password: " + ex.Message;
        LogError(logFilePath, ex);
        return View(model);
    }
}
        /// <summary>
        /// User home page
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UserHome()
        {
            ViewBag.Message = "User Home page";
            return View();
        }
        /// <summary>
        /// edit user page
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UpdateUser()
        {
            ViewBag.Message = "Update User page";
            return View();
        }
        /// <summary>
        /// Error page
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// GET: VisaApplication
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult VisaApplication()
        {
            return View();
        }

        /// <summary>
        /// Post: VisaApplication
        /// </summary>
        /// <param name="model"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult VisaApplication(VisaApplication model, HttpPostedFileBase file)
        {
            try
            {
            string mainconn = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            SqlCommand sqlCommand = new SqlCommand("SPI_VisaApplication", sqlconn);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@FirstName", model.FirstName);
            sqlCommand.Parameters.AddWithValue("@LastName", model.LastName);
            sqlCommand.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth);
            sqlCommand.Parameters.AddWithValue("@EmailID", model.EmailID);
            sqlCommand.Parameters.AddWithValue("@Phone", model.Phone);
            sqlCommand.Parameters.AddWithValue("@Address", model.Address);
            sqlCommand.Parameters.AddWithValue("@ExpectedDateOfArrival", model.ExpectedDateOfArrival);
            sqlCommand.Parameters.AddWithValue("@ExpectedDateOfDeparture", model.ExpectedDateOfDeparture);
            sqlCommand.Parameters.AddWithValue("@VisaService", model.VisaService);
            sqlCommand.Parameters.AddWithValue("@Gender", model.Gender);
            sqlCommand.Parameters.AddWithValue("@TownCityOfBirth", model.TownCityOfBirth);
            sqlCommand.Parameters.AddWithValue("@CountryOfBirth", model.CountryOfBirth);
            sqlCommand.Parameters.AddWithValue("@CitizenshipNationalIdNo", model.CitizenshipNationalIdNo);
            sqlCommand.Parameters.AddWithValue("@Religion", model.Religion);
            sqlCommand.Parameters.AddWithValue("@EducationalQualification", model.EducationalQualification);
            sqlCommand.Parameters.AddWithValue("@PassportType", model.PassportType);
            sqlCommand.Parameters.AddWithValue("@Nationality", model.Nationality);
            sqlCommand.Parameters.AddWithValue("@PassportNumber", model.PassportNumber);
            sqlCommand.Parameters.AddWithValue("@PlaceOfIssue", model.PlaceOfIssue);
            sqlCommand.Parameters.AddWithValue("@DateOfIssue", model.DateOfIssue);
            sqlCommand.Parameters.AddWithValue("@DateOfExpiry", model.DateOfExpiry);
            sqlCommand.Parameters.AddWithValue("@PassportOrICNo", model.PassportOrICNo);
            sqlCommand.Parameters.AddWithValue("@PortOfArrival", model.PortOfArrival);
            sqlCommand.Parameters.AddWithValue("@ReferenceNameInIndia", model.ReferenceNameInIndia);
            sqlCommand.Parameters.AddWithValue("@ReferenceAddressInIndia", model.ReferenceAddressInIndia);
            sqlCommand.Parameters.AddWithValue("@ReferencePhone", model.ReferencePhone);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            if (file != null && file.ContentLength > 0)
            {
                string filename = Path.GetFileName(file.FileName);
                string imgpath = Path.Combine(Server.MapPath("/Visa-Images/"), filename);
                file.SaveAs(imgpath);
                sqlCommand.Parameters.AddWithValue("@Photo", "/Visa-Images/" + filename);
            }
            else
            {
                sqlCommand.Parameters.AddWithValue("@Photo", DBNull.Value);
            }
            sqlconn.Open();
            sqlCommand.ExecuteNonQuery();
            sqlconn.Close();

            ViewData["Message"] = "Visa Application for " + model.FirstName + " Is Saved Successfully!";
            return View();
        }
            catch (Exception ex)
            {
                // Handle exception or log error
                ViewData["message"] = "An error occurred: " + ex.Message;
                LogError(logFilePath, ex);
                return View();
            }
        }

        /// <summary>
        /// GET: Login
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult CheckStatus()
        {
            return View();
        }

        /// <summary>
        /// POST: Login
        /// </summary>
        /// <param name="visaStatus"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult CheckStatus(VisaStatus visaStatus)
        {
            try
            {
                string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
                SqlConnection sqlconn = new SqlConnection(mainconn);
                SqlCommand sqlcomm = new SqlCommand("SPS_CheckVisaApplication", sqlconn);

                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@EmailID", visaStatus.EmailID);
                sqlcomm.Parameters.AddWithValue("@PassportNumber", visaStatus.PassportNumber);

                sqlconn.Open();
                SqlDataReader sqr = sqlcomm.ExecuteReader();
                
                if (sqr.Read())
                {
                    FormsAuthentication.SetAuthCookie(visaStatus.EmailID, true);
                    Session["emailid"] = visaStatus.EmailID.ToString();
                    return RedirectToAction("UserVisaStatus", "Userlogin");
                }
                else
                {
                    ViewData["message"] = "Email Id and Passport Number combination is incorrect!";
                }

                sqlconn.Close();
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                ViewData["message"] = "An error occurred: " + ex.Message;
                LogError(logFilePath, ex);
            }
            return View();
        }

        /// <summary>
        /// To check visa status
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UserVisaStatus()
        {
            try
            {
            string loggedInEmail = (string)Session["emailid"];
            string connectionString = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SPS_GetEVisaApplicationDetails", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmailID", loggedInEmail);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                
                VisaApplication visaApplication = new VisaApplication();
                if (reader.Read())
                {
                    string photo = reader["Photo"].ToString();
                    ViewData["Img"] = photo;
                    TempData["Oldimg"] = photo;

                    visaApplication.FirstName = reader["FirstName"].ToString();
                    visaApplication.LastName = reader["LastName"].ToString();
                    visaApplication.DateOfBirth = (DateTime)reader["DateOfBirth"];
                    visaApplication.EmailID = reader["EmailID"].ToString();
                    visaApplication.Phone = reader["Phone"].ToString();
                    visaApplication.Address = reader["Address"].ToString();
                    visaApplication.ExpectedDateOfArrival = (DateTime)reader["ExpectedDateOfArrival"];
                    visaApplication.ExpectedDateOfDeparture = (DateTime)reader["ExpectedDateOfDeparture"];
                    visaApplication.VisaService = reader["VisaService"].ToString();
                    visaApplication.Gender = reader["Gender"].ToString();
                    visaApplication.TownCityOfBirth = reader["TownCityOfBirth"].ToString();
                    visaApplication.CountryOfBirth = reader["CountryOfBirth"].ToString();
                    visaApplication.CitizenshipNationalIdNo = reader["CitizenshipNationalIdNo"].ToString();
                    visaApplication.Religion = reader["Religion"].ToString();
                    visaApplication.EducationalQualification = reader["EducationalQualification"].ToString();
                    visaApplication.PassportType = reader["PassportType"].ToString();
                    visaApplication.Nationality = reader["Nationality"].ToString();
                    visaApplication.PassportNumber = reader["PassportNumber"].ToString();
                    visaApplication.PlaceOfIssue = reader["PlaceOfIssue"].ToString();
                    visaApplication.DateOfIssue = (DateTime)reader["DateOfIssue"];
                    visaApplication.DateOfExpiry = (DateTime)reader["DateOfExpiry"];
                    visaApplication.PassportOrICNo = reader["PassportOrICNo"].ToString();
                    visaApplication.PortOfArrival = reader["PortOfArrival"].ToString();
                    visaApplication.ReferenceNameInIndia = reader["ReferenceNameInIndia"].ToString();
                    visaApplication.ReferenceAddressInIndia = reader["ReferenceAddressInIndia"].ToString();
                    visaApplication.ReferencePhone = reader["ReferencePhone"].ToString();

                    visaApplication.Status = reader["Status"].ToString();
                }
                connection.Close();
                return View(visaApplication);
            }
        }
            catch (Exception ex)
            {
                // Handle exception or log error
                ViewData["message"] = "An error occurred: " + ex.Message;
                LogError(logFilePath, ex);
                return View(new VisaApplication()); // Return an empty VisaApplication object or handle the error accordingly
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
