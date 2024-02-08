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

namespace FinalProject.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        /// <summary>
        /// Admin home view page
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult AdminHome()
        {
            ViewBag.Message = "Admin Home page";
            return View();
        }
        /// <summary>
        /// Admin welcome page
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult AdminWelcome()
        {
            string loggedInEmail = (string)Session["emailid"];
            string connectionString = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SPS_GetAdminDetails", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmailAddress", loggedInEmail);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                Admin admin = new Admin();
                if (reader.Read())
                {
                    string photo = reader["Photo"].ToString();
                    ViewData["Img"] = photo;
                    TempData["Oldimg"] = photo;

                    admin.FirstName = reader["FirstName"].ToString();
                    admin.LastName = reader["LastName"].ToString();
                    admin.DateOfBirth = (DateTime)reader["DateOfBirth"];
                    admin.Gender = reader["Gender"].ToString();
                    admin.PhoneNumber = reader["PhoneNumber"].ToString();
                    admin.EmailAddress = reader["EmailAddress"].ToString();
                    admin.Address = reader["Address"].ToString();
                    admin.Country = reader["Country"].ToString();
                    admin.State = reader["State"].ToString();
                    admin.City = reader["City"].ToString();
                    admin.Postcode = reader["Postcode"].ToString();
                    admin.PassportNumber = reader["PassportNumber"].ToString();
                    admin.AdharNumber = reader["AdharNumber"].ToString();
                    admin.Username = reader["Username"].ToString();
                    admin.Password = reader["Password"].ToString();
                }
                connection.Close();
                return View(admin);
            }
        }
        /// <summary>
        /// Change Admin Image
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult AdminImageChange(HttpPostedFileBase file)
        {
            var emailId = (string)Session["emailid"];

            string imgpath = Server.MapPath((string)TempData["Oldimg"]);
            string fileimgpath = imgpath;
            FileInfo fi = new FileInfo(fileimgpath);
            if (fi.Exists)
            {
                fi.Delete();
            }

            if (file != null && file.ContentLength > 0)
            {
                string filename = Path.GetFileName(file.FileName);
                string filepath = Path.Combine(Server.MapPath("/Admin-Images/"), filename);
                file.SaveAs(filepath);

                string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
                using (SqlConnection sqlconn = new SqlConnection(mainconn))
                {
                    sqlconn.Open();
                    SqlCommand sqlcomm = new SqlCommand("SPU_AdminUserPhoto", sqlconn);
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@Photo", "/Admin-Images/" + filename);
                    sqlcomm.Parameters.AddWithValue("@EmailAddress", emailId);
                    sqlcomm.ExecuteNonQuery();
                }
            }

            return RedirectToAction("AdminWelcome", "Admin");
        }

       
        /// <summary>
        /// User Repository
        /// </summary>
        User_Repository user_Repository = new User_Repository();
        /// <summary>
        /// to view user 
        /// </summary>
        [Authorize]
        public ActionResult UsersView()
        {
            var UsersList = user_Repository.GetAllUsers();
            
            if (UsersList.Count == 0)
            {
                TempData["InfoMessage"] = "Currently Users not available in the Database";
            }
            return View(UsersList);
        }

        /// <summary>
        /// GET: Product/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            try
            {
                var user = user_Repository.GetUsersByID(id).FirstOrDefault();
                
                if (user == null)
                {
                    TempData["InfoMessage"] = "Product not available with ID " + id.ToString();
                    return RedirectToAction("UsersView");
                }
                return View(user);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                LogError(logFilePath, ex);
                return View();
            }
        }


        ///GET: User/Delete/5 <summary>
        /// GET: User/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            try
            {
                var product = user_Repository.GetUsersByID(id).FirstOrDefault();
                
                if (product == null)
                {
                    TempData["InfoMessage"] = "User not available with ID " + id.ToString();
                    return RedirectToAction("UsersView");
                }
                return View(product);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                LogError(logFilePath, ex);
                return View();
            }
        }

        /// <summary>
        /// POST: User/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmation(int id, FormCollection collection)
        { 
            try
            {
                string result = user_Repository.DeleteUser(id);
                
                if (result.Contains("deleted"))
                {
                    TempData["SuccessMessage"] = result;
                }
                else
                {
                    TempData["ErrorMessage"] = result;

                }
                return RedirectToAction("UsersView");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                LogError(logFilePath, ex);
                return View();
            }

        }

        /// <summary>
        /// Visa Application repository
        /// </summary>
        VisaApplication_Repository visadata = new VisaApplication_Repository();
        /// <summary>
        /// GET: Applicant view
        /// </summary>
        /// <returns></returns>
         [Authorize]
        public ActionResult ApplicantsView()
        {
            try
            {
                var ApplicantList = visadata.GetAllApplicants();
                
                if (ApplicantList.Count == 0)
                {             
                TempData["InfoMessage"] = "Currently Applicants not available in the Database";
                }
           
            return View(ApplicantList);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                LogError(logFilePath, ex);
                return View();
            }
        }

        /// <summary>
        /// To change applicant status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(int id, string status)
        {
            try
            {                
                if (status == "approve")
                {
                // Update the status to "Approved" in the database
                visadata.UpdateStatus(id, "Approved");
                TempData["SuccessMessage"] = "Application has been approved successfully.";
                }
            else if (status == "reject")
                {
                // Update the status to "Rejected" in the database
                visadata.UpdateStatus(id, "Rejected");
                TempData["SuccessMessage"] = "Application has been rejected.";
                }
            else
                {
                TempData["ErrorMessage"] = "Invalid status specified.";
                }
            // Redirect back to the ApplicantsView page
            return RedirectToAction("ApplicantsView");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                LogError(logFilePath, ex);
                return RedirectToAction("ApplicantsView");
            }
        }

        /// <summary>
        /// GET: Applicant/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ApplicantDetails(int id)
        {
            try
            {
                var applicant = visadata.GetApplicantsByID(id).FirstOrDefault();
                
                if (applicant == null)
                {
                    TempData["InfoMessage"] = "Applicants not available with ID " + id.ToString();
                    return RedirectToAction("ApplicantsView");
                }
                return View(applicant);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                LogError(logFilePath, ex);
                return View();
            }
        }

        /// <summary>
        /// GET: Applicant/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteApplicant(int id)
        {
            try
            {       
                var applicant = visadata.GetApplicantsByID(id).FirstOrDefault();
                
                if (applicant == null)
                {
                    TempData["InfoMessage"] = "Applicant is not available with ID " + id.ToString();
                    return RedirectToAction("ApplicantsView");
                }
                return View(applicant);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                LogError(logFilePath, ex);
                return View();
            }
        }

        /// <summary>
        /// POST: Applicant/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost, ActionName("DeleteApplicant")]
        public ActionResult ApplicantDeleteConfirmation(int id, FormCollection collection)
        {
            try
            {
                string result = visadata.DeleteApplicant(id);
                
                if (result.Contains("deleted"))
                {
                    TempData["SuccessMessage"] = result;
                }
                else
                {
                    TempData["ErrorMessage"] = result;
                }
                return RedirectToAction("ApplicantsView");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
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