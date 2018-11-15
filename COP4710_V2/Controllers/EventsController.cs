using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using COP4710_V2.Models;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace COP4710_V2.Controllers
{
    public class EventsController : Controller
    {
        private readonly UniversityEventContext _context;

        public EventsController(UniversityEventContext context)
        {
            _context = context;
        }

        // GET: Events
		// Grabs all the events from the Database and directs user to Index 
		// View depending on admin or not (create option)
        public async Task<IActionResult> Index()
        {
			//Grabs all events in Events Table
			var eventContext =_context.Events.Include(e => e.Location);

			//Selects all nonPendingEvents
			var nonPendingEventContext = from b in eventContext
										 where (bool) !b.IsPending
										 select b;
			
			// Selects All Pending Events from Events Table
			var PendingEventContext =    from b in eventContext
										 where (bool)b.IsPending
										 select b;

			ViewBag.NonPendingEvents= await nonPendingEventContext.ToListAsync();

			//If the user is an admin, direct them to view with Create Button
			if (isUserAdmin())
				return View("IndexForAdmins", await nonPendingEventContext.ToListAsync());


			//If the user is a super admin, grab all PendingEvents
			if (isUserSuper())
			{
				return View("EventsApprove", await PendingEventContext.ToListAsync());
			}

			//If user is not admin or SuperAdmin, direct them to view without create button;	
			else
				return View("IndexForUsers", await eventContext.ToListAsync());
        }


        // GET: Events/Create
		// ONLY admins can Create so No need to check
        public IActionResult Create()
        {

			String userEmail = User.Identity.Name;

			var currentUser = _context.AspNetUsers
					.Where(b => b.Email == userEmail)
					.FirstOrDefault();

			//Preloads Users Email and Phone in contact info
			ViewData["UserEmail"] = userEmail;
			ViewData["UserPhone"] = currentUser.PhoneNumber;

			
			//Grabs list of Possible Locations to put the Event at
			ViewData["LocationId"] = new SelectList(_context.EventLocation, "LocationId", "LocationName");
			
			return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,LocationId,EventName,StartTime,StartDay,StartMonth,EventDesc,Category,ContactPhone,ContactEmail")]Events events)
        {
			if (ModelState.IsValid)
            {

				events.IsPending = true;
				events.ContactEmail = User.Identity.Name;
				//Add Created Event to Events Table
				await _context.AddAsync(events);

				//If NonRSO Event --> Insert PendingEvent Model into Pending Events Table
				//Approver remains null until SuperAdmin Approves
				PendingEvents newPendingEvent = new PendingEvents();
				newPendingEvent.PendingEventId = events.EventId;
				newPendingEvent.CreatorId = getUserID();
				newPendingEvent.ApproverId = null;

				//Grabs the users Selected Event Type (pub/priv/RSO)
				String EventType = Request.Form["EventType"].ToString();

				if (EventType == "Public Event")
				{
					PubEvents newPubEvent = new PubEvents();
					newPubEvent.PublicEventId = events.EventId;

					//Add to Pending and public table
					_context.Add(newPendingEvent);
					_context.Add(newPubEvent);
				}

				else if (EventType == "Private Event")
				{
					PrivEvents newPrivEvent = new PrivEvents();
					newPrivEvent.PrivateEventId = events.EventId;

					//Adds to Pending and private table
					_context.Add(newPendingEvent);
					_context.Add(newPrivEvent);
				}
				//RSO Event --> Automatically created
				//Need to check for User's RSO affiliation and add to RsoEventsTable
				else if(EventType == "RSO Event")
				{
					RsoEvents EventRSO = new RsoEvents();
					EventRSO.RsoEventId = events.EventId;

					_context.Add(EventRSO);
				}
				
				//Save the Event into event table
				await _context.AddAsync(events);

				await _context.SaveChangesAsync();

				//Selects all nonPendingEvents
				//QUERY CAN BE PUT AS STORED PR
				var nonPendingEventContext = from b in _context.Events
											 where (bool)!b.IsPending
											 select b;

				return View("IndexForAdmins",  await nonPendingEventContext.ToListAsync());

			}

			ViewData["LocationId"] = new SelectList(_context.EventLocation, "LocationId", "LocationId", events.LocationId);

			return RedirectToAction(nameof(Index));
		}

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Events.FindAsync(id);
            if (events == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.EventLocation, "LocationId", "LocationId", events.LocationId);
            return View(events);
        }



        private bool EventsExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }

		//Possibly Going to use to Make Drop Downs for Creating Events
		private void initilazeTimesAndDate(int Earliest, int Latest)
		{
			List<int> TimeRange = new List<int>();
			List<int> DaysRange = new List<int>();
			List<int> MonthsRange = new List<int>();

			for (int i = 1; i <= 31; i++)
			{
				DaysRange.Add(i);
			}

			for (int i = 1; i <= 12; i++)
			{
				MonthsRange.Add(i);
			}

			while (Earliest <= Latest)
			{
				TimeRange.Add(Earliest);
				Earliest++;
			}

			ViewBag.Months(MonthsRange);
			ViewBag.Days(DaysRange);
			ViewBag.StartTimes(TimeRange);
		}

		// POST: Events/Approve
		[ActionName("EventsApprove")]
		public async Task<IActionResult> EventsApprove(int? id)
		{
			
			String UserID = getUserID();
			
			//Grabs PendingEvent Model that was selected
			var SelectedEvent = await _context.PendingEvents.FindAsync(id);
	

			//Removes it from Table
			 _context.PendingEvents.Remove(SelectedEvent);

			//Updates Removcal
			await _context.SaveChangesAsync();

			//Adds SuperModelID to PendingEventModel
			SelectedEvent.ApproverId = UserID;

			// Adds the Inserted Model back into the table 
			// Causing Triggers to Update IsPending to 0 
			// Thus Removing PendingEvent Entity from the 
			// PendingEventTable by another Trigger
			await _context.PendingEvents.AddAsync(SelectedEvent);

			await _context.SaveChangesAsync();

			return RedirectToAction("Index", "Events", "");
		}

	private String getUserID()
		{

			String email = User.Identity.Name;

			return _context.AspNetUsers.FromSql("emailtoID '" + email + "'").FirstOrDefault().Id;
		}

		private Boolean isUserAdmin()
		{
			String userId = getUserID();

			return _context.Admins.FromSql("isUserAdmin '" + userId + "'").ToList().Any();
		}


		private Boolean isUserSuper()
		{
			String userId = getUserID();

			return _context.Superadmins.FromSql("isUserSuper '" + userId + "'").ToList().Any();
		}

		// GET: Events/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var events = await _context.Events
				.Include(e => e.Location)
				.FirstOrDefaultAsync(m => m.EventId == id);
			if (events == null)
			{
				return NotFound();
			}

			return View(events);
		}


		// GET: Events/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var events = await _context.Events
				.Include(e => e.Location)
				.FirstOrDefaultAsync(m => m.EventId == id);
			if (events == null)
			{
				return NotFound();
			}

			return View(events);
		}

		// POST: Events/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var events = await _context.Events.FindAsync(id);
			_context.Events.Remove(events);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}


		// POST: Events/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("EventId,LocationId,EventName,StartTime,StartDay,StartMonth,EventDesc,Category,ContactPhone,ContactEmail")] Events events)
		{
			if (id != events.EventId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(events);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!EventsExists(events.EventId))
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
			ViewData["LocationId"] = new SelectList(_context.EventLocation, "LocationId", "LocationId", events.LocationId);
			return View(events);
		}
	}
}
