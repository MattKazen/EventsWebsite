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
            var universityEventContext = _context.Rso.Include(r => r.RsoAdmin).Include(r => r.RsoUniversity);
            return View(await universityEventContext.ToListAsync());
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

        // GET: Rsoes/Create
        public IActionResult Create()
        {
            ViewData["RsoAdminId"] = new SelectList(_context.Admins, "AdminId", "AdminId");
            ViewData["RsoUniversityId"] = new SelectList(_context.University, "UniName", "UniName");
            return View();
        }

        // POST: Rsoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RsoId,NumMembers,RsoAdminId,RsoUniversityId,IsPending")] Rso rso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rso);
                await _context.SaveChangesAsync();
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

        // POST: Rsoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        private bool RsoExists(int id)
        {
            return _context.Rso.Any(e => e.RsoId == id);
        }
    }
}
