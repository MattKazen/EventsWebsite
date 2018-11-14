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
    public class CommentsController : Controller
    {
        private readonly UniversityEventContext _context;

        public CommentsController(UniversityEventContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index(int Id)
        {

			var SelectedEventId = Id;

			// Grab all comments for chosen Event
			var EventCommentContext = _context.Comments
							     			  .Where(b => b.EventId == SelectedEventId);
			
			// Grab the Selected Event 
			var EventContext = await _context.Events
											 .Where(b => b.EventId == SelectedEventId)
											 .FirstOrDefaultAsync();

			//Pass View Name of Event
			ViewData["EventName"] = EventContext.EventName;
			ViewData["EventId"] = SelectedEventId;

			return View(await EventCommentContext.ToListAsync());
        }


        // GET: Comments/Create
        public IActionResult Create(int id)
        {
			var SelectedEventId = id;
			//Get Event Name
			var EventName = _context.Events
									.Where(b => b.EventId == SelectedEventId)
									.FirstOrDefault()
									.EventName;

			ViewData["EventName"] = EventName;
			ViewData["EventId"] = SelectedEventId.ToString();

			return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
			[Bind("EventId, CommentId, Text, Rating")]
			Comments comments)
        {
			//gets the Current Users ID
			var CommenterId =  _context.AspNetUsers
									.Where(b=> b.Email == User.Identity.Name)
									.FirstOrDefault()
									.Id;
			//TimeStamp
			DateTime Time = DateTime.Now;
	
			if (ModelState.IsValid)
            {
				comments.UserId = CommenterId;
				comments.Timestamp = Time;

				_context.Add(comments);
                await _context.SaveChangesAsync();
            }

			return RedirectToAction("Index", new { id = comments.EventId }); 
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comments = await _context.Comments.FindAsync(id);
            if (comments == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", comments.EventId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", comments.UserId);
            return View(comments);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CommentId,EventId,UserId,Text,Rating,Timestamp")] Comments comments)
        {
            if (id != comments.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentsExists(comments.UserId))
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
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", comments.EventId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", comments.UserId);
            return View(comments);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comments = await _context.Comments
                .Include(c => c.Event)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (comments == null)
            {
                return NotFound();
            }

            return View(comments);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var comments = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		// GET: Comments/Details/5
		public async Task<IActionResult> Details(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var comments = await _context.Comments
				.Include(c => c.Event)
				.Include(c => c.User)
				.FirstOrDefaultAsync(m => m.UserId == id);
			if (comments == null)
			{
				return NotFound();
			}

			return View(comments);
		}

		private bool CommentsExists(string id)
        {
            return _context.Comments.Any(e => e.UserId == id);
        }
    }
}
