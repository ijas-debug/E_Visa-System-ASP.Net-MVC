using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using FinalProject.Models;

namespace FinalProject.Repository
{
    public class User_Repository
    {

        String connectionString = ConfigurationManager.ConnectionStrings["Myconnection"].ToString();

        ///Get All Users <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        public List<UserClass> GetAllUsers()
        {
            List<UserClass> UserList = new List<UserClass>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SPS_GetAllUsers";
                SqlDataAdapter sqlData = new SqlDataAdapter(command);
                DataTable dataUsers = new DataTable();

                try
                {
                connection.Open();
                sqlData.Fill(dataUsers);
                
                foreach (DataRow datarow in dataUsers.Rows)
                {
                    UserList.Add(new UserClass
                    {
                        ID = Convert.ToInt32(datarow["ID"]),
                        FirstName = datarow["FirstName"].ToString(),
                        LastName = datarow["LastName"].ToString(),
                        DateOfBirth = Convert.ToDateTime(datarow["DateOfBirth"]),
                        Gender = datarow["Gender"].ToString(),
                        PhoneNumber = datarow["PhoneNumber"].ToString(),
                        EmailAddress = datarow["EmailAddress"].ToString(),
                        Address = datarow["Address"].ToString(),
                        Country = datarow["Country"].ToString(),
                        State = datarow["State"].ToString(),
                        City = datarow["City"].ToString(),
                        Postcode = datarow["Postcode"].ToString(),
                        PassportNumber = datarow["PassportNumber"].ToString(),
                        Username = datarow["Username"].ToString(),
                    });
                }
                }
                finally
                {
                    // Ensure the connection is closed and resources are released
                    connection.Close();
                }
            }
            return UserList;
        }


        ///Get All Userss by ID <summary>
        /// Get All Userss by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<UserClass> GetUsersByID(int ID)
        {
            List<UserClass> UserList = new List<UserClass>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SPS_GetUsersByID";
                command.Parameters.AddWithValue("@ID", ID);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                try
                {
                connection.Open();
                sqlDA.Fill(dtProducts);
                
                foreach (DataRow datarow in dtProducts.Rows)
                {
                    UserList.Add(new UserClass
                    {
                        ID = Convert.ToInt32(datarow["ID"]),
                        FirstName = datarow["FirstName"].ToString(),
                        LastName = datarow["LastName"].ToString(),
                        DateOfBirth = Convert.ToDateTime(datarow["DateOfBirth"]),
                        Gender = datarow["Gender"].ToString(),
                        PhoneNumber = datarow["PhoneNumber"].ToString(),
                        EmailAddress = datarow["EmailAddress"].ToString(),
                        Address = datarow["Address"].ToString(),
                        Country = datarow["Country"].ToString(),
                        State = datarow["State"].ToString(),
                        City = datarow["City"].ToString(),
                        Postcode = datarow["Postcode"].ToString(),
                        PassportNumber = datarow["PassportNumber"].ToString(),
                        Username = datarow["Username"].ToString(),
                    });
                }
                }
                finally
                {
                    // Ensure the connection is closed and resources are released
                    connection.Close();
                }
            }
            return UserList;
        }



        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string DeleteUser(int userid)
        {
            string result = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
              try
                {
                SqlCommand command = new SqlCommand("SPD_DeleteUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID",userid);
                command.Parameters.Add("@OUTPUTMESSAGE", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                connection.Open();
                command.ExecuteNonQuery();
                result = command.Parameters["@OUTPUTMESSAGE"].Value.ToString();
                connection.Close();
                }
                finally
                {
                 connection.Close();
                }
            }
            return result;
        }

       

        /// <summary>
        /// Check Status by EmailId and PassportNumber
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="passportNumber"></param>
        /// <returns></returns>
        public string CheckStatus(string emailId, string passportNumber)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
             try
                {
                 SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SPS_CheckVisaApplication";
                command.Parameters.AddWithValue("@EmailId", emailId);
                command.Parameters.AddWithValue("@PassportNumber", passportNumber);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                string status = string.Empty;

                if (reader.Read())
                {
                    status = reader["Status"].ToString();
                }
                reader.Close();
                return status;
                }
                finally
                {  
                  connection.Close();
                }
            }
        }

       

    }
}