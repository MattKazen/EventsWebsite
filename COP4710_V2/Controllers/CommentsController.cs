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

			//gets the Current User
			var CurrentUser = GetCurrentUser();

			// Grab all comments for chosen Event 
			var EventCommentContext = _context.Comments
							     			  .Where(b => b.EventId == SelectedEventId);
			
			// Grab the Selected Event 
			var EventContext = await _context.Events
											 .Where(b => b.EventId == SelectedEventId)
											 .FirstOrDefaultAsync();

			ViewData["CurrentUserId"] = CurrentUser.Id;
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
		public async Task<IActionResult> Edit(int id)
		{

			int EventCommentId = id;

			var comments = await _context.Comments
											.Include(c => c.Event)
										    .Include(c => c.User)
										    .FirstOrDefaultAsync(c => c.CommentId == id);

			Events events = comments.Event;

			ViewData["EventName"] = events.EventName;
			ViewData["Commenter"] = User.Identity.Name;

			

            return View(comments);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("EventId,UserId,Text,Rating,Timestamp")] Comments comments)
        {
 
            if (ModelState.IsValid)
            {

				comments.Timestamp = DateTime.Now;

				_context.Update(comments);

				await _context.SaveChangesAsync();
                
            }

			return RedirectToAction("Index", new { id = comments.EventId });
		}



        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

			var CommentId = id;

			var comments = await _context.Comments
									   	 .Include(c => c.Event)
										 .Include(c => c.User)
										 .FirstOrDefaultAsync(c => c.CommentId == id);

			Events event_ = comments.Event;

			ViewData["EventName"] = event_.EventName;
			
			return View(comments);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

			var CommentId = id;

			var comment = _context.Comments
								  .Where(c => c.CommentId == CommentId);

	//		var comments = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(await comment.FirstOrDefaultAsync());

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index),nameof(Events));
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
		private AspNetUsers GetCurrentUser()
		{
			return _context.AspNetUsers
						   .Where(b => b.Email == User.Identity.Name)
						   .FirstOrDefault();
		}
		private bool CommentsExists(string id)
        {
            return _context.Comments.Any(e => e.UserId == id);
        }
    }
}
