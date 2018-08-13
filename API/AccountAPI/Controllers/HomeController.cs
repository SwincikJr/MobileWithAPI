using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using AccountAPI.Models;

namespace AccountAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public string Login(string username, string password)
        {
            SqlConnection connection = new SqlConnection
            (
                System.Web.Configuration.WebConfigurationManager
                    .ConnectionStrings["EFDbContext"].ConnectionString
            );

            string query = @"SELECT * FROM Users WHERE 
                                UserID = @username AND
                                Password = @password";

            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                command.ExecuteNonQuery();
                SqlDataReader reader = command.ExecuteReader();
                User queryResult = new User { UserID = null };
                if (reader.HasRows)
                {
                    reader.Read();
                    queryResult.UserID = reader["UserID"].ToString(); 
                }
                return JsonConvert.SerializeObject(queryResult);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }
}