using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using COP4710_V2.Models;
using System.Data;
using System.Data.SqlClient;



namespace COP4710_V2.Controllers
{
    public class HomeController : Controller
    {
		private String ConnectionString = "server=cop4017-2.database.windows.net;database=University Event;" +
											"uid=dbadmin;pwd=Ucfdbs!!";

		// A method that can be called by passing in a query and it will return a DataTable
		// for however the table needs to be processed
		DataTable GetDataFromQuery(string query)
		{
			SqlDataAdapter adap =
				 new SqlDataAdapter(query, ConnectionString);
			DataTable data = new DataTable();
			adap.Fill(data);
			return data;
		}

		// Queries the database for all admins and returns a DataTable
		DataTable GetAdmins()
		{
			DataTable data = GetDataFromQuery("SELECT * FROM admins");
			return data;
		}

		// Extracts the Table of Admins into a List of AdminID's
		private List<String> AdminIDToList(DataTable Table)
		{
			List<String> AdIdList = new List<String>();

			foreach (DataRow dr in Table.Rows)
			{
				AdIdList.Add( (String) dr["AdminId"]); 
			
			}
			return AdIdList;
		}

		// Uses the logged in users Email (Identity.name) to find the corresponding
		// ID in the database. The user's ID is then placed into the ViewBag
		private void GetCurrentUserID()
		{
			String userEmail = User.Identity.Name;
            String Query = "Select Id From AspNetUsers Where Email = '" + userEmail + "'";

			DataTable table = GetDataFromQuery(Query);

			String UserID = table.Rows[0][0].ToString();
			ViewBag.UserID = UserID;
		}

		// Directs user to /Views/Home/ShowAdmins where
		// they are able to see a list of all adminsID's
		// and their userID
		public IActionResult showAdmins()
		{
			//Places UserID into ViewBag.UserID
			GetCurrentUserID();
			
			List<String> AdminList = AdminIDToList(GetAdmins());
			
			ViewBag.Admins = AdminList;

			//If the Current UserID is not in the list of admins then return to previous view
			if (!AdminList.Contains(ViewBag.UserID))
				return null;

			
			return View();
		}

		

		public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
