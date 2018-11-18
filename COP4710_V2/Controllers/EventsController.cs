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
			var eventContext =_context.Events;

			var UniversityId = getUsersUniversityId();
			

			//Split events into Pending and nonPending
			if (isUserSuper())
			{
				//Grabs all NonPending Events
				var AllNonPendingEventContext = eventContext
												.Include(e => e.Location)
													.Where(e => !((bool)e.IsPending));

				//Grabs all Pending Events (Only Super Admin needs to see)
				var AllPendingEventContext = eventContext
												.Include(e => e.Location)
												.Where(e => ((bool)e.IsPending));

				var UniversityPendingEventsContext = (from e in AllPendingEventContext
													  from a in _context.Admins
													  from au in _context.AspNetUsers
													  from u in _context.UserUniversity
														 where e.ContactEmail == a.AdminEmail
														 where a.AdminEmail == au.UserName
														 where au.Id == u.StudentId
														 where u.UniversityId == getUsersUniversityId()
															select e)
															.Include(e => e.Location);

				var UniversityNonPendingEventsContext = (from e in AllNonPendingEventContext
														 from a in _context.Admins
														 from au in _context.AspNetUsers
														 from u in _context.UserUniversity
														  where e.ContactEmail == a.AdminEmail
														  where a.AdminEmail == au.UserName
														  where au.Id == u.StudentId
														  where u.UniversityId == getUsersUniversityId()
															select e)
																.Include(e => e.Location);




				ViewBag.NonPendingEvents = await UniversityNonPendingEventsContext.ToListAsync();

				return View("EventsApprove", await UniversityPendingEventsContext.ToListAsync());
			}

			//Split Events into Public and Private for Users and admins
			else
			{
				//Grabs all Approved Public Events
				var PublicEventContext = eventContext
												.Include(e => e.PubEvents)
												.Include(e => e.Location)
													.Where(e => !(bool)e.IsPending)
													.Where(e => e.EventId == e.PubEvents.PublicEventId);

				//Grabs all Approved Private Events
				var PrivEventContext = eventContext
												.Include(e => e.PrivEvents)
												.Include(e => e.Location)
													.Where(e => !(bool)e.IsPending)
													.Where(e => e.EventId == e.PrivEvents.PrivateEventId)
													.Where(e => e.PrivEvents.EventUniversityId == UniversityId);

				// Save Private Events into Viewbag
				ViewBag.PrivateEvents = await PrivEventContext.ToListAsync();
				ViewBag.UserUniversityId = UniversityId;

				// Pass Public Events to view and save Private Events into Viewbag
				if (isUserAdmin())
				{
					return View("IndexForAdmins", await PublicEventContext.ToListAsync());
				}


				//If user is not admin or SuperAdmin, direct them to view without create button;	
				else
				{
					return View("IndexForUsers", await PublicEventContext.ToListAsync());
				}
			}
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

			if (CheckIfEventIsTimeRestricted(events))
			{
				ViewBag.ErrorMessage = "Sorry there is currently an event scheduled at that location during that time";
				return View("CustomErrorViewMessage");
			}

			if (ModelState.IsValid)
            {
				//Grabs the users Selected Event Type (pub/priv/RSO)
				String EventType = Request.Form["EventType"].ToString();

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
					var usersUniversityId = getUsersUniversityId();

					//Create New Private Event Model
					PrivEvents newPrivEvent = new PrivEvents();
					newPrivEvent.PrivateEventId = events.EventId;
					newPrivEvent.EventUniversityId = usersUniversityId;

					//Adds to Pending and private table
					_context.Add(newPendingEvent);
					_context.Add(newPrivEvent);
				}

			
				await _context.SaveChangesAsync();

				//Selects all nonPendingEvents
				//QUERY CAN BE PUT AS STORED PR
				var nonPendingEventContext = from b in _context.Events
											 where (bool)!b.IsPending
											 select b;

			//	return View("IndexForAdmins",  await nonPendingEventContext.ToListAsync());

			}

			ViewData["LocationId"] = new SelectList(_context.EventLocation, "LocationId", "LocationId", events.LocationId);

			return RedirectToAction(nameof(Index));
		}

		private String getUsersUniversityId()
		{
			return _context.UserUniversity
								.Where(u => u.StudentId == getUserID())
								.Select(u => u.UniversityId).First();
		}

		public IActionResult CreateRsoEvent()
		{

			String userEmail = User.Identity.Name;

			var currentUser = _context.AspNetUsers
										.Include(r => r.Admins)
										.Where(b => b.Email == userEmail)
										.FirstOrDefault();

			var RsoCurrentUserIsAdmin = _context.Rso
												.Where(r => r.RsoAdmin.AdminEmail == userEmail)
													.ToList();

			//Preloads Users Email and Phone in contact info
			ViewData["AdminsRsos"] = RsoCurrentUserIsAdmin;
			ViewData["UserEmail"] = currentUser.UserName;
			ViewData["UserPhone"] = currentUser.PhoneNumber;

			//Grabs list of Possible Locations to put the Event at
			ViewData["LocationId"] = new SelectList(_context.EventLocation, "LocationId", "LocationName");

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateRsoEvent([Bind("EventId,LocationId,EventName,StartTime,StartDay,StartMonth,EventDesc,Category,ContactPhone,ContactEmail")]Events events)
		{

			if (CheckIfEventIsTimeRestricted(events))
			{
				ViewBag.ErrorMessage = "Sorry there is currently an event scheduled at that location during that time";
				return View("CustomErrorViewMessage");
			}

			if (ModelState.IsValid)
			{
				//Grabs the users Selected Event Type (pub/priv/RSO)
				int RsoId = int.Parse(Request.Form["RsoId"]);
				
				//Need to add Event to Events Table before able to add RsoEvent
				await _context.AddAsync(events);
				await _context.SaveChangesAsync();

				events.ContactEmail = User.Identity.Name;

				RsoEvents NewRsoEvent = new RsoEvents();
				NewRsoEvent.Rso = RsoId;
				NewRsoEvent.RsoEventId = events.EventId;
				

				await _context.AddAsync(NewRsoEvent);

				await _context.SaveChangesAsync();

				//Selects all nonPendingEvents
				//QUERY CAN BE PUT AS STORED PR
				var nonPendingEventContext = from b in _context.Events
											 where (bool)!b.IsPending
											 select b;

				return View("IndexForAdmins", await nonPendingEventContext.ToListAsync());

			}

			ViewData["LocationId"] = new SelectList(_context.EventLocation, "LocationId", "LocationId", events.LocationId);

			return RedirectToAction(nameof(Index));
		}

		private Boolean CheckIfEventIsTimeRestricted(Events events)
		{
			int? Time = events.StartTime;
			int? Day = events.StartDay;
			int? Month = events.StartMonth;
			int? Location = events.LocationId;

		
			return _context.Events
								.Where(x => x.StartMonth == Month)
								.Where(x => x.StartDay == Day)
								.Where(x => x.StartTime == Time)
								.Where(x => x.LocationId == Location).Any();
	
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


			//_context.AspNetUsers.Where(u => u.UserName == email).First().Id;

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
