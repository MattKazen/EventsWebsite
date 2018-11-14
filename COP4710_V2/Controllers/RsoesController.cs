using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using COP4710_V2.Models;
using System.Diagnostics;

namespace COP4710_V2.Controllers
{
    public class RsoesController : Controller
    {
        private readonly UniversityEventContext _context;

        public RsoesController(UniversityEventContext context)
        {
            _context = context;
        }

        // GET: Rsoes
        public async Task<IActionResult> Index()
        {
			//Grabs All RS
			//Also note .ThenInclude() Lets you traverse through relationships
			var AllRsoContext = _context.Rso
										.Include(r => r.RsoUniversity)
										.Include(r => r.RsoAdmin)
											.ThenInclude(r => r.Admin);
										

			var PendingRsoContext = _context.PendingRso
											.Include(r => r.PendingRsoCreator)
											.Include(r => r.PendingRsoUniversity);

			var UserId = getCurrentUser().Id;

		/*	var AffiliateRsoContext = _context.Rso
											  .Include(r => r.RsoAdmin)
											  .Include(r => r.RsoUniversity)
											  .Include(r => r.StudentsInRsos)
											  .Where(r => UserId ;
											  */

			return View(await AllRsoContext.ToListAsync());
        }



        // GET: Rsoes/Create
        public IActionResult Create()
        {
			var userEmail = User.Identity.Name;
			//gets the Current Users ID
			var userId = getCurrentUser().Id;

			//Gets the Current Users University
			var userUniversity = _context.UserUniversity
										 .Where(b => b.StudentId == userId)
										 .FirstOrDefault()
										 .UniversityId;



			ViewData["CreatorId"] = userId;
			ViewData["UniversityId"] = userUniversity;

            return View();
        }

		
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RsoId,Name,NumMembers,RsoAdminId,RsoUniversityId")] Rso rso)
        {
            if (ModelState.IsValid)
            {
				rso.IsPending = true;
				//Add Rso To DB so Pending Event can be made
				await _context.AddAsync(rso);

				await _context.SaveChangesAsync();

				// Make a copy of RSO in PendingRso Model and Add to Database
				PendingRso PRso = new PendingRso();
				PRso.PendingRsoId = rso.RsoId;
				PRso.PendingNumMem = rso.NumMembers;
				PRso.PendingRsoCreator = getCurrentUser();
				PRso.PendingRsoUniversityId = rso.RsoUniversityId;

			///In case IsPending doesn't work. 
			/*	//Strip all values from rso (except rsoId)
				rso.NumMembers = null;
				rso.RsoAdminId = null;
				rso.RsoUniversityId = null;
				_context.Update(rso); */


				//Add PendingRso To Table
				await _context.AddAsync(PRso);

				await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

			return RedirectToAction(nameof(Index));
        }

       

        // GET: Rsoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rso = await _context.Rso
                .Include(r => r.RsoAdmin)
                .Include(r => r.RsoUniversity)
                .FirstOrDefaultAsync(m => m.RsoId == id);
            if (rso == null)
            {
                return NotFound();
            }

            return View(rso);
        }

        // POST: Rsoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rso = await _context.Rso.FindAsync(id);
            _context.Rso.Remove(rso);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
		// GET: Rsoes/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var rso = await _context.Rso
				.Include(r => r.RsoAdmin)
				.Include(r => r.RsoUniversity)
				.FirstOrDefaultAsync(m => m.RsoId == id);
			if (rso == null)
			{
				return NotFound();
			}

			return View(rso);
		}

	
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("RsoId,NumMembers,RsoAdminId,RsoUniversityId,IsPending")] Rso rso)
		{
			if (id != rso.RsoId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(rso);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!RsoExists(rso.RsoId))
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
			ViewData["RsoAdminId"] = new SelectList(_context.Admins, "AdminId", "AdminId", rso.RsoAdminId);
			ViewData["RsoUniversityId"] = new SelectList(_context.University, "UniName", "UniName", rso.RsoUniversityId);
			return View(rso);
		}


		// GET: Rsoes/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var rso = await _context.Rso.FindAsync(id);
			if (rso == null)
			{
				return NotFound();
			}
			ViewData["RsoAdminId"] = new SelectList(_context.Admins, "AdminId", "AdminId", rso.RsoAdminId);
			ViewData["RsoUniversityId"] = new SelectList(_context.University, "UniName", "UniName", rso.RsoUniversityId);
			return View(rso);
		}


		private bool RsoExists(int id)
        {
            return _context.Rso.Any(e => e.RsoId == id);
        }

		private AspNetUsers getCurrentUser()
		{
			return _context.AspNetUsers
						   .Where(b => b.Email == User.Identity.Name)
						   .FirstOrDefault();
		}
	}
}
