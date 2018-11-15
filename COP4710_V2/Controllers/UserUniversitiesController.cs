using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using COP4710_V2.Models;

namespace COP4710_V2.Controllers
{
	public class UserUniversitiesController : Controller
	{
		private readonly UniversityEventContext _context;

		public UserUniversitiesController(UniversityEventContext context)
		{
			_context = context;
		}

		// GET: UserUniversities
		public async Task<IActionResult> Index()
		{

			var universityEventContext = _context.UserUniversity
												 .Include(u => u.Student)
												 .Include(u => u.University);


			return View(await universityEventContext.ToListAsync());
		}

	}
}