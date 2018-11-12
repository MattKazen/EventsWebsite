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
    public class PubEventsController : Controller
    {
        private readonly UniversityEventContext _context;

        public PubEventsController(UniversityEventContext context)
        {
            _context = context;
        }

        // GET: PubEvents
        public async Task<IActionResult> Index()
        {
            var universityEventContext = _context.PubEvents.Include(p => p.PublicEvent);
            return View(await universityEventContext.ToListAsync());
        }

        // GET: PubEvents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pubEvents = await _context.PubEvents
                .Include(p => p.PublicEvent)
                .FirstOrDefaultAsync(m => m.PublicEventId == id);
            if (pubEvents == null)
            {
                return NotFound();
            }

            return View(pubEvents);
        }

        // GET: PubEvents/Create
        public IActionResult Create()
        {
            ViewData["PublicEventId"] = new SelectList(_context.Events, "EventId", "EventId");
            return View();
        }

        // POST: PubEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PublicEventId")] PubEvents pubEvents)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pubEvents);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PublicEventId"] = new SelectList(_context.Events, "EventId", "EventId", pubEvents.PublicEventId);
            return View(pubEvents);
        }

        // GET: PubEvents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pubEvents = await _context.PubEvents.FindAsync(id);
            if (pubEvents == null)
            {
                return NotFound();
            }
            ViewData["PublicEventId"] = new SelectList(_context.Events, "EventId", "EventId", pubEvents.PublicEventId);
            return View(pubEvents);
        }

        // POST: PubEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PublicEventId")] PubEvents pubEvents)
        {
            if (id != pubEvents.PublicEventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pubEvents);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PubEventsExists(pubEvents.PublicEventId))
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
            ViewData["PublicEventId"] = new SelectList(_context.Events, "EventId", "EventId", pubEvents.PublicEventId);
            return View(pubEvents);
        }

        // GET: PubEvents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pubEvents = await _context.PubEvents
                .Include(p => p.PublicEvent)
                .FirstOrDefaultAsync(m => m.PublicEventId == id);
            if (pubEvents == null)
            {
                return NotFound();
            }

            return View(pubEvents);
        }

        // POST: PubEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pubEvents = await _context.PubEvents.FindAsync(id);
            _context.PubEvents.Remove(pubEvents);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PubEventsExists(int id)
        {
            return _context.PubEvents.Any(e => e.PublicEventId == id);
        }
    }
}
