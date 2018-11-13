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
    public class UniversitiesController : Controller
    {
        private readonly UniversityEventContext _context;

        public UniversitiesController(UniversityEventContext context)
        {
            _context = context;
        }

		//Given UserEmail returns Corresponding ID
		private String getIdFromEmail()
		{
			String email = "'" + User.Identity.Name+ "'" ;

			AspNetUsers currentUser = _context.AspNetUsers.FromSql("emailtoID " + email).FirstOrDefault();

			return currentUser.Id;
		}

		// GET: Universities
		public async Task<IActionResult> Index()
        {

			var unis = await _context.University.ToListAsync();

			// If user is a Super Admin return("IndexforSAdmins")
			var currentUserEmail = User.Identity.Name;

			var currentUserID = getIdFromEmail();
			
			// Queries SAdmin table for Current User ID
			var isUserSAdmin = (from A in _context.Superadmins
							   where A.SuperAdminId == currentUserID
							   select A.SuperAdminId);
			
			//If table is not empty
			if(isUserSAdmin.Any())
				return View("IndexForSAdmins", unis);

			//If user is NOT return("IndexForUser");	
			else
				return View("IndexForUsers", unis);
        }

        // GET: Universities/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var university = await _context.University
                .FirstOrDefaultAsync(m => m.UniName == id);
            if (university == null)
            {
                return NotFound();
            }

            return View(university);
        }

        // GET: Universities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Universities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UniName,UniDesc,UniLocation,NumStudents, UniEmail")] University university)
        {
            if (ModelState.IsValid)
            {
				//Adds User key to University Class
				university.CreatorId = getIdFromEmail();

				//Add University to Database
				_context.Add(university);
				

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(university);
        }

        // GET: Universities/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var university = await _context.University.FindAsync(id);
            if (university == null)
            {
                return NotFound();
            }
            return View(university);
        }

        // POST: Universities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UniName,UniDesc,UniLocation,NumStudents")] University university)
        {
            if (id != university.UniName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(university);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UniversityExists(university.UniName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(university);
        }

        // GET: Universities/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var university = await _context.University
                .FirstOrDefaultAsync(m => m.UniName == id);
            if (university == null)
            {
                return NotFound();
            }

            return View(university);
        }

        // POST: Universities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var university = await _context.University.FindAsync(id);
            _context.University.Remove(university);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UniversityExists(string id)
        {
            return _context.University.Any(e => e.UniName == id);
        }
    }
}
