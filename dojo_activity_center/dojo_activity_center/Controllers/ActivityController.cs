using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using dojo_activity_center.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace dojo_activity_center.Controllers
{
    public class ActivityController : Controller
    {
        private DojoActivityContext _context;
        private User ActiveUser
        {
            get {return _context.Users.Where(u => u.UserId ==HttpContext.Session.GetInt32("id")).FirstOrDefault();}
        }
        public ActivityController(DojoActivityContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("Dashboard")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index", "User");
            }
            return View(InitializeDashboard());
        }
        [HttpGet]
        [Route("NewActivity")]
        public IActionResult NewActivity()
        {
            if(HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }
        [HttpPost]
        [Route("AddActivity")]
        public IActionResult AddActivity(NewActivity newActivity)
        {
            if(HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index", "User");
            }
            List<Activity> ParticipatingActivities = _context.Participate.Where( p => p.UserId == ActiveUser.UserId).Select(u => u.ParticipatingActivities).ToList();
            foreach (var activity in ParticipatingActivities)
            {
                Console.WriteLine(activity.Date);
                DateTime start_date = activity.Date;
                DateTime end_date = start_date.AddMinutes((double)activity.Duration);
                Console.WriteLine(end_date);
                DateTime activity_date = newActivity.Date;
                DateTime activity_end_date = activity_date.AddMinutes((double)newActivity.Duration);
                if(activity_date < start_date && activity_end_date < start_date)
                {
                    newActivity.Date = newActivity.Date;
                    newActivity.Duration = newActivity.Duration;
                }
                else if (activity_date > start_date && activity_date > end_date )
                {
                    newActivity.Date = newActivity.Date;
                    newActivity.Duration = newActivity.Duration;
                }
                else
                {
                    ModelState.AddModelError("Date", "Activity conflicts with another activity");
                }
            }
            List<Activity> CreatedActivities = _context.Activities.Where(u => u.UserId == ActiveUser.UserId).Include( a => a.Coordinator).ToList();
            foreach(var activity in CreatedActivities)
            {
                DateTime start_date = activity.Date;
                DateTime end_date = start_date.AddMinutes((double)activity.Duration);
                Console.WriteLine(end_date);
                DateTime activity_date = newActivity.Date;
                DateTime activity_end_date = activity_date.AddMinutes((double)newActivity.Duration);
                if(activity_date < start_date && activity_end_date < start_date)
                {
                    newActivity.Date = newActivity.Date;
                    newActivity.Duration = newActivity.Duration;
                }
                else if (activity_date > start_date && activity_date > end_date )
                {
                    newActivity.Date = newActivity.Date;
                    newActivity.Duration = newActivity.Duration;
                }
                else
                {
                    ModelState.AddModelError("Date", "Activity conflicts with another activity");
                }
            }
            if(Request.Form["dur"] == "minutes")
            {
                newActivity.Duration =  newActivity.Duration;
            }
            if(Request.Form["dur"] == "hours")
            {
                newActivity.Duration =  newActivity.Duration * 60;
            }
            if(Request.Form["dur"] == "days")
            {
                newActivity.Duration =  newActivity.Duration * 1440;
            }
            if(ModelState.IsValid)
            {
                Activity theActivity = new Activity()
                {
                    Title = newActivity.Title,
                    Date = newActivity.Date,
                    Time = newActivity.Time,
                    Duration = newActivity.Duration,
                    Description = newActivity.Description,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = ActiveUser.UserId,
                };
                _context.Add(theActivity);
                _context.SaveChanges();
                Activity currentActivity = _context.Activities.Where(a => a.UserId == ActiveUser.UserId).Last();
                int currentActivityId = currentActivity.ActivityId;
                return Redirect($"ActivityDetails/{currentActivityId}");
            }
            return View("NewActivity");
        }
        [HttpGet]
        [Route("Join/{activity_id}")]
        public IActionResult JoinActivity(int activity_id)
        {
             if(HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index", "User");
            }
            List<Activity> ParticipatingActivities = _context.Participate.Where( p => p.UserId == ActiveUser.UserId).Select(u => u.ParticipatingActivities).ToList();
            Activity currentActivity = _context.Activities.SingleOrDefault(a => a.ActivityId == activity_id);
            foreach (var activity in ParticipatingActivities)
            {
                DateTime start_date = activity.Date;
                DateTime end_date = start_date.AddMinutes((double)activity.Duration);
                Console.WriteLine(end_date);
                DateTime activity_date = currentActivity.Date;
                DateTime activity_end_date = activity_date.AddMinutes((double)currentActivity.Duration);
                if(activity_date < start_date && activity_end_date < start_date)
                {
                    currentActivity.Date = currentActivity.Date;
                    currentActivity.Duration = currentActivity.Duration;
                }
                else if (activity_date > start_date && activity_date > end_date )
                {
                    currentActivity.Date = currentActivity.Date;
                    currentActivity.Duration = currentActivity.Duration;
                }
                else
                {
                    ViewBag.Error = null;
                    TempData["error"] = "Can not join activity because it conflicts with a currenty activity";
                    return Redirect ("/Dashboard");
                }
            }
            Participate newParticipation = new Participate
            {
                UserId = ActiveUser.UserId,
                ActivityId = activity_id
            };
            _context.Participate.Add(newParticipation);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("Leave/{activity_id}")]
        public IActionResult LeaveActivity(int activity_id)
        {
            Participate currentParticipate = _context.Participate.SingleOrDefault( p => p.UserId == ActiveUser.UserId && p.ActivityId == activity_id);
            _context.Participate.Remove(currentParticipate);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("Delete/{activity_id}")]
        public IActionResult DeleteActivity(int activity_id)
        {
            Activity theActivity = _context.Activities.SingleOrDefault(a => a.ActivityId == activity_id);
            _context.Activities.Remove(theActivity);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("ActivityDetails/{activity_id}")]
        public IActionResult ActivityDetails( int activity_id)
        {
            Activity theActivity = _context.Activities.Include(a => a.Participants).ThenInclude(p => p.ParticipatingUsers).SingleOrDefault(a => a.ActivityId == activity_id);
            int num = theActivity.UserId;
            ViewBag.theUser = _context.Users.SingleOrDefault( u => u.UserId == num);
            ViewBag.ActiveUserId = ActiveUser.UserId;
            return View(theActivity);
        }
        public DashboardModels InitializeDashboard()
        {
            List<Activity> ParticipatingActivities = _context.Participate.Where( p => p.UserId == ActiveUser.UserId).Select(u => u.ParticipatingActivities).ToList();
            return new DashboardModels
            {
                AllActivities = _context.Activities.OrderBy(u => u.Date).Include( a => a.Participants).ThenInclude(p => p.ParticipatingUsers).ToList(),
                AllUsers = _context.Users.Include( u => u.CreatedActivities).ToList(),
                User = ActiveUser,
                JoinedActivities = ParticipatingActivities       
            };
        }
    }
}