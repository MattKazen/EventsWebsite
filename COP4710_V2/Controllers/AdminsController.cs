﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using COP4710_V2.Models;
using System.Data;

namespace COP4710_V2.Controllers
{
    public class AdminsController : Controller
    {
        private readonly UniversityEventContext _context;

        public AdminsController(UniversityEventContext context)
        {
            _context = context;
        }

        // GET: Admins
        public async Task<IActionResult> Index()
        {
            var universityEventContext = _context.Admins.Include(a => a.Admin);
            return View(await universityEventContext.ToListAsync());
        }

        // GET: Admins/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var admins = await _context.Admins
                .Include(a => a.Admin)
                .FirstOrDefaultAsync(m => m.AdminId == id);
            if (admins == null)
            {
                return NotFound();
            }

            return View(admins);
        }

        // GET: Admins/Create
        public IActionResult Create()
        {
            ViewData["AdminId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdminId")] Admins admins)
        {
            if (ModelState.IsValid)
            {
                _context.Add(admins);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdminId"] = new SelectList(_context.AspNetUsers, "Id", "Id", admins.AdminId);
            return View(admins);
        }

        // GET: Admins/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admins = await _context.Admins.FindAsync(id);
            if (admins == null)
            {
                return NotFound();
            }
            ViewData["AdminId"] = new SelectList(_context.AspNetUsers, "Id", "Id", admins.AdminId);
            return View(admins);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AdminId")] Admins admins)
        {
            if (id != admins.AdminId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admins);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminsExists(admins.AdminId))
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
            ViewData["AdminId"] = new SelectList(_context.AspNetUsers, "Id", "Id", admins.AdminId);
            return View(admins);
        }

        // GET: Admins/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admins = await _context.Admins
                .Include(a => a.Admin)
                .FirstOrDefaultAsync(m => m.AdminId == id);
            if (admins == null)
            {
                return NotFound();
            }

            return View(admins);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var admins = await _context.Admins.FindAsync(id);
            _context.Admins.Remove(admins);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminsExists(string id)
        {
            return _context.Admins.Any(e => e.AdminId == id);
        }
        
        public async Task<IActionResult> TestDisplay(string id)
        {
            ViewBag.test = id;

			//Current User Email Address
			String UserEmail = User.Identity.Name;

			//Returns AspUserModel for Current user
			var IDD = GetIdFromEmail(UserEmail);
			
			//Saves Current Users Id
			String userId = IDD.Id;

			//Returns List of University Models created by the user
			var Unilist = GetUniversitiesCreated(userId);

			//Returns Boolean if user is Admin
			var isAdmin = IsUserAdmin(userId);

			//Returns Boolean for if user is SuperAdmin
			var isSuper = isUserSuper(userId);


			ViewBag.Unis = Unilist;
			ViewBag.ID = IDD;

			if (isAdmin)
				ViewBag.IsAdmin = " YESS ";
			else
				ViewBag.IsAdmin = " NOO";


			if (isSuper)
				ViewBag.IsSuper = " YESS ";
			else
				ViewBag.IsSuper = " NOO";


			return View("PullTest");
        }

		private AspNetUsers GetIdFromEmail(String email)
		{
			return _context.AspNetUsers.FromSql("emailtoID '" + email + "'" ).FirstOrDefault();
		}

		private List<University> GetUniversitiesCreated(String userId)
		{
			return _context.University
						   .FromSql("findSelfCreatedUniversities '" + userId + "'")
						   .ToList<University>();
		}

		private Boolean IsUserAdmin(String userId)
		{
			return _context.Admins.FromSql("isUserAdmin '" + userId + "'").ToList().Any();
		}


		private Boolean isUserSuper(String userId)
		{
			return _context.Superadmins.FromSql("isUserSuper '" + userId + "'").ToList().Any();
		}
	}
}


