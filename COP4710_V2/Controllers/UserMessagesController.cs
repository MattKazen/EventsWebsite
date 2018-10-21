using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using COP4710_V2.Data;
using COP4710_V2.Models;

namespace COP4710_V2.Controllers
{
    public class UserMessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserMessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserMessages
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserMessages.ToListAsync());
        }

        // GET: UserMessages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userMessages = await _context.UserMessages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userMessages == null)
            {
                return NotFound();
            }

            return View(userMessages);
        }

        // GET: UserMessages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserMessages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Message,From")] UserMessages userMessages)
        {
            if(userMessages.From == null)
            {
                userMessages.From = User.Identity.Name;
            }
            if (ModelState.IsValid)
            {
                _context.Add(userMessages);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userMessages);
        }

        // GET: UserMessages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userMessages = await _context.UserMessages.FindAsync(id);
            if (userMessages == null)
            {
                return NotFound();
            }
            return View(userMessages);
        }

        // POST: UserMessages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Message,From")] UserMessages userMessages)
        {
            if (id != userMessages.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userMessages);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserMessagesExists(userMessages.Id))
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
            return View(userMessages);
        }

        // GET: UserMessages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userMessages = await _context.UserMessages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userMessages == null)
            {
                return NotFound();
            }

            return View(userMessages);
        }

        // POST: UserMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userMessages = await _context.UserMessages.FindAsync(id);
            _context.UserMessages.Remove(userMessages);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserMessagesExists(int id)
        {
            return _context.UserMessages.Any(e => e.Id == id);
        }
    }
}
