using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WhenAmIGettingPresents.Models;

namespace WhenAmIGettingPresents.Controllers
{
    public class HomeController : Controller
    {
        public static DateTime Holiday = new DateTime(DateTime.Now.Year, 12, 25);

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public JsonResult GetNextDatesSource(string birthdateSerialized)
        {
            JObject birthdateData = JObject.Parse(birthdateSerialized);
            int birthMonth = Convert.ToInt32(birthdateData["birthMonth"]);
            int birthDay = Convert.ToInt32(birthdateData["birthDay"]);

            List<PresentsDay> response = new List<PresentsDay>();

            DateTime nextBirthdate;
            bool isBirthDateValid = DateTime.TryParse(DateTime.Now.Year + "-" + birthMonth + "-" + birthDay, out nextBirthdate);

            if (isBirthDateValid)
            {
                DateTime currentDate = DateTime.Now;

                // Next Birthdate
                if (nextBirthdate < currentDate)
                {
                    nextBirthdate = nextBirthdate.AddYears(1);
                }
                TimeSpan timeUntilNextBirthday = nextBirthdate - currentDate;
                PresentsDay nextBirthdayWait = new PresentsDay();
                nextBirthdayWait.Name = "Days until Next Birthday";
                nextBirthdayWait.DaysLeft = Math.Abs(timeUntilNextBirthday.Days);
                response.Add(nextBirthdayWait);

                // Next Holiday presents
                if (currentDate.Date > Holiday.Date)
                {
                    Holiday = Holiday.AddYears(1);
                }
                TimeSpan timeUntilNextHoliday = Holiday - currentDate;
                PresentsDay nextHolidayWait = new PresentsDay();
                nextHolidayWait.Name = "Days until Next Holiday season";
                nextHolidayWait.DaysLeft = Math.Abs(timeUntilNextHoliday.Days);
                response.Add(nextHolidayWait);

                return Json(response);
            }
            else
            {
                object errorResponse = new { error = "Invalid Birthdate" };
                return Json(errorResponse);
            }
        }
    }
}